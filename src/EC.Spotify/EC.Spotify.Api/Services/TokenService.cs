using EC.Spotify.Api.Models;
using EC.Spotify.Api.Providers;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace EC.Spotify.Api.Services;

public interface ITokenService
{
    bool HasAppToken { get; }
    bool HasUserToken { get; }

    Task<AuthToken?> AuthAppTokenGetAsync(CancellationToken cancellationToken = default);
    void AuthAppTokenReset();
    void AuthTokenReset();
    Task<AuthToken?> AuthUserTokenGetAsync(CancellationToken cancellationToken = default);
    void AuthUserTokenReset();
}

public class TokenService(ILogger<TokenService> logger, IConfiguration configuration, IHttpProvider httpProvider, IMemoryCache memoryCache, IAuthorizationService authorizationService) : ITokenService
{
    private readonly ILogger<TokenService> _logger = logger;
    private readonly IHttpProvider _httpProvider = httpProvider;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    private readonly string _clientId = configuration.GetValue<string?>("AppSettings:ClientId") ?? throw new ArgumentNullException("AppSettings:ClientId");
    private readonly string _clientSecret = configuration.GetValue<string?>("AppSettings:ClientSecret") ?? throw new ArgumentNullException("AppSettings:ClientSecret");
    private readonly string _tokenUri = configuration.GetValue<string?>("AppSettings:TokenUri") ?? throw new ArgumentNullException("AppSettings:TokenUri");
    private readonly string _redirectUri = configuration.GetValue<string?>("AppSettings:RedirectUri") ?? throw new ArgumentNullException("AppSettings:RedirectUri");


    private string? RefreshToken { get; set; }

    public bool HasUserToken => _memoryCache.Get<AuthToken>("SpotifyUserAuthToken") != null;
    public bool HasAppToken => _memoryCache.Get<AuthToken>("SpotifyAppAuthToken") != null;

    public async Task<AuthToken?> AuthUserTokenGetAsync(CancellationToken cancellationToken = default)
    {
        var authorizationCode = await _authorizationService.AuthorizationCodeGetAsync(cancellationToken);
        if (authorizationCode is null) return default;

        var ret = await _memoryCache.GetOrCreateAsync("SpotifyUserAuthToken", async (cacheEntry) =>
        {
            var authToken = default(AuthToken);
            var authentication = $"{_clientId}:{_clientSecret}".EncodeBase64();

            try
            {
                var data = AuthUserTokenFormContentGet(authorizationCode);

                var response = await _httpProvider.ExecuteAsync("post", _tokenUri, data, (h) => h.Authorization = new AuthenticationHeaderValue("Basic", authentication), true, cancellationToken);
                authToken = response.FromJson<AuthToken>();

                ArgumentNullException.ThrowIfNull(authToken, nameof(authToken));

                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authToken.ExpiresIn - 10);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to obtain token, resetting... {message}", ex.Message);
            }
            return authToken;
        });

        if (ret is null) AuthUserTokenReset();

        return ret;
    }
    private FormUrlEncodedContent AuthUserTokenFormContentGet(string? authorizationCode)
    {
        var data = string.IsNullOrEmpty(RefreshToken)
            ? new FormUrlEncodedContent([
                new ("code", authorizationCode),
                new ("grant_type", "authorization_code"),
                new ("redirect_uri", _redirectUri)
            ])
            : new FormUrlEncodedContent([
                new ("client_id", _clientId),
                new ("grant_type", "refresh_token"),
                new ("refresh_token", RefreshToken)
            ]);

        return data;
    }
    public void AuthUserTokenReset() => _memoryCache.Remove("SpotifyUserAuthToken");

    public async Task<AuthToken?> AuthAppTokenGetAsync(CancellationToken cancellationToken = default)
    {
        var authAppToken = await _memoryCache.GetOrCreateAsync("SpotifyAppAuthToken", async (cacheEntry) =>
        {
            var data = new FormUrlEncodedContent([
                new ("client_id", _clientId),
                new ("client_secret", _clientSecret),
                new ("grant_type", "client_credentials")
            ]);

            var response = await _httpProvider.ExecuteAsync("post", _tokenUri, data, null, true, cancellationToken);

            var authToken = response.FromJson<AuthToken>();
            ArgumentNullException.ThrowIfNull(authToken, nameof(authToken));

            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authToken.ExpiresIn - 10);

            return authToken;
        });

        if (authAppToken is null) AuthAppTokenReset();

        return authAppToken;
    }
    public void AuthAppTokenReset() => _memoryCache.Remove("SpotifyAppAuthToken");

    public void AuthTokenReset()
    {
        AuthUserTokenReset();
        AuthAppTokenReset();
    }
}