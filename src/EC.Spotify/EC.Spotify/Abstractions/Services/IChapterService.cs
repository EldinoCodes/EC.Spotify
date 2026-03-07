using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;

namespace EC.Spotify.Abstractions.Services;

public interface IChapterService
{
    /// <summary>
    /// Retrieves the details of a Spotify audiobook chapter by its unique identifier asynchronously.
    /// </summary>
    /// <remarks>If the specified chapter ID does not exist or is invalid, the result will indicate an error
    /// or not found. This method does not block the calling thread.</remarks>
    /// <param name="id">The Spotify ID of the chapter to retrieve. Can be null or empty to indicate an invalid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Chapter}"/> with the chapter details if found; otherwise, the result may indicate an error
    /// or a not found status.</returns>
    Task<SpotifyResult<Chapter>> ChapterGetAsync(string? id, CancellationToken cancellationToken = default);
}
