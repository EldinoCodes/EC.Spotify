using EC.Spotify.Abstractions.Models;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Players;

public class PlayerState
{
    [JsonPropertyName("device")]
    public Device? Device { get; set; }
    [JsonPropertyName("repeat_state")]
    public string? RepeatState { get; set; }
    [JsonPropertyName("shuffle_state")]
    public bool ShuffleState { get; set; }
    [JsonPropertyName("context")]
    public Context? Context { get; set; }
    [JsonPropertyName("timestamp")]
    public long? Timestamp { get; set; }
    [JsonPropertyName("progress_ms")]
    public int? ProgressMilliseconds { get; set; }
    [JsonPropertyName("is_playing")]
    public bool IsPlaying { get; set; }
    [JsonPropertyName("item")]
    public IPolymorphicItem? Item { get; set; }
    [JsonPropertyName("currently_playing_type")]
    public string? CurrentlyPlayingType { get; set; }
    [JsonPropertyName("actions")]
    public PlayerActions? Actions { get; set; }
}
