using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
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
        var uri = string.Format(SpotifyTrackUri, id);

        return await _spotifyProvider.ExecuteSpotifyResultAsync<Track>("get", uri, cancellationToken: cancellationToken);
    }
}
