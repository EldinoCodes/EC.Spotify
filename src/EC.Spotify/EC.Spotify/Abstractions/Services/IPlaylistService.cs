using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using EC.Spotify.Models.Playlists;

namespace EC.Spotify.Abstractions.Services;

public interface IPlaylistService
{
    /// <summary>
    /// Retrieves all playlists owned or followed by the current user asynchronously.
    /// </summary>
    /// <remarks>The result may be empty if the user does not have any playlists. This method supports
    /// cancellation via the provided token.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult holding the user's playlists.</returns>
    Task<SpotifyResult<SpotifyPageResult>> MyPlaylistGetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the details of a Spotify playlist by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the playlist to retrieve. If null, the method will not attempt to retrieve a playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult object with the
    /// playlist details if found; otherwise, the result may indicate an error or that the playlist does not exist.</returns>
    Task<SpotifyResult<Playlist>> PlaylistGetAsync(string? id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a paged list of items from the specified Spotify playlist asynchronously.
    /// </summary>
    /// <param name="id">The Spotify ID of the playlist to retrieve items from. Can be null to indicate no playlist.</param>
    /// <param name="limit">The maximum number of items to return in the result. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first item to return. Must be zero or greater. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult holding the playlist items.</returns>
    Task<SpotifyResult<SpotifyPageResult>> PlaylistItemGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the details of an existing playlist with the specified information.
    /// </summary>
    /// <param name="id">The unique identifier of the playlist to update. Can be null to indicate no playlist will be updated.</param>
    /// <param name="playlistDetail">The new details to apply to the playlist. If null, no changes will be made.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult indicating true if
    /// the update was successful; otherwise, false.</returns>
    Task<SpotifyResult<bool>> PlaylistDetailUpdateAsync(string? id, PlaylistDetail? playlistDetail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds all specified items to a playlist asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the playlist to which the items will be added. Can be null to indicate the current
    /// user's default playlist.</param>
    /// <param name="libraryItems">A list of reference items to add to the playlist. Each item represents a track or media to be added. Cannot be
    /// null or empty.</param>
    /// <param name="position">The zero-based position in the playlist at which to insert the new items. If null, items are added to the end of
    /// the playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a list of
    /// boolean values indicating the success or failure of adding each item.</returns>
    Task<SpotifyResult<List<bool>>> PlaylistItemAddAllAsync(string? id, List<ReferenceItem> libraryItems, int? position = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Adds an item to a playlist asynchronously at the specified position.
    /// </summary>
    /// <param name="id">The identifier of the playlist to which the item will be added. Can be null if the playlist is specified by
    /// other means.</param>
    /// <param name="libraryItem">The item to add to the playlist. Represents a reference to a track or other supported media. Cannot be null.</param>
    /// <param name="position">The zero-based position at which to insert the item in the playlist. If null, the item is added to the end of
    /// the playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult indicating whether
    /// the item was successfully added.</returns>
    Task<SpotifyResult<bool>> PlaylistItemAddAsync(string? id, ReferenceItem? libraryItem, int? position = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Removes all specified items from the playlist with the given identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the playlist from which items will be removed. Can be null to indicate the current
    /// user's default playlist.</param>
    /// <param name="libraryItems">A list of reference items representing the tracks or playlist items to remove. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a list of
    /// boolean values indicating whether each corresponding item was successfully removed.</returns>
    Task<SpotifyResult<List<bool>>> PlaylistItemRemoveAllAsync(string? id, List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
    /// <summary>
    /// Removes an item from a playlist asynchronously.
    /// </summary>
    /// <remarks>Either <paramref name="id"/> or <paramref name="libraryItem"/> must be provided to identify
    /// the item to remove. If both are null, the operation will not remove any item.</remarks>
    /// <param name="id">The unique identifier of the playlist item to remove. Can be null if the item is specified by <paramref
    /// name="libraryItem"/>.</param>
    /// <param name="libraryItem">A reference to the library item to remove from the playlist. Can be null if the item is specified by <paramref
    /// name="id"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Boolean}"/> indicating <see langword="true"/> if the item was successfully removed;
    /// otherwise, <see langword="false"/>.</returns>
    Task<SpotifyResult<bool>> PlaylistItemRemoveAsync(string? id, ReferenceItem? libraryItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds or replaces the image for the specified playlist asynchronously.
    /// </summary>
    /// <remarks>The image replaces any existing playlist image. The operation may fail if the image does not
    /// meet Spotify's requirements or if the user does not have permission to modify the playlist.</remarks>
    /// <param name="id">The Spotify ID of the playlist to update. Cannot be null or empty.</param>
    /// <param name="imageData">A byte array containing the image data in JPEG format. The image must be a valid JPEG and meet Spotify's size
    /// requirements. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult indicating whether
    /// the image was successfully added or replaced.</returns>
    Task<SpotifyResult<bool>> PlaylistImageAddAsync(string? id, byte[]? imageData, CancellationToken cancellationToken = default);
}