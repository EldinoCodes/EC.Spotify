 using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class Error
{
    [JsonPropertyName("status")]
    public int? Status { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}
