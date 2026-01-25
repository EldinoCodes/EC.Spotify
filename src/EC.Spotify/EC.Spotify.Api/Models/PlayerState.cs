using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class PlayerState
{
    [JsonPropertyName("currently_playing")]
    public Track? CurrentlyPlaying { get; set; }
    [JsonPropertyName("queue")]
    public List<Track> Queue { get; set; } = [];
}
