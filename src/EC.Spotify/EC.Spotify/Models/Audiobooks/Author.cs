using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Audiobooks;

public class Author
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
