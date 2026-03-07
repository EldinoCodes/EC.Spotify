using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class Restriction
{
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}
