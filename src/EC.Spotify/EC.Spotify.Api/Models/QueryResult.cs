using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class QueryResult
{
    [JsonPropertyName("tracks")]
    public GenericSearchResult<Track>? Tracks { get; set; }
}
