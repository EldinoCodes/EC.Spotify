using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;

namespace EC.Spotify.Abstractions.Services;

public interface IArtistService
{
    /// <summary>
    /// Retrieves detailed information about a Spotify artist asynchronously by their unique identifier.
    /// </summary>
    /// <remarks>If the specified artist ID is invalid or does not correspond to an existing artist, the
    /// result will indicate failure and contain no artist data. This method does not throw exceptions for missing
    /// artists; check the result for success or failure.</remarks>
    /// <param name="id">The Spotify artist ID to retrieve. Can be null or empty to indicate an invalid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Artist}"/> with the artist's details if found; otherwise, the result indicates failure.</returns>
    Task<SpotifyResult<Artist>> ArtistGetAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paged list of albums for the specified artist from the Spotify catalog asynchronously.
    /// </summary>
    /// <param name="id">The Spotify ID of the artist whose albums are to be retrieved. Can be null to indicate no artist.</param>
    /// <param name="albumTypes">A filter specifying which types of albums to include in the results. If not specified, all album types are
    /// included.</param>
    /// <param name="limit">The maximum number of albums to return. Must be a non-negative integer. The default is 5.</param>
    /// <param name="offset">The index of the first album to return. Used for paging. Must be a non-negative integer. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult wrapping a
    /// SpotifyPageResult of Album objects for the specified artist. The result may be empty if the artist has no albums
    /// or if the artist ID is invalid.</returns>
    Task<SpotifyResult<SpotifyPageResult<Album>>> ArtistAlbumGetAllAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw artist JSON from Spotify asynchronously by artist identifier.
    /// </summary>
    /// <param name="id">The Spotify artist ID to retrieve. Can be null or empty to indicate an invalid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> ArtistGetRawAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated album JSON for the specified artist from Spotify asynchronously.
    /// </summary>
    /// <param name="id">The Spotify ID of the artist whose albums are to be retrieved. Can be null to indicate no artist.</param>
    /// <param name="albumTypes">A filter specifying which types of albums to include in the results.</param>
    /// <param name="limit">The maximum number of albums to return. The default is 5.</param>
    /// <param name="offset">The index of the first album to return. Used for paging. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> ArtistAlbumGetAllRawAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default);
}
