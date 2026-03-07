using System.Text.Json.Serialization;

namespace EC.Spotify.Models;

public class SpotifyPageResult<T> where T : class
{
    private string? _next;
    private string? _prev;

    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    
    [JsonPropertyName("next")]
    public string? Next
    {
        get
        {
            return _next;
        }
        set {
            _next = !string.IsNullOrEmpty(value) 
                ? new Uri(value).Query 
                : value;
        }
    }
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    [JsonPropertyName("previous")]
    public string? Previous
    {
        get
        {
            return _prev;
        }
        set
        {
            _prev = !string.IsNullOrEmpty(value)
                ? new Uri(value).Query
                : value;
        }
    }
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = [];
}
