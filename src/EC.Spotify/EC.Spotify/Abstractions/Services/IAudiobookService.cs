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
    /// Retrieves raw audiobook JSON from Spotify asynchronously by audiobook identifier.
    /// </summary>
    /// <param name="id">The Spotify ID of the audiobook to retrieve. Can be null to indicate no audiobook is specified.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> AudiobookGetRawAsync(string? id, CancellationToken cancellationToken = default);
    
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

    /// <summary>
    /// Retrieves raw paginated chapter JSON for the specified audiobook from Spotify asynchronously.
    /// </summary>
    /// <param name="id">The Spotify ID of the audiobook for which chapters are to be retrieved. Can be null to indicate no audiobook.</param>
    /// <param name="limit">The maximum number of chapters to return. The default is 20.</param>
    /// <param name="offset">The index of the first chapter to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> AudiobookChapterGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of Audiobooks saved in the current user's library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope.</remarks>
    /// <param name="limit">The maximum number of Audiobooks to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first Audiobook to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult of Audiobook objects for the user's saved Audiobooks.</returns>
    [Obsolete("This method is deprecated. Use IUserService.MyAudiobookGetAllAsync instead.")]
    Task<SpotifyResult<SpotifyPageResult<Audiobook>>> MyAudiobookGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated Audiobook JSON for Audiobooks saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of Audiobooks to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first Audiobook to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    [Obsolete("This method is deprecated. Use IUserService.MyAudiobookGetAllRawAsync instead.")]
    Task<string?> MyAudiobookGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
