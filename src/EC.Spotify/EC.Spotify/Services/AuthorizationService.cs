using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace EC.Spotify.Services;

internal sealed class AuthorizationService(ILogger<AuthorizationService> logger, IOptions<SpotifyOptions> spotifyOptions, ISpotifyHttpProvider httpProvider, ISpotifyJsonSerializer spotifyJsonSerializer, IMemoryCache memoryCache) : IAuthorizationService
{
    private readonly ILogger<AuthorizationService> _logger = logger;
    private readonly SpotifyOptions _spotifyOptions = spotifyOptions.Value;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;
    private readonly ISpotifyJsonSerializer _spotifyJsonSerializer = spotifyJsonSerializer;
    private readonly IMemoryCache _memoryCache = memoryCache;

    private const string AuthUri = "https://accounts.spotify.com/authorize";
    private const string TokenUri = "https://accounts.spotify.com/api/token";

    private string? RefreshToken { get; set; }


    public async Task<string?> Validate(CancellationToken cancellationToken = default)
    {
        var uri = AuthorizationCodeUrl();

        var authCode = await AuthorizationCodeGetAsync(cancellationToken);
        if (authCode is null) return uri;

        var authToken = await AuthorizationTokenGetAsync(cancellationToken);
        if (authToken is null) return uri;

        return default;
    }

    public string AuthorizationCodeUrl()
    {
        var uriBuilder = new UriBuilder(AuthUri)
            .AddQuery(new Dictionary<string, string?> {
                { "client_id", _spotifyOptions.ClientId },
                { "response_type", "code"},
                { "scope", string.Join(" ", _spotifyOptions.Scopes) },
                { "redirect_uri", _spotifyOptions.RedirectUri },
                { "state", Guid.NewGuid().ToString() }
            });
        return uriBuilder.ToString();
    }

    public async Task<string?> AuthorizationCodeGetAsync(CancellationToken cancellationToken = default)
    {
        if (!_memoryCache.TryGetValue($"{GetType().Namespace}.SpotifyAuthCode", out string? authCode)) return default;

        return await Task.FromResult(authCode);
    }
    public async Task<bool> AuthorizationCodeAddAsync(string? authorizationCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(authorizationCode)) return false;

        await AuthorizationCodeRemoveAsync(cancellationToken);
        await _memoryCache.GetOrCreateAsync($"{GetType().Namespace}.SpotifyAuthCode", entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            return Task.FromResult(authorizationCode);
        });

        return true;
    }
    public async Task<bool> AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove($"{GetType().Namespace}.SpotifyAuthCode");

        return await Task.FromResult(true);
    }

    public async Task<AuthToken?> AuthorizationTokenGetAsync(CancellationToken cancellationToken = default)
    {
        var authorizationCode = await AuthorizationCodeGetAsync(cancellationToken);
        if (authorizationCode is null) return default;

        var ret = await _memoryCache.GetOrCreateAsync($"{GetType().Namespace}.SpotifyAuthToken", async (cacheEntry) =>
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

                var res = await _httpProvider.ExecuteAsync("post", TokenUri, data, (h) => h.Authorization = new AuthenticationHeaderValue("Basic", authentication), cancellationToken);

                var error = res?.Contains("error", StringComparison.InvariantCultureIgnoreCase) ?? false
                    ? _spotifyJsonSerializer.Deserialize<SpotifyError?>(res, "error")
                    : null;
                if (error is not null) throw new Exception(error.Message);

                var token = _spotifyJsonSerializer.Deserialize<AuthToken?>(res);
                ArgumentNullException.ThrowIfNull(token, nameof(token));

                authToken = token;

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
    public async Task<bool> AuthorizationTokenReset()
    {
        _memoryCache.Remove($"{GetType().Namespace}.SpotifyAuthCode");
        _memoryCache.Remove($"{GetType().Namespace}.SpotifyAuthToken");

        RefreshToken = default;

        return await Task.FromResult(true);
    }
}