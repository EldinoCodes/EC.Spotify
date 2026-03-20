using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class EpisodeService(ILogger<EpisodeService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IEpisodeService
{
    private readonly ILogger<EpisodeService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyEpisodeUri = "https://api.spotify.com/v1/episodes/{0}";

    public async Task<SpotifyResult<Episode>> EpisodeGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(SpotifyEpisodeUri, id);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<Episode>("get", uri, cancellationToken: cancellationToken);
    }
}
