using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace EC.Spotify.Api.Services;

public interface IAuthorizationService
{
    Task<bool> AuthorizationCodeAddAsync(string? authorizationCode, CancellationToken cancellationToken = default);
    Task<string?> AuthorizationCodeGetAsync(CancellationToken cancellationToken = default);
    Task<bool> AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default);
    string AuthorizationCodeUrl();
}

public class AuthorizationService(ILogger<AuthorizationService> logger, IConfiguration configuration, IMemoryCache memoryCache) : IAuthorizationService
{
    private readonly ILogger<AuthorizationService> _logger = logger;
    private readonly IMemoryCache _memoryCache = memoryCache;

    private readonly string _clientId = configuration.GetValue<string?>("AppSettings:ClientId") ?? throw new ArgumentNullException("AppSettings:ClientId");
    private readonly string _authUri = configuration.GetValue<string?>("AppSettings:AuthUri") ?? throw new ArgumentNullException("AppSettings:AuthUri");
    private readonly string _redirectUri = configuration.GetValue<string?>("AppSettings:RedirectUri") ?? throw new ArgumentNullException("AppSettings:RedirectUri");
    private readonly string[] _scopes = configuration.GetSection("AppSettings:Scopes")?.Get<string[]>() ?? throw new ArgumentNullException("AppSettings:Scopes");

    public string AuthorizationCodeUrl()
    {
        var queryBuilder = new QueryBuilder
        {
            { "client_id", _clientId },
            { "response_type", "code"},
            { "scope", string.Join(" ", _scopes) },
            { "redirect_uri", _redirectUri },
            { "state", Guid.NewGuid().ToString() }
        };
        return new UriBuilder(_authUri)
        {
            Query = queryBuilder
                .ToQueryString()
                .ToUriComponent()
        }.ToString();
    }

    public async Task<string?> AuthorizationCodeGetAsync(CancellationToken cancellationToken = default)
    {
        if (!_memoryCache.TryGetValue("AuthorizationCode", out string? code)) return default;

        return await Task.FromResult(code);
    }
    public async Task<bool> AuthorizationCodeAddAsync(string? authorizationCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(authorizationCode)) return false;

        await _memoryCache.GetOrCreateAsync("AuthorizationCode", entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            return Task.FromResult(authorizationCode);
        });

        return true;
    }
    public async Task<bool> AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove("AuthorizationCode");

        return await Task.FromResult(true);
    }
}
