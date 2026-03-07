using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class AlbumService(ILogger<AlbumService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), IAlbumService
{
    private readonly ILogger<AlbumService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string AlbumUri = "https://api.spotify.com/v1/albums/{0}";
    private const string AlbumTrackUri = "https://api.spotify.com/v1/albums/{0}/tracks";

    public async Task<SpotifyResult<Album>> AlbumGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = string.Format(AlbumUri, id);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting album '{album}'", id);
        }
        return GenerateResult<Album>(ret);
    }

    public async Task<SpotifyResult<SpotifyPageResult<Track>>> AlbumTrackGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            };
            var uri = BuildUri(string.Format(AlbumTrackUri, id), queryParams);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting album tracks for album '{album}'", id);
        }
        return GenerateResult<SpotifyPageResult<Track>>(ret);
    }

}
