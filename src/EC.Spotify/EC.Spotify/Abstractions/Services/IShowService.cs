using EC.Spotify.Models;
using EC.Spotify.Models.Shows;

namespace EC.Spotify.Abstractions.Services;

public interface IShowService
{
    /// <summary>
    /// Retrieves a Spotify show by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The Spotify show ID to retrieve. Can be null or empty to indicate an invalid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SpotifyResult{Show}"/>
    /// with the show details if found; otherwise, the result indicates failure.</returns>
    Task<SpotifyResult<Show>> ShowGetAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw show JSON from Spotify asynchronously by show identifier.
    /// </summary>
    /// <param name="id">The Spotify show ID to retrieve. Can be null or empty to indicate an invalid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> ShowGetRawAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of episodes for the specified show asynchronously.
    /// </summary>
    /// <param name="id">The Spotify identifier of the show for which to retrieve episodes. Can be null to indicate no show.</param>
    /// <param name="limit">The maximum number of episodes to return. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Episode objects for the specified show.</returns>
    Task<SpotifyResult<SpotifyPageResult<Episode>>> ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated episode JSON for the specified show from Spotify asynchronously.
    /// </summary>
    /// <param name="id">The Spotify identifier of the show for which to retrieve episodes. Can be null to indicate no show.</param>
    /// <param name="limit">The maximum number of episodes to return. The default is 20.</param>
    /// <param name="offset">The index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> ShowEpisodeGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
