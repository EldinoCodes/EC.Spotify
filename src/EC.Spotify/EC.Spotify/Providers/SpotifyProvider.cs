using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace EC.Spotify.Providers;

internal class SpotifyProvider(ILogger<SpotifyProvider> logger, ISpotifyHttpProvider spotifyHttpProvider, ISpotifyJsonProvider spotifyJsonProvider, IAuthorizationService authorizationService) : ISpotifyProvider
{
    private readonly ILogger<SpotifyProvider> _logger = logger;
    private readonly ISpotifyHttpProvider _spotifyHttpProvider = spotifyHttpProvider;
    private readonly ISpotifyJsonProvider _spotifyJsonProvider = spotifyJsonProvider;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    public async Task<SpotifyResult<T>> ExecuteSpotifyResultAsync<T>(string? method, string? uri, HttpContent? httpContent = default, List<string?>? jsonPaths = default, CancellationToken cancellationToken = default)
    {
        try
        {
            var responseContent = await ExecuteSpotifyRequestAsync(method, uri, httpContent, cancellationToken);            
            var errorJson = _spotifyJsonProvider.ProcessSpotifyJson(responseContent, "error");

            if (!string.IsNullOrEmpty(errorJson))
                return new SpotifyResult<T>
                {
                    Data = default,
                    Error = _spotifyJsonProvider.Deserialize<SpotifyError>(errorJson)
                };

            var json = default(string?);
            if (jsonPaths?.Count > 0)
                foreach (var jsonPath in jsonPaths ?? [])
                {
                    json = _spotifyJsonProvider.ProcessSpotifyJson(responseContent, jsonPath);
                    if (!string.IsNullOrEmpty(json)) break;
                }

            return new SpotifyResult<T>
            {
                Data = _spotifyJsonProvider.Deserialize<T>(json ?? _spotifyJsonProvider.ProcessSpotifyJson(responseContent)),
                Error = default
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing Spotify API request.");

            return new SpotifyResult<T>
            {
                Error = new SpotifyError
                {
                    Message = "An error occurred while executing the Spotify API request.",
                    Reason = ex.Message
                }
            };
        }
    }

    public async Task<string?> ExecuteSpotifyRequestAsync(string? method, string? uri, HttpContent? httpContent = default, CancellationToken cancellationToken = default)
    {
        string? response = default;

        ArgumentNullException.ThrowIfNull(method, nameof(method));
        ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        var authToken = await _authorizationService.AuthorizationTokenGetAsync(cancellationToken) ?? throw new InvalidOperationException("Authorization token is null.");

        void headers(HttpRequestHeaders h)
        {
            if (string.IsNullOrEmpty(authToken?.TokenType) || string.IsNullOrEmpty(authToken?.AccessToken)) return;
            h.Authorization = new AuthenticationHeaderValue(authToken.TokenType, authToken.AccessToken);
        }

        response = await _spotifyHttpProvider.ExecuteAsync(method, uri, httpContent, headers, cancellationToken);

        return response;
    }
}
