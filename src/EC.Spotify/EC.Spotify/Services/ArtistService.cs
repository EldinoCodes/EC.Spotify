using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
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
    public async Task<SpotifyResult<SpotifyPageResult<Album>>> ArtistAlbumGetAllAsync(string? id, int? limit = 5, int? offset = 0, string? includeGroups = default, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistAlbumGetAllAsync called with id: {Id}, limit: {Limit}, offset: {Offset}, includeGroups: {IncludeGroups}", id, limit, offset, includeGroups);

            var queryParams = new Dictionary<string, string?>()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            };
            if (!string.IsNullOrEmpty(includeGroups))
                queryParams.Add("include_groups", includeGroups);

            var uri = string.Format(SpotifyArtistAlbumsUri, id).ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ArtistAlbumGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Album>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ArtistAlbumGetAllAsync failed for id: {Id}", id);
            return new SpotifyResult<SpotifyPageResult<Album>> { Error = ex.ToSpotifyError() };
        }
    }
}
