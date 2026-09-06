using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class EpisodeService(ILogger<EpisodeService> logger, IOptions<SpotifyOptions> options, IUserService userService, ISpotifyProvider spotifyProvider) : IEpisodeService
{
    private readonly ILogger<EpisodeService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly IUserService _userService = userService;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyEpisodeUri = "https://api.spotify.com/v1/episodes/{0}";

    public async Task<SpotifyResult<Episode>> EpisodeGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-position"]);
            if (error is not null) return new SpotifyResult<Episode> { Error = error };

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
    public async Task<string?> EpisodeGetRawAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-position"]);
            if (error is not null) throw new InvalidOperationException(error.Message);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("EpisodeGetRawAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyEpisodeUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("EpisodeGetRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EpisodeGetRawAsync failed for id: {Id}", id);
            throw;
        }
    }

    public async Task<SpotifyResult<SpotifyPageResult<Episode>>> MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
        => await _userService.MyEpisodeGetAllAsync(limit, offset, cancellationToken);
    public async Task<string?> MyEpisodeGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
        => await _userService.MyEpisodeGetAllRawAsync(limit, offset, cancellationToken);
}
