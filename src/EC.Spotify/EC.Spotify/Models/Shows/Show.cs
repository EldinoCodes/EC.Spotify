using EC.Spotify.Abstractions.Models;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shows;

public class Show : IPolymorphicItem
{
    [JsonPropertyName("copyrights")]
    public List<Copyright>? Copyrights { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("html_description")]
    public string? HtmlDescription { get; set; }
    [JsonPropertyName("explicit")]
    public bool Explicit { get; set; }
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
    [JsonPropertyName("is_externally_hosted")]
    public bool IsExternallyHosted { get; set; }
    [JsonPropertyName("is_playable")]
    public List<string>? Languages { get; set; }
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("publisher")]
    public string? Publisher { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    [JsonPropertyName("total_episodes")]
    public int TotalEpisodes { get; set; }
}
