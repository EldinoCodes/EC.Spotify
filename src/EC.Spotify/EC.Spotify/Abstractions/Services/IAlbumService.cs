using EC.Spotify.Models;
using EC.Spotify.Models.Albums;

namespace EC.Spotify.Abstractions.Services;

public interface IAlbumService
{
    /// <summary>
    /// Retrieves album details from Spotify asynchronously by album identifier.
    /// </summary>
    /// <remarks>If the specified album identifier does not correspond to an existing album, the result will
    /// indicate failure and contain no album data. This method does not throw exceptions for missing albums; check the
    /// result for success or failure.</remarks>
    /// <param name="id">The Spotify album identifier. If null or empty, the request will not be sent and the result will indicate
    /// failure.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="SpotifyResult{Album}"/>
    /// with the album details if found; otherwise, the result indicates failure.</returns>
    Task<SpotifyResult<Album>> AlbumGetAsync(string? id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a paginated list of tracks for the specified album from Spotify asynchronously.
    /// </summary>
    /// <param name="id">The Spotify album identifier. If null, no tracks will be returned.</param>
    /// <param name="limit">The maximum number of tracks to return. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first track to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Track objects for the specified album. The result may be empty if the album has no tracks
    /// or the album ID is invalid.</returns>
    Task<SpotifyResult<SpotifyPageResult<Track>>> AlbumTrackGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
