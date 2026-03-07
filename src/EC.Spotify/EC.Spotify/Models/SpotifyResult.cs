using EC.Spotify.Extensions;
using System.Text.Json.Serialization;

namespace EC.Spotify.Models;

public class SpotifyResult<T>    
{
    [JsonIgnore]
    public string? Raw { get; set; }
    public T? Value { get; set; }
    public SpotifyError? Error { get; set; }
}
