using EC.Spotify.Enums;

namespace EC.Spotify.Models.Library;

public class LibraryItem
{
    public string? Id { get; set; }
    public LibraryType Type { get; set; } = LibraryType.Track;

    public string? Uri => $"spotify:{Type.ToString().ToLower()}:{Id}";
}
