using EC.Spotify.Abstractions.Models;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Players;

public class PlayerQueue
{
    [JsonPropertyName("currently_playing")]
    public IPlayerItem? CurrentlyPlaying { get; set; }
    [JsonPropertyName("queue")]
    public List<IPlayerItem>? Queue { get; set; }
}
