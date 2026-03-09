using EC.Spotify.Abstractions.Models;
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
    /// Retrieves a paginated list of albums for the specified artist from Spotify asynchronously.
    /// </summary>
    /// <remarks>This method supports pagination through the limit and offset parameters. Use includeGroups to
    /// filter the types of albums returned. The operation is performed asynchronously and can be cancelled using the
    /// cancellation token.</remarks>
    /// <param name="id">The Spotify ID of the artist whose albums are to be retrieved. Can be null to indicate no artist.</param>
    /// <param name="limit">The maximum number of albums to return in the result. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first album to return. Used for pagination. The default is 0.</param>
    /// <param name="includeGroups">A comma-separated list of album types to include, such as "album", "single", "appears_on", or "compilation". If
    /// null or empty, all album types are included.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Album objects for the specified artist. The result may be empty if the artist has no albums
    /// or the ID is invalid.</returns>
    Task<SpotifyResult<SpotifyPageResult>> ArtistAlbumGetAllAsync(string? id, int? limit = 20, int? offset = 0, string? includeGroups = default, CancellationToken cancellationToken = default);
}
