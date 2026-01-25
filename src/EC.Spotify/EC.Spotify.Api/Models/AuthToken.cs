using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class AuthToken
{
    [JsonPropertyName("access_token")]
    public virtual string? AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public virtual string? RefreshToken { get; set; }
    [JsonPropertyName("token_type")]
    public virtual string? TokenType { get; set; }
    [JsonPropertyName("expires_in")]
    public virtual double ExpiresIn { get; set; }
    [JsonPropertyName("scope")]
    public virtual string? Scope { get; set; }
}