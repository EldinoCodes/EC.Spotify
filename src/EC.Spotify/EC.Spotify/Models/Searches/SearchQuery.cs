using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Searches;

public partial class SearchQuery
{
    public string? ArtistName { get; set; }
    public string? AlbumName { get; set; }
    public string? TrackName { get; set; }
    public string? Genre { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SearchType Type { get; set; }

    public int? Limit { get; set; }
    public int? Offset { get; set; }
}