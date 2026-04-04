using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
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
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-position"]);
            if (error is not null) return new SpotifyResult<Episode>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("EpisodeGetAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyEpisodeUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("EpisodeGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Episode>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EpisodeGetAsync failed for id: {Id}", id);
            return new SpotifyResult<Episode> { Error = ex.ToSpotifyError() };
        }
    }
}
