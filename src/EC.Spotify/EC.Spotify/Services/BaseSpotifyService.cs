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

    protected virtual SpotifyResult<T> GenerateResult<T>(string? raw, List<string?>? jsonPaths = default) where T : new()
    {
        var error = raw?.Contains("error", StringComparison.InvariantCultureIgnoreCase) ?? false
            ? _spotifyJsonSerializer.Deserialize<SpotifyError?>(raw, "error")
            : null;
        if (typeof(T) == typeof(bool) && error is null) raw = "true";

        T? data = default;
        foreach (var jsonPath in jsonPaths ?? [])
        {
            data = _spotifyJsonSerializer.Deserialize<T?>(raw, jsonPath);
            if (data is not null) break;
        }
        data ??= _spotifyJsonSerializer.Deserialize<T?>(raw);

        return new SpotifyResult<T>
        {
            Data = error is null ? data : default,
            Error = error
        };
    }
}