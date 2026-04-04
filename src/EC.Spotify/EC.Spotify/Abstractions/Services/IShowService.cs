using EC.Spotify.Models;
using EC.Spotify.Models.Shows;

namespace EC.Spotify.Abstractions.Services;

public interface IShowService
{
    /// <summary>
    /// Retrieves a Spotify show by its unique identifier asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-read-playback-position</c> scope.</remarks>
    /// <param name="id">The Spotify show ID to retrieve. Can be null or empty to indicate an invalid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SpotifyResult{Show}"/>
    /// with the show details if found; otherwise, the result indicates failure.</returns>
    Task<SpotifyResult<Show>> ShowGetAsync(string? id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a paginated list of episodes for the specified show asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-read-playback-position</c> scope. If the show contains more episodes than the specified limit, use the offset parameter to
    /// retrieve additional pages. The method does not throw if the show has no episodes; the result will contain an
    /// empty page.</remarks>
    /// <param name="id">The Spotify identifier of the show for which to retrieve episodes. Can be null to indicate no show.</param>
    /// <param name="limit">The maximum number of episodes to return. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Episode objects for the specified show.</returns>
    Task<SpotifyResult<SpotifyPageResult<Episode>>> ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
