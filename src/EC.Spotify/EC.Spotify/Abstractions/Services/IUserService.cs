using EC.Spotify.Abstractions.Models;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.Models.Playlists;
using EC.Spotify.Models.Shows;

namespace EC.Spotify.Abstractions.Services;

public interface IUserService
{
    /// <summary>
    /// Retrieves a paginated list of albums saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope. The <paramref name="limit"/> value must be
    /// between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of albums to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first album to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// albums.</returns>
    Task<SpotifyResult<SpotifyPageResult<Album>>> MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of audiobooks saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope. The <paramref name="limit"/> value must be
    /// between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of audiobooks to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first audiobook to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// audiobooks.</returns>
    Task<SpotifyResult<SpotifyPageResult<Audiobook>>> MyAudiobookGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of episodes saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> and <c>user-read-playback-position</c> scopes. The
    /// <paramref name="limit"/> value must be between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of episodes to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// episodes.</returns>
    Task<SpotifyResult<SpotifyPageResult<Episode>>> MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all playlists owned or followed by the current user asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>playlist-read-private</c> scope. The result may be empty if the user does not have any playlists. This method supports
    /// cancellation via the provided token.</remarks>
    /// <param name="limit">The maximum number of playlists to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first playlist to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a
    /// SpotifyPageResult holding the user's playlists.</returns>
    Task<SpotifyResult<SpotifyPageResult<Playlist>>> MyPlaylistGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of shows saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> and <c>user-read-playback-position</c> scopes. The
    /// <paramref name="limit"/> value must be between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of shows to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first show to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// shows.</returns>
    Task<SpotifyResult<SpotifyPageResult<Show>>> MyShowGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of tracks saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope. The <paramref name="limit"/> value must be
    /// between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of tracks to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first track to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// tracks.</returns>
    Task<SpotifyResult<SpotifyPageResult<Track>>> MyTrackGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of the current user's top artists or tracks asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-top-read</c> scope.</remarks>
    /// <param name="limit">The maximum number of items to return. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first item to return. Used for pagination. The default is 0.</param>
    /// <param name="userTopType">The type of top items to retrieve. Use <see cref="UserTopType.Artists"/> for
    /// artists or <see cref="UserTopType.Tracks"/> for tracks. The default is <see cref="UserTopType.Tracks"/>.</param>
    /// <param name="userTopTimeRange">The time range over which affinity is computed. The default is
    /// <see cref="UserTopTimeRange.MediumTerm"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's top
    /// items.</returns>
    Task<SpotifyResult<SpotifyPageResult<IPolymorphicItem>>> MyTopItemGetAllAsync(UserTopType userTopType = UserTopType.Tracks, UserTopTimeRange userTopTimeRange = UserTopTimeRange.MediumTerm, int? limit = 20, int offset = 0, CancellationToken cancellationToken = default);
}
