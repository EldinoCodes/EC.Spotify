using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.Models.Players;
using EC.Spotify.Models.Shows;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Searches;

public class SearchResult
{
    [JsonPropertyName("albums")]
    public SpotifyPageResult<Album>? Albums { get; set; }
    [JsonPropertyName("artists")]
    public SpotifyPageResult<Artist>? Artists { get; set; }
    [JsonPropertyName("audiobooks")]
    public SpotifyPageResult<Audiobook>? Audiobooks { get; set; }
    [JsonPropertyName("episodes")]
    public SpotifyPageResult<Episode>? Episodes { get; set; }
    [JsonPropertyName("playlists")]
    public SpotifyPageResult<Playlist>? Playlists { get; set; }
    [JsonPropertyName("shows")]
    public SpotifyPageResult<Show>? Shows { get; set; }
    [JsonPropertyName("tracks")]
    public SpotifyPageResult<Track>? Tracks { get; set; }
}
