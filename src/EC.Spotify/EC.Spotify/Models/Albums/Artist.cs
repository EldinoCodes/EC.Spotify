using EC.Spotify.Abstractions.Models;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Albums;

public class Artist : IPolymorphicItem
{
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}
