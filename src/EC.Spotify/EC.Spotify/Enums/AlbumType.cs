using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EC.Spotify.Enums;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AlbumType
{
    [EnumMember(Value = "album")]
    Album = 1,
    [EnumMember(Value = "single")]
    Single =  2,
    [EnumMember(Value = "appears_on")]
    AppearsOn = 4,
    [EnumMember(Value = "compilation")]
    Compilation = 8
}
