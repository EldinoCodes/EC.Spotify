using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Audiobooks;

public class Narrator
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
