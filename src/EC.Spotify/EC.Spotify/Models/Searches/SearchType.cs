namespace EC.Spotify.Models.Searches;

[Flags]
public enum SearchType
{
    Album = 1,
    Artist = 2,
    Audiobook = 64,
    Playlist = 4,
    Track = 8,
    Show = 16,
    Episode = 32
}
