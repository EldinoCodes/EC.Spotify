using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EC.Spotify.Serialization;

internal sealed class SpotifyJsonSerializer : ISpotifyJsonSerializer
{
    private readonly ILogger<SpotifyJsonSerializer> _logger;
    private readonly JsonSerializerOptions _jssOptions;

    internal readonly string[] jsonBools = ["true", "false"];
    internal readonly string[] jsonElementStart = ["{", "["];
    internal readonly string[] jsonElementEnd = ["}", "]"];

    private readonly Dictionary<string, Type> spotifyTypeMapping = new()
    {
        { "track", typeof(Track) },
        { "episode",typeof(Episode) }
    };

    public SpotifyJsonSerializer(ILogger<SpotifyJsonSerializer> logger)
    {
        _logger = logger;

        _jssOptions = new() { PropertyNameCaseInsensitive = true };
        _jssOptions.Converters.Add(new SpotifyJsonTypeConverter<object?>(
            (ref Utf8JsonReader reader) =>
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                var typeDiscriminator = doc.RootElement.GetProperty("type").GetString();

                if (!spotifyTypeMapping.TryGetValue(typeDiscriminator ?? string.Empty, out var type)) throw new JsonException();

                return type;
            }
        ));
    }

    public T? Deserialize<T>(string? json, string? jsonPath = default)
    {
        T? ret = default;        
        json = json?.Trim();
        if (string.IsNullOrEmpty(json)) return ret;

        
        if (
            !jsonElementStart.Any(json.StartsWith) 
            && !jsonElementEnd.Any(json.EndsWith) 
            && !jsonBools.Any(json.Equals)
        ) return ret;

        var type = typeof(T?);
        try
        {
            var node = JsonNode.Parse(json);
            foreach (var path in jsonPath?.Split('.', StringSplitOptions.RemoveEmptyEntries) ?? [])
            {
                if (string.IsNullOrEmpty(path)) continue;
                if (node is null) continue;

                node = node[path];
            }
            json = node?.ToJsonString() ?? "";
            if (json.GetType() == type) return (T?)Convert.ChangeType(json, type);

            ret = JsonSerializer.Deserialize<T?>(json, _jssOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize JSON. JSON: {Json}, JSON Path: {JsonPath}", json, jsonPath);
        }
        return ret;
    }
    public string? Serialize<T>(T? obj)
    {
        if (obj is null) return default;
        if (obj is string ret) return ret;

        return JsonSerializer.Serialize(obj, _jssOptions);
    }
}
