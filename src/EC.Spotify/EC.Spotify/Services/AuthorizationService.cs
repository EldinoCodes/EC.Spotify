using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace EC.Spotify.Services;

internal class AuthorizationService(ILogger<AuthorizationService> logger, IOptions<SpotifyOptions> spotifyOptions, ISpotifyHttpProvider httpSpotifyProvider, ISpotifyJsonProvider spotifyJsonProvider, IMemoryCache memoryCache) : IAuthorizationService, IDisposable
{
    private readonly ILogger<AuthorizationService> _logger = logger;
    private readonly SpotifyOptions _spotifyOptions = spotifyOptions.Value;
    private readonly ISpotifyHttpProvider _httpSpotifyProvider = httpSpotifyProvider;
    private readonly ISpotifyJsonProvider _spotifyJsonProvider = spotifyJsonProvider;
    private readonly IMemoryCache _memoryCache = memoryCache;

    private const string AuthUri = "https://accounts.spotify.com/authorize";
    private const string TokenUri = "https://accounts.spotify.com/api/token";
    private const string AuthCodeCacheKey = "EC.Spotify.Services.SpotifyAuthCode";
    private const string AuthTokenCacheKey = "EC.Spotify.Services.SpotifyAuthToken";
    private const string AuthStateCacheKey = "EC.Spotify.Services.SpotifyAuthState";

    private readonly SemaphoreSlim _tokenLock = new(1, 1);
    private string? RefreshToken { get; set; }


    public async Task<string?> Validate(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("Validate called");

            var uri = AuthorizationCodeUrl();

            var authCode = await AuthorizationCodeGetAsync(cancellationToken);
            if (authCode is null) return uri;

            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("Validate: authorization code found, fetching token");

            var authToken = await AuthorizationTokenGetAsync(cancellationToken);
            if (authToken is null) return uri;

            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("Validate: authorization token obtained successfully");

            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Validate failed");
            throw;
        }
    }

    public string? AuthorizationCodeUrl()
    {
        try
        {
            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationCodeUrl called");

            var state = Guid.NewGuid().ToString();
            _memoryCache.Set(AuthStateCacheKey, state, TimeSpan.FromMinutes(10));

            return AuthUri.ToUri(new() {
                { "client_id", _spotifyOptions.ClientId },
                { "response_type", "code"},
                { "scope", string.Join(" ", _spotifyOptions.Scopes) },
                { "redirect_uri", _spotifyOptions.RedirectUri },
                { "state", state }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthorizationCodeUrl failed");
            throw;
        }
    }

    public async Task<string?> AuthorizationCodeGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationCodeGetAsync called");

            if (!_memoryCache.TryGetValue(AuthCodeCacheKey, out string? authCode))
            {
                if (_spotifyOptions.VerboseLogging)
                    _logger.LogDebug("AuthorizationCodeGetAsync: no authorization code found in cache");

                return default;
            }

            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationCodeGetAsync: authorization code found in cache");

            return await Task.FromResult(authCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthorizationCodeGetAsync failed");
            throw;
        }
    }
    public async Task<bool> AuthorizationCodeAddAsync(string? authorizationCode, string? state = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationCodeAddAsync called");

            if (state is not null)
            {
                if (!_memoryCache.TryGetValue(AuthStateCacheKey, out string? storedState) || storedState != state)
                {
                    _logger.LogWarning("AuthorizationCodeAddAsync: state mismatch, possible CSRF attempt");
                    return false;
                }
                _memoryCache.Remove(AuthStateCacheKey);
            }

            if (string.IsNullOrEmpty(authorizationCode)) return false;

            await AuthorizationCodeRemoveAsync(cancellationToken);
            await _memoryCache.GetOrCreateAsync(AuthCodeCacheKey, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
                return Task.FromResult(authorizationCode);
            });

            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationCodeAddAsync: authorization code cached successfully");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthorizationCodeAddAsync failed");
            return false;
        }
    }
    public async Task<bool> AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationCodeRemoveAsync called");

            _memoryCache.Remove(AuthCodeCacheKey);

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthorizationCodeRemoveAsync failed");
            return false;
        }
    }

    public async Task<AuthToken?> AuthorizationTokenGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationTokenGetAsync called");

            var authorizationCode = await AuthorizationCodeGetAsync(cancellationToken);
            if (authorizationCode is null) return default;

            if (_memoryCache.TryGetValue(AuthTokenCacheKey, out AuthToken? cached) && cached is not null)
            {
                if (_spotifyOptions.VerboseLogging)
                    _logger.LogDebug("AuthorizationTokenGetAsync: token found in cache");

                return cached;
            }

            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationTokenGetAsync: token not in cache, requesting new token");

            await _tokenLock.WaitAsync(cancellationToken);
            try
            {
                var ret = await _memoryCache.GetOrCreateAsync(AuthTokenCacheKey, async (cacheEntry) =>
                {
                    var authToken = default(AuthToken);
                    var authentication = $"{_spotifyOptions.ClientId}:{_spotifyOptions.ClientSecret}".EncodeBase64();

                    try
                    {
                        var data = string.IsNullOrEmpty(RefreshToken)
                            ? new FormUrlEncodedContent([
                                new ("code", authorizationCode),
                                new ("grant_type", "authorization_code"),
                                new ("redirect_uri", _spotifyOptions.RedirectUri)
                            ])
                            : new FormUrlEncodedContent([
                                new ("client_id", _spotifyOptions.ClientId),
                                new ("grant_type", "refresh_token"),
                                new ("refresh_token", RefreshToken)
                            ]);

                        var res = await _httpSpotifyProvider.ExecuteAsync("post", TokenUri, data, (h) => h.Authorization = new AuthenticationHeaderValue("Basic", authentication), cancellationToken);
                        authToken = _spotifyJsonProvider.Deserialize<AuthToken?>(res);

                        ArgumentNullException.ThrowIfNull(authToken, nameof(authToken));

                        RefreshToken = authToken.RefreshToken ?? RefreshToken;
                        cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authToken.ExpiresIn - 10);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unable to obtain token, resetting... {message}", ex.Message);
                    }
                    return authToken;
                });

                if (ret is null) await AuthorizationTokenReset();

                return ret;
            }
            finally
            {
                _tokenLock.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthorizationTokenGetAsync failed");
            throw;
        }
    }
    public async Task<bool> AuthorizationTokenReset()
    {
        try
        {
            if (_spotifyOptions.VerboseLogging)
                _logger.LogDebug("AuthorizationTokenReset called");

            _memoryCache.Remove(AuthCodeCacheKey);
            _memoryCache.Remove(AuthTokenCacheKey);

            RefreshToken = default;

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthorizationTokenReset failed");
            return false;
        }
    }

    public void Dispose()
    {
        _tokenLock.Dispose();
    }
}