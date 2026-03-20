using EC.Spotify.Abstractions.Models;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Playlists;

public class Playlist : IPolymorphicItem
{
    [JsonPropertyName("collaborative")]
    public bool Collaborative { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("owner")]
    public User? Owner { get; set; }
    [JsonPropertyName("public")]
    public bool? Public { get; set; }
    [JsonPropertyName("snapshot_id")]
    public string? SnapshotId { get; set; }
    [JsonPropertyName("items")]
    public PlaylistPageResult? Items { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}


