using EC.Spotify.Enums;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Searches;

public partial class SearchQuery
{
    public string? Query { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SearchType Type { get; set; }

    public int? Limit { get; set; }
    public int? Offset { get; set; }
}