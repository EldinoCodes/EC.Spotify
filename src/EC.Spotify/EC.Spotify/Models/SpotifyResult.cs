
namespace EC.Spotify.Models;

/// <summary>
/// Represents the outcome of a Spotify API operation, containing either a data payload or an error.
/// </summary>
/// <typeparam name="T">The type of the data returned by a successful operation.</typeparam>
public class SpotifyResult<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation succeeded. Returns <see langword="true"/> if <see
    /// cref="Error"/> is <see langword="null"/>; otherwise, <see langword="false"/>.
    /// </summary>
    public bool IsSuccess => Error is null;

    /// <summary>
    /// Gets or sets the data returned by a successful operation. <see langword="null"/> if the operation
    /// failed.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the error that occurred during the operation. <see langword="null"/> if the operation
    /// succeeded.
    /// </summary>
    public SpotifyError? Error { get; set; }
}
