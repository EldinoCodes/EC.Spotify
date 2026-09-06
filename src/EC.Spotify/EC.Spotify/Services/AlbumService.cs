using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class AlbumService(ILogger<AlbumService> logger, IOptions<SpotifyOptions> options, IUserService userService, ISpotifyProvider spotifyProvider) : IAlbumService
{
    private readonly ILogger<AlbumService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly IUserService _userService = userService;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string AlbumUri = "https://api.spotify.com/v1/albums/{0}";
    private const string AlbumTrackUri = "https://api.spotify.com/v1/albums/{0}/tracks";

    public async Task<SpotifyResult<Album>> AlbumGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumGetAsync called with id: {Id}", id);

            var uri = string.Format(AlbumUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Album>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AlbumGetAsync failed for id: {Id}", id);
            return new SpotifyResult<Album> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<string?> AlbumGetRawAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumGetRawAsync called with id: {Id}", id);

            var uri = string.Format(AlbumUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AlbumGetAsync failed for id: {Id}", id);

            throw;
        }
    }

    public async Task<SpotifyResult<SpotifyPageResult<Track>>> AlbumTrackGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumTrackGetAllAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            var uri = string.Format(AlbumTrackUri, id).ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumTrackGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Track>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AlbumTrackGetAllAsync failed for id: {Id}", id);
            return new SpotifyResult<SpotifyPageResult<Track>> { Error = ex.ToSpotifyError() };
        }
    }    
    public async Task<string?> AlbumTrackGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumTrackGetAllRawAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            var uri = string.Format(AlbumTrackUri, id).ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AlbumTrackGetAllRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AlbumTrackGetAllRawAsync failed for id: {Id}", id);

            throw;
        }
    }

    public async Task<SpotifyResult<SpotifyPageResult<Album>>> MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default) 
        => await _userService.MyAlbumGetAllAsync(limit, offset, cancellationToken);
    public async Task<string?> MyAlbumGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
        => await _userService.MyAlbumGetAllRawAsync(limit, offset, cancellationToken);
}
