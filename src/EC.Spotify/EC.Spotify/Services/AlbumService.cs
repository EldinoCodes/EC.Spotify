using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class AlbumService(ILogger<AlbumService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IAlbumService
{
    private readonly ILogger<AlbumService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string AlbumUri = "https://api.spotify.com/v1/albums/{0}";
    private const string AlbumTrackUri = "https://api.spotify.com/v1/albums/{0}/tracks";

    public async Task<SpotifyResult<Album>> AlbumGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(AlbumUri, id);

        return await _spotifyProvider.ExecuteSpotifyResultAsync<Album>("get", uri, cancellationToken: cancellationToken);
    }

    public async Task<SpotifyResult<SpotifyPageResult>> AlbumTrackGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(AlbumTrackUri, id).ToUri(new()
        {
            { "limit", $"{limit}"},
            { "offset", $"{offset}"}
        });
        return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult>("get", uri, cancellationToken: cancellationToken);
    }
}
