using EC.Spotify.Models;
using EC.Spotify.Models.Albums;

namespace EC.Spotify.Abstractions.Services;

public interface ITrackService
{
    /// <summary>
    /// Retrieves the details of a Spotify track by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The Spotify track identifier. Can be null or empty to indicate no track; in such cases, the result will not
    /// contain track data.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SpotifyResult{Track}"/>
    /// with the track details if found; otherwise, the result indicates failure or not found.</returns>
    Task<SpotifyResult<Track>> TrackGetAsync(string? id, CancellationToken cancellationToken = default);
}
