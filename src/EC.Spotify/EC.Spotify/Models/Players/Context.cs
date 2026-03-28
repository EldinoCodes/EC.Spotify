using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Players;

public class Context
{
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}

