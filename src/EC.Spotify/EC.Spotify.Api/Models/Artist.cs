using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class Artist
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
