using EC.Spotify.Models;
using EC.Spotify.Models.Library;

namespace EC.Spotify.Abstractions.Services;

public interface ILibraryService
{
    /// <summary>
    /// Saves the specified items to the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-modify</c> scope. Items are sent in batches of up to 40
    /// per request to comply with Spotify API limits.</remarks>
    /// <param name="libraryItems">A list of reference items to save to the user's library. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a boolean value for each item indicating whether it was successfully
    /// saved.</returns>
    Task<SpotifyResult<List<bool>>> LibraryAddAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the specified item to the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-modify</c> scope.</remarks>
    /// <param name="libraryItem">The reference item to save. If <see langword="null"/>, no action is taken.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> indicating whether the item was successfully saved.</returns>
    Task<SpotifyResult<bool>> LibraryAddAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether the specified items are saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope. Items are checked in batches of up to 40
    /// per request to comply with Spotify API limits.</remarks>
    /// <param name="libraryItems">A list of reference items to check. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a boolean value for each item indicating whether it is saved in
    /// the user's library.</returns>
    Task<SpotifyResult<List<bool>>> LibraryCheckAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether the specified item is saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope.</remarks>
    /// <param name="libraryItem">The reference item to check. If <see langword="null"/>, the result will be <see langword="false"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> indicating whether the item is saved in the user's library.</returns>
    Task<SpotifyResult<bool>> LibraryCheckAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the specified items from the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-modify</c> scope. Items are sent in batches of up to 40
    /// per request to comply with Spotify API limits.</remarks>
    /// <param name="libraryItems">A list of reference items to remove from the user's library. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a boolean value for each item indicating whether it was successfully
    /// removed.</returns>
    Task<SpotifyResult<List<bool>>> LibraryRemoveAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the specified item from the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-modify</c> scope.</remarks>
    /// <param name="libraryItem">The reference item to remove. If <see langword="null"/>, no action is taken.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> indicating whether the item was successfully removed.</returns>
    Task<SpotifyResult<bool>> LibraryRemoveAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);
}