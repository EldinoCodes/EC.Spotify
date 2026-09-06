using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class ShowService(ILogger<ShowService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IShowService
{
    private readonly ILogger<ShowService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyShowUri = "https://api.spotify.com/v1/shows/{0}";
    private const string SpotifyShowEpisodesUri = "https://api.spotify.com/v1/shows/{0}/episodes";

    public async Task<SpotifyResult<Show>> ShowGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-position"]);
            if (error is not null) return new SpotifyResult<Show> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowGetAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyShowUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Show>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ShowGetAsync failed for id: {Id}", id);
            return new SpotifyResult<Show> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<string?> ShowGetRawAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-position"]);
            if (error is not null) throw new InvalidOperationException(error.Message);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowGetRawAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyShowUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowGetRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ShowGetRawAsync failed for id: {Id}", id);
            throw;
        }
    }

    public async Task<SpotifyResult<SpotifyPageResult<Episode>>> ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-position"]);
            if (error is not null) return new SpotifyResult<SpotifyPageResult<Episode>> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowEpisodeGetAllAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            var uri = string.Format(SpotifyShowEpisodesUri, id).ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowEpisodeGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Episode>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ShowEpisodeGetAllAsync failed for id: {Id}", id);
            return new SpotifyResult<SpotifyPageResult<Episode>> { Error = ex.ToSpotifyError() };
        }
    }    
    public async Task<string?> ShowEpisodeGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-position"]);
            if (error is not null) throw new InvalidOperationException(error.Message);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowEpisodeGetAllRawAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowEpisodeGetAllRawAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            var uri = string.Format(SpotifyShowEpisodesUri, id).ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ShowEpisodeGetAllRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ShowEpisodeGetAllRawAsync failed for id: {Id}", id);
            throw;
        }
    }
}
