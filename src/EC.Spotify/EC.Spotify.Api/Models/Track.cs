using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class Track
{
    [JsonPropertyName("album")]
    public Album? Album { get; set; }
    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; set; } = [];
    [JsonPropertyName("duration_ms")]
    public long? DurationMilliseconds { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("popularity")]
    public int Popularity { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}