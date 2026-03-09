using EC.Spotify.Abstractions.Models;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Players;

public class PlayerQueue
{
    [JsonPropertyName("currently_playing")]
    public IPolymorphicItem? CurrentlyPlaying { get; set; }
    [JsonPropertyName("queue")]
    public List<IPolymorphicItem>? Queue { get; set; }
}
