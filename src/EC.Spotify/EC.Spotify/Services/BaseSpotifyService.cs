using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using System.Net.Http.Headers;

namespace EC.Spotify.Services;

internal abstract class BaseSpotifyService(IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer)
{
    private readonly IAuthorizationService _authorizationService = authorizationService;
    private readonly ISpotifyJsonSerializer _spotifyJsonSerializer = spotifyJsonSerializer;

    protected virtual string? BuildUri(string? uri, Dictionary<string, string?>? queryParams) 
        => new UriBuilder(uri ?? string.Empty).AddQuery(queryParams).ToString();

    protected virtual async Task<Action<HttpRequestHeaders>> GetAuthorizationHeaderAsync(CancellationToken cancellationToken = default)
    {
        var authToken = await _authorizationService.AuthorizationTokenGetAsync(cancellationToken);

        void headers(HttpRequestHeaders h)
        {
            if (string.IsNullOrEmpty(authToken?.TokenType) || string.IsNullOrEmpty(authToken?.AccessToken)) return;

            h.Authorization = new AuthenticationHeaderValue(authToken.TokenType, authToken.AccessToken);
        }

        return headers;
    }

    protected SpotifyResult<T> GenerateResult<T>(string? raw, string? jsonPath = default) where T : new()
    {
        var error = _spotifyJsonSerializer.Deserialize<SpotifyError?>(raw, "error");
        if (typeof(T) == typeof(bool) && error is null) raw = "true";
        return new SpotifyResult<T>
        {
            Raw = raw,            
            Value = error is null 
                ? _spotifyJsonSerializer.Deserialize<T?>(raw, jsonPath)
                : default,
            Error = error
        };
    }
}
