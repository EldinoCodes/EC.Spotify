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
    /// Retrieves raw album JSON from Spotify asynchronously by album identifier.
    /// </summary>
    /// <param name="id">The Spotify album identifier. If null or empty, the request will not be sent.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> AlbumGetRawAsync(string? id, CancellationToken cancellationToken = default);


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

    /// <summary>
    /// Retrieves raw paginated track JSON for the specified album from Spotify asynchronously.
    /// </summary>
    /// <param name="id">The Spotify album identifier. If null, no tracks will be returned.</param>
    /// <param name="limit">The maximum number of tracks to return. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first track to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> AlbumTrackGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of albums saved in the current user's library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope.</remarks>
    /// <param name="limit">The maximum number of albums to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first album to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Album objects for the user's saved albums.</returns>
    [Obsolete("This method is deprecated. Use IUserService.MyAlbumGetAllAsync instead.")]
    Task<SpotifyResult<SpotifyPageResult<Album>>> MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated album JSON for albums saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of albums to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first album to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    [Obsolete("This method is deprecated. Use IUserService.MyAlbumGetAllRawAsync instead.")]
    Task<string?> MyAlbumGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
