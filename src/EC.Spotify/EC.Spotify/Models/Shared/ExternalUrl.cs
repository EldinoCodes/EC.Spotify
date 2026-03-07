using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class ExternalUrl
{
    [JsonPropertyName("spotify")]
    public string? Spotify { get; set; }
}
