using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class ExternalId
{
    [JsonPropertyName("isrc")]
    public string? Isrc { get; set; }
    [JsonPropertyName("ean")]
    public string? Ean { get; set; }
    [JsonPropertyName("upc")]
    public string? Upc { get; set; }
}
