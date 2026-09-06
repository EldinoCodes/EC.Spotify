using EC.Spotify.Models;
using EC.Spotify.Models.Shows;

namespace EC.Spotify.Abstractions.Services;

public interface IEpisodeService 
{
    /// <summary>
    /// Retrieves the details of a Spotify episode by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The Spotify episode ID to retrieve. Can be null to indicate no episode; if null or invalid, the result will
    /// indicate an error.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Episode}"/> with the episode details if found; otherwise, an error result.</returns>
    Task<SpotifyResult<Episode>> EpisodeGetAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw episode JSON from Spotify asynchronously by episode identifier.
    /// </summary>
    /// <param name="id">The Spotify episode ID to retrieve. Can be null or invalid.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> EpisodeGetRawAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of episodes saved in the current user's library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope.</remarks>
    /// <param name="limit">The maximum number of episodes to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Episode objects for the user's saved episodes.</returns>
    [Obsolete("This method is deprecated. Use IUserService.MyEpisodeGetAllAsync instead.")]
    Task<SpotifyResult<SpotifyPageResult<Episode>>> MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated episode JSON for episodes saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of episodes to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    [Obsolete("This method is deprecated. Use IUserService.MyEpisodeGetAllRawAsync instead.")]
    Task<string?> MyEpisodeGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
