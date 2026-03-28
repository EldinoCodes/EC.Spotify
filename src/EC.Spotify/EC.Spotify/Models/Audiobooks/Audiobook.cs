using EC.Spotify.Abstractions.Models;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Audiobooks;

public class Audiobook : IPolymorphicItem
{
    [JsonPropertyName("authors")]
    public List<Author>? Authors { get; set; }
    [JsonPropertyName("copyrights")]
    public List<Copyright>? Copyrights { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("html_description")]
    public string? HtmlDescription { get; set; }
    [JsonPropertyName("edition")]
    public string? Edition { get; set; }
    [JsonPropertyName("explicit")]
    public bool Explicit { get; set; }
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
    public List<string>? Languages { get; set; }
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("narrators")]
    public List<Narrator>? Narrators { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    [JsonPropertyName("total_chapters")]
    public int TotalChapters { get; set; }
    [JsonPropertyName("chapters")]
    public SpotifyPageResult<Chapter>? Chapters { get; set; }
}
