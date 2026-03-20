using EC.Spotify.Abstractions.Models;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Playlists;

public class PlaylistTrack : IPolymorphicItem
{
    [JsonPropertyName("added_at")]
    public string? AddedAt { get; set; }
    [JsonPropertyName("added_by")]
    public User? AddedBy { get; set; }
    [JsonPropertyName("is_local")]
    public bool IsLocal { get; set; }
    [JsonPropertyName("item")]
    public /*IPlayerItem*/IPolymorphicItem? Item { get; set; }
}

