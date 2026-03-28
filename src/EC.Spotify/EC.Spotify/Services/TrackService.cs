using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class TrackService(ILogger<TrackService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : ITrackService
{
    private readonly ILogger<TrackService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyTrackUri = "https://api.spotify.com/v1/tracks/{0}";

    public async Task<SpotifyResult<Track>> TrackGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("TrackGetAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyTrackUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("TrackGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Track>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TrackGetAsync failed for id: {Id}", id);
            return new SpotifyResult<Track> { Error = ex.ToSpotifyError() };
        }
    }
}
