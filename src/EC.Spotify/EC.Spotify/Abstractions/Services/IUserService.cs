using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
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
}
