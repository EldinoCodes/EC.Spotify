using System.Text.Json.Serialization;

namespace EC.Spotify.Models.Shared;

public class User
{
    [JsonPropertyName("account_id")]
    public string? AccountId { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("explicit_content")]
    public ExplicitContent? ExplicitContent { get; set; }    
    [JsonPropertyName("external_urls")]
    public ExternalUrl? ExternalUrls { get; set; }
    [JsonPropertyName("followers")]
    public Followers? Followers { get; set; }
    [JsonPropertyName("href")]
    public string? Href { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("images")]
    public Image[]? Images { get; set; }
    [JsonPropertyName("product")]
    public string? Product { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }    
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}

