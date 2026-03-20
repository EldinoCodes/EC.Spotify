using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class User
{
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }
}

