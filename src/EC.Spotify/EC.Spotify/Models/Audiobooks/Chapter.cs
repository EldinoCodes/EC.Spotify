using EC.Spotify.Abstractions.Models;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Audiobooks;

public class Chapter : IPolymorphicItem
{
    [JsonPropertyName("chapter_number")]
    public int ChapterNumber { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("html_description")]
    public string? HtmlDescription { get; set; }
    [JsonPropertyName("duration_ms")]
    public long? DurationMilliseconds { get; set; }
    [JsonPropertyName("explicit")]
    public bool Explicit { get; set; }
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
    [JsonPropertyName("is_playable")]
    public bool IsPlayable { get; set; }
    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }
    [JsonPropertyName("name")]
    public string? name { get; set; }
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }
    [JsonPropertyName("release_date_precision")]
    public string? ReleaseDatePrecision { get; set; }
    [JsonPropertyName("resume_point")]
    public ResumePoint? ResumePoint { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    [JsonPropertyName("restrictions")]
    public Restriction? Restrictions { get; set; }
}
