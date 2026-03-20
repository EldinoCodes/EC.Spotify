using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Playlists;

public class PlaylistDetail
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("public")]
    public bool? Public { get; set; }
    [JsonPropertyName("collaborative")]
    public bool Collaborative { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
