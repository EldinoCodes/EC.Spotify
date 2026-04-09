using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class ArtistService(ILogger<ArtistService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IArtistService
{
    private readonly ILogger<ArtistService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyArtistUri = "https://api.spotify.com/v1/artists/{0}";
    private const string SpotifyArtistAlbumsUri = "https://api.spotify.com/v1/artists/{0}/albums";

    public async Task<SpotifyResult<Artist>> ArtistGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistGetAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyArtistUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Artist>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ArtistGetAsync failed for id: {Id}", id);
            return new SpotifyResult<Artist> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<SpotifyPageResult<Album>>> ArtistAlbumGetAllAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var albumType = typeof(AlbumType);
            var includeTypes = string.Join(",", Enum.GetValues(albumType)
                .Cast<AlbumType>()
                .Where(t => albumTypes?.HasFlag(t) ?? false)
                .Select(t => Enum.GetName(albumType, t)?.ToLower())
            );

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistAlbumGetAllAsync called with id: {Id}, limit: {Limit}, offset: {Offset}, includeGroups: {IncludeGroups}", id, limit, offset, includeTypes);

            var queryParams = new Dictionary<string, string?>()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            };
            if (!string.IsNullOrEmpty(includeTypes))
                queryParams.Add("include_groups", includeTypes);

            var uri = string.Format(SpotifyArtistAlbumsUri, id).ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistAlbumGetAllAsync requesting URI: {Uri}", uri);

            var ret = await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Album>>("get", uri, cancellationToken: cancellationToken);

            ret.Data?.Next = ret.Data?.Next?.Replace("include_groups", "albumTypes");
            ret.Data?.Previous = ret.Data?.Previous?.Replace("include_groups", "albumTypes");

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ArtistAlbumGetAllAsync failed for id: {Id}", id);
            return new SpotifyResult<SpotifyPageResult<Album>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<string?> ArtistGetRawAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistGetRawAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyArtistUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistGetRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ArtistGetRawAsync failed for id: {Id}", id);
            throw;
        }
    }
    public async Task<string?> ArtistAlbumGetAllRawAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var albumType = typeof(AlbumType);
            var includeTypes = string.Join(",", Enum.GetValues(albumType)
                .Cast<AlbumType>()
                .Where(t => albumTypes?.HasFlag(t) ?? false)
                .Select(t => Enum.GetName(albumType, t)?.ToLower())
            );

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistAlbumGetAllRawAsync called with id: {Id}, limit: {Limit}, offset: {Offset}, includeGroups: {IncludeGroups}", id, limit, offset, includeTypes);

            var queryParams = new Dictionary<string, string?>()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            };
            if (!string.IsNullOrEmpty(includeTypes))
                queryParams.Add("include_groups", includeTypes);

            var uri = string.Format(SpotifyArtistAlbumsUri, id).ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistAlbumGetAllRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ArtistAlbumGetAllRawAsync failed for id: {Id}", id);
            throw;
        }
    }
}
