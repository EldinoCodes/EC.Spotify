using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Playlists;

public class PlaylistSnapshot
{
    [JsonPropertyName("snapshot_id")]
    public string? SnapshotId { get; set; }
}
