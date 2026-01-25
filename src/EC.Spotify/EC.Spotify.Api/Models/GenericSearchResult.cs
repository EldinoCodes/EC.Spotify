using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class GenericSearchResult<T> where T : class
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    [JsonPropertyName("next")]
    public string? Next { get; set; }
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    [JsonPropertyName("previous")]
    public object? Previous { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = [];
}
