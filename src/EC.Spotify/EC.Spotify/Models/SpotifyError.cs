
namespace EC.Spotify.Models;

/// <summary>
/// Represents an error returned by the Spotify API or generated internally by the EC.Spotify library.
/// </summary>
public class SpotifyError
{
    /// <summary>
    /// Gets or sets the HTTP status code associated with the error. Set to <c>500</c> for errors generated
    /// internally by the library.
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// Gets or sets a human-readable description of the error.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets a machine-readable reason for the error. For internally generated errors this contains
    /// the exception type name; for Spotify API errors this contains Spotify's reason string.
    /// </summary>
    public string? Reason { get; set; }
}
