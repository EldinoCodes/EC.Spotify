using System.Text.Json.Serialization;

namespace EC.Spotify.Api.Models;

public class Album
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
}
