using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class ExplicitContent
{
    [JsonPropertyName("filter_enabled")]
    public bool FilterEnabled { get; set; }
    [JsonPropertyName("filter_locked")]
    public bool FilterLocked { get; set; }
}