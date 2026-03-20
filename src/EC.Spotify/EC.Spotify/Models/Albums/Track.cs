using EC.Spotify.Abstractions.Models;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Albums;

public class Track : IPolymorphicItem
{
    [JsonPropertyName("album")]
    public Album? Album { get; set; }
    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; set; } = [];
    [JsonPropertyName("disc_number")]
    public int DiskNumber { get; set; }
    [JsonPropertyName("duration_ms")]
    public long? DurationMilliseconds { get; set; }
    [JsonPropertyName("explicit")]
    public bool Explicit { get; set; }
    [JsonPropertyName("external_ids")]
    public ExternalId? ExternalIds { get; set; }
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("is_playable")]
    public bool IsPlayable { get; set; }
    [JsonPropertyName("restrictions")]
    public Restriction? Restrictions { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("track_number")]
    public int TrackNumber { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    [JsonPropertyName("is_local")]
    public bool IsLocal { get; set; }
}