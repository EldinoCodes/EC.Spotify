using System.Text.Json.Serialization;
using EC.Spotify.Models.Albums;

namespace EC.Spotify.Models.Players;

/// <summary>
/// Represents a recently played track or episode from the user's listening history.
/// </summary>
public class RecentlyPlayedItem
{
    /// <summary>
    /// The Spotify ID for the item.
    /// </summary>
    [JsonPropertyName("track_id")]
    public string? TrackId { get; set; }

    /// <summary>
    /// The Spotify track or episode object.
    /// </summary>
    [JsonPropertyName("track")]
    public Track? Track { get; set; }

    /// <summary>
    /// The time the item was played.
    /// </summary>
    [JsonPropertyName("played_at")]
    public string? PlayedAt { get; set; }

    /// <summary>
    /// The context that the item was played from.
    /// </summary>
    [JsonPropertyName("context")]
    public Context? Context { get; set; }
}
