using EC.Spotify.Models;
using EC.Spotify.Models.Shows;

namespace EC.Spotify.Abstractions.Services;

public interface IEpisodeService 
{
    /// <summary>
    /// Retrieves the details of a Spotify episode by its unique identifier asynchronously.
    /// </summary>
    /// <remarks>Requires the <c>user-read-playback-position</c> scope. The operation may fail if the
    /// episode does not exist or if the ID is invalid. The result object provides error information in
    /// such cases.</remarks>
    /// <param name="id">The Spotify episode ID to retrieve. Can be null to indicate no episode; if null or invalid, the result will
    /// indicate an error.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{Episode}"/> with the episode details if found; otherwise, an error result.</returns>
    Task<SpotifyResult<Episode>> EpisodeGetAsync(string? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves raw episode JSON from Spotify asynchronously by episode identifier.
    /// </summary>
    /// <remarks>Requires the <c>user-read-playback-position</c> scope.</remarks>
    /// <param name="id">The Spotify episode ID to retrieve. Can be null or invalid.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the raw JSON string
    /// response, or null if no content was returned.</returns>
    Task<string?> EpisodeGetRawAsync(string? id, CancellationToken cancellationToken = default);
}
