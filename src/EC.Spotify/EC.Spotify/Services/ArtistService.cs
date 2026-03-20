using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class ArtistService(ILogger<ArtistService> logger, ISpotifyProvider spotifyProvider) : IArtistService
{
    private readonly ILogger<ArtistService> _logger = logger;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyArtistUri = "https://api.spotify.com/v1/artists/{0}";
    private const string SpotifyArtistAlbumsUri = "https://api.spotify.com/v1/artists/{0}/albums";

    public async Task<SpotifyResult<Artist>> ArtistGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(SpotifyArtistUri, id);

        return await _spotifyProvider.ExecuteSpotifyResultAsync<Artist>("get", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<SpotifyPageResult>> ArtistAlbumGetAllAsync(string? id, int? limit = 20, int? offset = 0, string? includeGroups = default, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>()
        {
            { "limit", $"{limit}"},
            { "offset", $"{offset }"}
        };
        if (!string.IsNullOrEmpty(includeGroups))
            queryParams.Add("include_groups", includeGroups);

        var uri = string.Format(SpotifyArtistAlbumsUri, id).ToUri(queryParams);

        return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult>("get", uri, cancellationToken: cancellationToken);
    }
}
