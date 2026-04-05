using EC.Spotify.Abstractions.Models;
using EC.Spotify.Abstractions.Providers;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace EC.Spotify.Providers;

internal class SpotifyJsonProvider(ILogger<SpotifyJsonProvider> logger) : ISpotifyJsonProvider
{
    private readonly ILogger<SpotifyJsonProvider> _logger = logger;

    private static readonly JsonSerializerOptions _jssOptions = new() { PropertyNameCaseInsensitive = true };
    private static readonly string[] jsonBools = ["true", "false"];
    private static readonly string[] jsonElementStart = ["{", "["];
    private static readonly string[] jsonElementEnd = ["}", "]"];
    private static readonly JsonValueKind[] jsonValueKinds = [JsonValueKind.Array, JsonValueKind.Object];

    private static readonly string? _polymorphicPropertyName = Attribute
        .GetCustomAttributes(typeof(IPolymorphicItem), typeof(JsonPolymorphicAttribute))
        .Cast<JsonPolymorphicAttribute>()
        .FirstOrDefault()
        ?.TypeDiscriminatorPropertyName;

    public string? Serialize<T>(T? obj)
    {
        if (obj is null) return default;
        if (obj is string ret) return ret;

        string? json = default;
        try
        {
            json = JsonSerializer.Serialize(obj, _jssOptions);
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug(ex, "Failed to serialize object. Type: {type}", typeof(T).Name);
        }
        return json;
    }
    public T? Deserialize<T>(string? json)
    {
        T? ret = default;
        if (string.IsNullOrEmpty(json)) return ret;

        var type = typeof(T?);
        try
        {
            ret = json.GetType() == type
                ? (T?)Convert.ChangeType(json, type)
                : JsonSerializer.Deserialize<T?>(json, _jssOptions);
        }
        catch (Exception ex)
        {
            if(_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug(ex, "Failed to deserialize JSON. JSON: {Json}", json);
        }
        return ret;
    }

    public string? ProcessSpotifyJson(string? json, string? jsonPath = default)
    {
        if (json is null) return default;

        json = json.Trim();

        if (!IsJson(json)) return json;

        var jsonNode = JsonNode.Parse(json);
        jsonNode = RecurseJson(jsonNode, jsonPath);

        MovePolymorphicTypeProperty(jsonNode);

        return jsonNode?.ToJsonString();
    }    
    private static bool IsJson(string json) =>
        (
            jsonElementStart.Any(json.StartsWith)
            && jsonElementEnd.Any(json.EndsWith)
        )
        || jsonBools.Any(json.Equals);
    private static JsonNode? RecurseJson(JsonNode? node, string? jsonPath = default)
    {
        if (node is null) return default;

        var steps = (jsonPath ?? "")
            .Replace("[", ".").Replace("]", ".")
            .Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];

        foreach (var step in steps)
        {
            if (node is null) continue;
            node = node is JsonArray arr && int.TryParse(step, out int idx)
                ? arr.ElementAtOrDefault(idx)
                : node is JsonObject obj
                    ? node[step]
                    : null;
        }
        return node;
    }
    private static void MovePolymorphicTypeProperty(JsonNode? node)
    {
        if (node is null) return;
        if (string.IsNullOrEmpty(_polymorphicPropertyName)) return;
        if (!jsonValueKinds.Any(k => k == node.GetValueKind())) return;

        if (node is JsonObject jsonObject)
        {
            var discriminator = jsonObject[_polymorphicPropertyName];
            if (discriminator != null)
            {
                jsonObject.Remove(_polymorphicPropertyName);
                jsonObject.Insert(0, _polymorphicPropertyName, discriminator);
            }

            foreach (var property in jsonObject)
            {
                if (property.Value is null) continue;

                MovePolymorphicTypeProperty(property.Value);
            }
        }
        else if (node is JsonArray jsonArray)
        {
            foreach (var element in jsonArray)
                MovePolymorphicTypeProperty(element);
        }
    }
}