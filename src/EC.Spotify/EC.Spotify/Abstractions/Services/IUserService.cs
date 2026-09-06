using EC.Spotify.Abstractions.Models;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.Models.Playlists;
using EC.Spotify.Models.Shared;
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
    /// Retrieves raw paginated album JSON for albums saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of albums to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first album to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyAlbumGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of Artists saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope. The <paramref name="limit"/> value must be
    /// between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of Artists to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first Artist to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// Artists.</returns>
    Task<SpotifyResult<SpotifyPageResult<Artist>>> MyArtistGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated Artist JSON for Artists saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of Artists to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first Artist to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyArtistGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

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
    /// Retrieves raw paginated audiobook JSON for audiobooks saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of audiobooks to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first audiobook to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyAudiobookGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

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
    /// Retrieves raw paginated playlist JSON for playlists owned/followed by the current user from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of playlists to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first playlist to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyPlaylistGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of episodes saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope. The <paramref name="limit"/> value must be
    /// between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of episodes to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// episodes.</returns>
    Task<SpotifyResult<SpotifyPageResult<Episode>>> MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves raw paginated episode JSON for episodes saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of episodes to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first episode to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyEpisodeGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of shows saved in the current user's Spotify library asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-library-read</c> scope. The <paramref name="limit"/> value must be
    /// between 1 and 50.</remarks>
    /// <param name="limit">The maximum number of shows to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first show to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{T}"/> with a <see cref="SpotifyPageResult{T}"/> holding the user's saved
    /// shows.</returns>
    Task<SpotifyResult<SpotifyPageResult<Show>>> MyShowGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw paginated show JSON for shows saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of shows to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first show to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyShowGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

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
    /// Retrieves raw paginated track JSON for tracks saved in the current user's library from Spotify asynchronously.
    /// </summary>
    /// <param name="limit">The maximum number of tracks to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first track to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyTrackGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);

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

    /// <summary>
    /// Retrieves raw paginated top item JSON for the current user from Spotify asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-top-read</c> scope.</remarks>
    /// <param name="userTopType">The type of top items to retrieve (artists or tracks). The default is <see cref="UserTopType.Tracks"/>.</param>
    /// <param name="userTopTimeRange">The time range over which affinity is computed. The default is <see cref="UserTopTimeRange.MediumTerm"/>.</param>
    /// <param name="limit">The maximum number of items to return. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first item to return. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> MyTopItemGetAllRawAsync(UserTopType userTopType = UserTopType.Tracks, UserTopTimeRange userTopTimeRange = UserTopTimeRange.MediumTerm, int? limit = 20, int offset = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves detailed profile information about the current user asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-read-private</c> and <c>user-read-email</c> scopes. This method returns the current user's
    /// profile information including their display name, email, and Spotify ID.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{User}"/> with the current user's profile information.</returns>
    Task<SpotifyResult<User>> CurrentProfileGetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of the current user's followed artists asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-follow-read</c> scope. This method returns the artists that the current user
    /// is following on Spotify.</remarks>
    /// <param name="limit">The maximum number of artists to return. Must be between 1 and 50. The default is 20.</param>
    /// <param name="offset">The zero-based index of the first artist to return. Used for pagination. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{SpotifyPageResult{Artist}}"/> with the user's followed artists.</returns>
    Task<SpotifyResult<SpotifyPageResult<Artist>>> GetFollowingAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
}
