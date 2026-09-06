using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Players;

namespace EC.Spotify.Abstractions.Services;

public interface IPlayerService
{
    /// <summary>
    /// Retrieves the current playback state for the user's Spotify account asynchronously.
    /// </summary>
    /// <remarks>Returns information about the user's current playback, including the active device, repeat
    /// state, shuffle state, and the currently playing item. Requires the
    /// <c>user-read-playback-state</c> scope.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{PlayerState}"/> with the user's current playback state, or an error if the
    /// request fails or the user has no active session.</returns>
    Task<SpotifyResult<PlayerState>> StateGetAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Transfers playback to the specified device asynchronously, optionally starting playback on the device.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. If the specified device is not available or cannot be transferred to, the operation may fail.
    /// This method does not change the playback state unless <paramref name="play"/> is set to <see
    /// langword="true"/>.</remarks>
    /// <param name="deviceId">The identifier of the target device to which playback should be transferred. If null, the currently active
    /// device is used.</param>
    /// <param name="play">A value indicating whether playback should begin immediately on the target device. Set to <see langword="true"/>
    /// to start playback; otherwise, playback will not start automatically.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the transfer operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Boolean}"/> indicating whether the transfer was successful.</returns>
    Task<SpotifyResult<bool>> TransferAsync(string? deviceId, bool play = false, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a list of all available devices associated with the user's Spotify account asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-read-playback-state</c> scope.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{List{Device}}"/> with the list of devices. The list will be empty if no devices are
    /// available.</returns>
    Task<SpotifyResult<List<Device>>> DeviceGetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves the item currently playing on the user's Spotify account asynchronously.
    /// </summary>
    /// <remarks>Returns the full playback state for the currently playing item, which may be a track,
    /// episode, or other media type. Requires the <c>user-read-currently-playing</c> scope. Returns an empty
    /// result if nothing is currently playing.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{PlayerState}"/> describing the currently playing item and its playback
    /// context.</returns>
    Task<SpotifyResult<PlayerState>> CurrentlyPlayingGetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts playback of the specified tracks on the given Spotify device asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. If the device is not available or the user does not have playback permissions, the operation
    /// may fail. This method does not wait for playback to complete; it only initiates playback.</remarks>
    /// <param name="deviceId">The identifier of the target Spotify device on which to start playback. If null, playback will occur on the
    /// user's currently active device.</param>
    /// <param name="trackUris">A list of Spotify track URIs to play. If null or empty, playback resumes the user's current queue.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the playback request.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a value indicating whether playback was
    /// successfully started.</returns>
    Task<SpotifyResult<bool>> PlayAsync(string? deviceId, List<string>? trackUris, CancellationToken cancellationToken = default);
    /// <summary>
    /// Pauses playback on the user's Spotify account.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. The pause always applies to the currently active device; <c>deviceId</c> is not supported by the Spotify API for this endpoint.</remarks>
    /// <param name="deviceId">The identifier of the target Spotify device on which to pause playback. If null, the currently active device is used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the pause operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SpotifyResult{bool}"/>
    /// indicating whether the pause request was successful.</returns>
    Task<SpotifyResult<bool>> PauseAsync(string? deviceId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Skips to the next track in the user's currently active Spotify player.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. This method requires the user to have an active Spotify playback session. If no device is
    /// active, the operation may fail.</remarks>
    /// <param name="deviceId">The identifier of the target device on which to perform the action. If null, the currently active device is
    /// used.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a cancellation token allows the operation to be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult indicating whether
    /// the request was successful.</returns>
    Task<SpotifyResult<bool>> NextAsync(string? deviceId = null, CancellationToken cancellationToken = default);    
    /// <summary>
    /// Skips to the previous track in the user's currently active Spotify player.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. This method requires the user to be authenticated and have an active Spotify playback
    /// session. If no device is active, the operation may fail.</remarks>
    /// <param name="deviceId">The identifier of the target device on which to control playback. If null, the currently active device is used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Boolean}"/> indicating whether the request was successful.</returns>
    Task<SpotifyResult<bool>> PreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Seeks to the specified position in the currently playing track on the user's Spotify player.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. If playback is not active or the specified position exceeds the track's duration, the
    /// operation may fail. This method does not start playback if the player is paused.</remarks>
    /// <param name="positionMs">The position, in milliseconds, to seek to within the current track. Must be between 0 and the track's duration.</param>
    /// <param name="deviceId">The identifier of the target Spotify device. If null, the currently active device is used.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests while performing the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a value indicating whether the seek
    /// operation was successful.</returns>
    Task<SpotifyResult<bool>> SeekAsync(long positionMs, string? deviceId = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Sets the repeat mode for the current Spotify playback session asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. If the specified device is not active or available, the operation may fail. This method does
    /// not change playback state; it only updates the repeat mode.</remarks>
    /// <param name="playerRepeatMode">The repeat mode to apply to the playback session. Defaults to <see cref="PlayerRepeatMode.Off"/>.</param>
    /// <param name="deviceId">The identifier of the target device. If <see langword="null"/>, the user's currently active device is used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Boolean}"/> indicating whether the repeat mode was successfully set.</returns>
    Task<SpotifyResult<bool>> RepeatAsync(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off, string? deviceId = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Sets the playback volume for the specified Spotify device asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. If the device is not active or does not support volume control, the operation may fail. This
    /// method does not change playback state; it only adjusts the volume.</remarks>
    /// <param name="volumePercent">The desired volume level as a percentage, ranging from 0 (muted) to 100 (maximum volume).</param>
    /// <param name="deviceId">The unique identifier of the target device. If null, the currently active device is used.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests while the operation is in progress.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult indicating whether
    /// the volume was successfully set.</returns>
    Task<SpotifyResult<bool>> VolumeAsync(int volumePercent, string? deviceId = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Enables or disables shuffle mode for the user's playback on Spotify.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. If no device is specified, the shuffle setting is applied to the user's currently active
    /// device. The operation may fail if playback is not active.</remarks>
    /// <param name="playerShuffleMode">Specifies whether shuffle mode should be turned on or off. The default is <see cref="PlayerShuffleMode.Off"/>.</param>
    /// <param name="deviceId">The identifier of the target device on which to apply the shuffle setting. If <see langword="null"/>, the user's
    /// currently active device is used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="SpotifyResult{Boolean}"/> indicating whether the shuffle mode was successfully updated. The <see
    /// langword="true"/> value represents a successful operation; otherwise, <see langword="false"/>.</returns>
    Task<SpotifyResult<bool>> ShuffleAsync(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off, string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of the user's recently played tracks asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-read-recently-played</c> scope. The results are paginated and limited to 50 items per request.
    /// This method returns the most recently played tracks or episodes from the user's listening history.</remarks>
    /// <param name="limit">The maximum number of items to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="after">A timestamp in RFC3339 format (e.g., "2022-01-01T00:00:00Z") specifying the point in time after which to return results.
    /// Used for pagination to retrieve older items.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{SpotifyPageResult{RecentlyPlayedItem}}"/> with the user's recently played tracks.</returns>
    Task<SpotifyResult<SpotifyPageResult<RecentlyPlayedItem>>> RecentlyPlayedGetAllAsync(int? limit = 20, string? after = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the current playback queue for the user asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-read-playback-state</c> and <c>user-read-currently-playing</c> scopes.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{PlayerQueue}"/> with the user's current playback queue.</returns>
    Task<SpotifyResult<PlayerQueue>> QueueGetAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Adds the specified track to the playback queue on the user's Spotify account asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-modify-playback-state</c> scope. If the specified device is not active or available, the operation may fail. The method does
    /// not start playback; it only queues the track for future playback.</remarks>
    /// <param name="trackId">The Spotify track identifier to add to the queue. Cannot be null or empty.</param>
    /// <param name="deviceId">The identifier of the target playback device. If null, the user's currently active device is used.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Allows the operation to be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a value indicating whether the track was
    /// successfully added to the queue.</returns>
    Task<SpotifyResult<bool>> QueueAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default);
}