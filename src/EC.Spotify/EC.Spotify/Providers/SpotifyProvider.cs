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
            ArgumentNullException.ThrowIfNull(method, nameof(method));
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));

            var authToken = await _authorizationService.AuthorizationTokenGetAsync(cancellationToken) ?? throw new InvalidOperationException("Authorization token is null.");

            void headers(HttpRequestHeaders h)
            {
                if (string.IsNullOrEmpty(authToken?.TokenType) || string.IsNullOrEmpty(authToken?.AccessToken)) return;
                h.Authorization = new AuthenticationHeaderValue(authToken.TokenType, authToken.AccessToken);
            }

            var responseContent = await _spotifyHttpProvider.ExecuteAsync(method, uri, httpContent, headers, cancellationToken);
            var error = _spotifyJsonProvider.Deserialize<SpotifyError>(responseContent, "error");

            var data = default(T);
            if (error is null)
            {
                foreach (var jsonPath in jsonPaths ?? [])
                {
                    if (data is not null) break;
                    data = _spotifyJsonProvider.Deserialize<T>(responseContent, jsonPath);
                }
                data ??= _spotifyJsonProvider.Deserialize<T>(responseContent);
            }

            return new SpotifyResult<T>
            {
                Data = data,
                Error = error
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
}
