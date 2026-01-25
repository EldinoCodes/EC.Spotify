 using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class ErrorResult
{
    [JsonPropertyName("error")]
    public Error? Error { get; set; }
}
