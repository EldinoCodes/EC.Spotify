using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class TrackService(ILogger<TrackService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), ITrackService
{
    private readonly ILogger<TrackService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifyTrackUri = "https://api.spotify.com/v1/tracks/{0}";

    public async Task<SpotifyResult<Track>> TrackGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = string.Format(SpotifyTrackUri, id);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting track '{track}'", id);
        }
        return GenerateResult<Track>(ret);
    }
}
