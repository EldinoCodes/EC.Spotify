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

    /// <summary>
    /// Retrieves raw track JSON from Spotify asynchronously by track identifier.
    /// </summary>
    /// <param name="id">The Spotify track identifier. Can be null or empty to indicate an invalid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> TrackGetRawAsync(string? id, CancellationToken cancellationToken = default);
}
