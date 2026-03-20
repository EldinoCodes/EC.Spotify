using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Playlists;

public class PlaylistPageResult
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
        set
        {
            _next = Uri.TryCreate(value, new UriCreationOptions(), out var uri) ? uri.Query : null;
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
            _prev = Uri.TryCreate(value, new UriCreationOptions(), out var uri) ? uri.Query : null;
        }
    }
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("items")]
    public List<PlaylistTrack>? Items { get; set; }
}
