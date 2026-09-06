using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Playlists;

/// <summary>
/// Represents the request body for creating a new playlist.
/// </summary>
public class PlaylistCreate
{
    /// <summary>
    /// The name for the new playlist.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Whether the playlist should be public. Default is true.
    /// </summary>
    [JsonPropertyName("public")]
    public bool? Public { get; set; }

    /// <summary>
    /// Whether the playlist can be collaborative. Default is false.
    /// </summary>
    [JsonPropertyName("collaborative")]
    public bool? Collaborative { get; set; }

    /// <summary>
    /// Optional description for the playlist.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
