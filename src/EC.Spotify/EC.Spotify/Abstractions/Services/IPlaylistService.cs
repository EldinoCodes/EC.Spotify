using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using EC.Spotify.Models.Playlists;
using EC.Spotify.Models.Shared;

namespace EC.Spotify.Abstractions.Services;

public interface IPlaylistService
{
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
    /// <remarks>Requires the <c>playlist-read-private</c> scope.</remarks>
    /// <param name="id">The Spotify ID of the playlist to retrieve items from. Can be null to indicate no playlist.</param>
    /// <param name="limit">The maximum number of items to return in the result. Must be a positive integer. The default is 20.</param>
    /// <param name="offset">The index of the first item to return. Must be zero or greater. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult holding the playlist items.</returns>
    Task<SpotifyResult<PlaylistPageResult>> PlaylistItemGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the details of an existing playlist with the specified information.
    /// </summary>
    /// <remarks>Requires the <c>playlist-modify-public</c> and <c>playlist-modify-private</c> scopes.</remarks>
    /// <param name="id">The unique identifier of the playlist to update. Can be null to indicate no playlist will be updated.</param>
    /// <param name="playlistDetail">The new details to apply to the playlist. If null, no changes will be made.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult indicating true if
    /// the update was successful; otherwise, false.</returns>
    Task<SpotifyResult<bool>> PlaylistDetailUpdateAsync(string? id, PlaylistDetail? playlistDetail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds all specified items to a playlist asynchronously at the given position.
    /// </summary>
    /// <remarks>If the playlist does not exist or the user does not have permission to modify it, the
    /// operation may fail. The order of items in the libraryItems list is preserved in the playlist.</remarks>
    /// <param name="id">The unique identifier of the playlist to which the items will be added. Can be null to indicate a default or
    /// current playlist, if supported.</param>
    /// <param name="libraryItems">The list of items to add to the playlist. Each item represents a reference to a track or other supported entity.
    /// Cannot be null or empty.</param>
    /// <param name="position">The zero-based index at which to insert the items in the playlist. If null, items are added to the end of the
    /// playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a list of
    /// PlaylistSnapshot objects representing the state of the playlist after the items are added.</returns>
    Task<SpotifyResult<List<PlaylistSnapshot>>> PlaylistItemAddAllAsync(string? id, List<ReferenceItem> libraryItems, int? position = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Adds an item to a playlist asynchronously at the specified position.
    /// </summary>
    /// <remarks>If the specified position is out of range, the item will be added to the end of the playlist.
    /// The operation may fail if the playlist is collaborative and the user does not have permission to modify
    /// it.</remarks>
    /// <param name="id">The Spotify ID of the playlist to which the item will be added. Cannot be null or empty.</param>
    /// <param name="libraryItem">The item to add to the playlist. Represents a track or episode reference. Cannot be null.</param>
    /// <param name="position">The zero-based position at which to insert the item. If null, the item is added to the end of the playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// PlaylistSnapshot indicating the state of the playlist after the item is added.</returns>
    Task<SpotifyResult<PlaylistSnapshot>> PlaylistItemAddAsync(string? id, ReferenceItem? libraryItem, int? position = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Removes all specified items from the playlist with the given identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the playlist from which items will be removed. Can be null to indicate no playlist.</param>
    /// <param name="libraryItems">A list of reference items representing the tracks to remove from the playlist. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a list of
    /// playlist snapshots reflecting the state of the playlist after the removals.</returns>
    Task<SpotifyResult<List<PlaylistSnapshot>>> PlaylistItemRemoveAllAsync(string? id, List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
    /// <summary>
    /// Removes a specified item from a playlist asynchronously and returns the resulting playlist snapshot.
    /// </summary>
    /// <param name="id">The unique identifier of the playlist from which the item will be removed. Cannot be null or empty.</param>
    /// <param name="libraryItem">The item to remove from the playlist. Must not be null and must reference a valid item in the playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with the updated
    /// playlist snapshot after the item is removed.</returns>
    Task<SpotifyResult<PlaylistSnapshot>> PlaylistItemRemoveAsync(string? id, ReferenceItem? libraryItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds or replaces the image for the specified playlist asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>ugc-image-upload</c>, <c>playlist-modify-public</c>, and <c>playlist-modify-private</c> scopes. The image replaces any existing playlist image. The operation may fail if the image does not
    /// meet Spotify's requirements or if the user does not have permission to modify the playlist.</remarks>
    /// <param name="id">The Spotify ID of the playlist to update. Cannot be null or empty.</param>
    /// <param name="imageData">A byte array containing the image data in JPEG format. The image must be a valid JPEG and meet Spotify's size
    /// requirements. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult indicating whether
    /// the image was successfully added or replaced.</returns>
    Task<SpotifyResult<bool>> PlaylistImageAddAsync(string? id, byte[]? imageData, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves all images associated with the specified playlist asynchronously.
    /// </summary>
    /// <param name="id">The Spotify ID of the playlist for which to retrieve images. Can be null to indicate no playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a list of
    /// images for the specified playlist. The list will be empty if the playlist has no images.</returns>
    Task<SpotifyResult<List<Image>>> PlaylistImageGetAllAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw playlist JSON from Spotify asynchronously by playlist identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the playlist to retrieve. If null, the method will not attempt to retrieve a playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> PlaylistGetRawAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated playlist item JSON from Spotify asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>playlist-read-private</c> scope.</remarks>
    /// <param name="id">The Spotify ID of the playlist to retrieve items from. Can be null to indicate no playlist.</param>
    /// <param name="limit">The maximum number of items to return. The default is 20.</param>
    /// <param name="offset">The index of the first item to return. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> PlaylistItemGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw playlist image JSON from Spotify asynchronously by playlist identifier.
    /// </summary>
    /// <param name="id">The Spotify ID of the playlist for which to retrieve images. Can be null to indicate no playlist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> PlaylistImageGetAllRawAsync(string? id, CancellationToken cancellationToken = default);
}