using EC.Spotify.Abstractions.Models;
using EC.Spotify.Enums;
using EC.Spotify.Models.Shared;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Albums;

public class Album : IPolymorphicItem
{
    [JsonPropertyName("album_type")]
    public AlbumType? AlbumType { get; set; }

    [JsonPropertyName("total_tracks")]
    public int TotalTracks { get; set; }

    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("release_date_precision")]
    public string? ReleaseDatePrecision { get; set; }

    [JsonPropertyName("restrictions")]
    public Restriction? Restrictions { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("artists")]
    public List<Artist>? Artists { get; set; }

    [JsonPropertyName("tracks")]
    public SpotifyPageResult<Track>? Tracks { get; set; }

    [JsonPropertyName("copyrights")]
    public List<Copyright>? Copyrights { get; set; }
}