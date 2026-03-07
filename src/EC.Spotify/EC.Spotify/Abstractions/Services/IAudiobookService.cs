using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;

namespace EC.Spotify.Abstractions.Services;

public interface IAudiobookService 
{
    /// <summary>
    /// Retrieves the details of an audiobook by its Spotify identifier asynchronously.
    /// </summary>
    /// <remarks>If the specified ID does not correspond to an existing audiobook, the result will indicate a
    /// not found status. This method does not throw an exception for missing audiobooks; check the result for success
    /// or failure.</remarks>
    /// <param name="id">The Spotify ID of the audiobook to retrieve. Can be null to indicate no audiobook is specified.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Audiobook}"/> with the audiobook details if found; otherwise, the result may indicate an
    /// error or missing item.</returns>
    Task<SpotifyResult<Audiobook>> AudiobookGetAsync(string? id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a paginated list of chapters for the specified audiobook asynchronously.
    /// </summary>
    /// <param name="id">The Spotify ID of the audiobook for which chapters are to be retrieved. Can be null to indicate no audiobook is
    /// specified.</param>
    /// <param name="limit">The maximum number of chapters to return. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first chapter to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Chapter objects for the specified audiobook.</returns>
    Task<SpotifyResult<SpotifyPageResult<Chapter>>> AudiobookChapterGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
