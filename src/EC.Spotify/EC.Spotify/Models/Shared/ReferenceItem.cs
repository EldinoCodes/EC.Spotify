using EC.Spotify.Enums;

namespace EC.Spotify.Models.Library;

public class ReferenceItem
{
    public string? Id { get; set; }
    public ReferenceItemType Type { get; set; } = ReferenceItemType.Track;

    public string? Uri => Id is null ? null : $"spotify:{TypeName}:{Id}";

    private string TypeName => Type switch
    {
        ReferenceItemType.Album => "album",
        ReferenceItemType.Audiobook => "audiobook",
        ReferenceItemType.Episode => "episode",
        ReferenceItemType.Playlist => "playlist",
        ReferenceItemType.Show => "show",
        ReferenceItemType.Track => "track",
        ReferenceItemType.User => "user",
        _ => Type.ToString().ToLowerInvariant()
    };
}
