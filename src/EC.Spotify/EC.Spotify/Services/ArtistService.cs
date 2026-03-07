using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class ArtistService(ILogger<ArtistService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), IArtistService
{
    private readonly ILogger<ArtistService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifyArtistUri = "https://api.spotify.com/v1/artists/{0}";
    private const string SpotifyArtistAlbumsUri = "https://api.spotify.com/v1/artists/{0}/albums";

    public async Task<SpotifyResult<Artist>> ArtistGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = string.Format(SpotifyArtistUri, id);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting artist '{artist}'", id);
        }
        return GenerateResult<Artist>(ret);
    }

    public async Task<SpotifyResult<SpotifyPageResult<Album>>> ArtistAlbumGetAllAsync(string? id, int? limit = 20, int? offset = 0, string? includeGroups = default, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            };
            if (!string.IsNullOrEmpty(includeGroups)) queryParams.Add("include_groups", includeGroups);

            var uri = BuildUri(string.Format(SpotifyArtistAlbumsUri, id), queryParams);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting albums for artist '{artist}'", id);
        }
        return GenerateResult<SpotifyPageResult<Album>>(ret);
    }
}
