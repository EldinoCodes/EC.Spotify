namespace EC.Spotify;

/// <summary>
/// Configuration options for the EC.Spotify library. Bind from a configuration section or configure via
/// an <see cref="System.Action{T}"/> delegate when calling <c>AddSpotify</c>.
/// </summary>
public class SpotifyOptions
{
    /// <summary>
    /// Gets or sets the Spotify application client ID.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the Spotify application client secret.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the URI that Spotify redirects to after the user completes authorization.
    /// </summary>
    public string? RedirectUri { get; set; }

    /// <summary>
    /// Gets or sets the list of Spotify OAuth scopes requested by the application. The default is an empty
    /// list.
    /// </summary>
    public List<string> Scopes { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether verbose debug-level logging is enabled. When
    /// <see langword="true"/>, each service method emits additional log entries for each request.
    /// </summary>
    public bool VerboseLogging { get; set; }
}
