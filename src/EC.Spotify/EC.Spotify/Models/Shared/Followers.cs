using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class Followers
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}