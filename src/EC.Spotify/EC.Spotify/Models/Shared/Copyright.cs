using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class Copyright
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
