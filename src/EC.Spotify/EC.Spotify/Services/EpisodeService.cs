using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class EpisodeService(ILogger<EpisodeService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), IEpisodeService
{
    private readonly ILogger<EpisodeService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifyEpisodeUri = "https://api.spotify.com/v1/episodes/{0}";

    public async Task<SpotifyResult<Episode>> EpisodeGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = string.Format(SpotifyEpisodeUri, id);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting episode '{episode}'", id);
        }
        return GenerateResult<Episode>(ret);
    }
}
