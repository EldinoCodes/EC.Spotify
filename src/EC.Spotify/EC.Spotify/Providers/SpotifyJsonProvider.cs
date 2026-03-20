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

    // i could hardcode these, but this way it will be more flexible and reusable for other polymorphic types in the future
    private static readonly string? _polymorphicPropertyName = Attribute
        .GetCustomAttributes(typeof(IPolymorphicItem), typeof(JsonPolymorphicAttribute))
        .Cast<JsonPolymorphicAttribute>()
        .FirstOrDefault()
        ?.TypeDiscriminatorPropertyName;
    private static readonly List<string?> _polymorphicTypeNames = Attribute
        .GetCustomAttributes(typeof(IPolymorphicItem), typeof(JsonDerivedTypeAttribute))
        .Select(a => ((JsonDerivedTypeAttribute)a)?.TypeDiscriminator?.ToString())?.ToList() ?? [];

    public string? Serialize<T>(T? obj)
    {
        if (obj is null) return default;
        if (obj is string ret) return ret;

        return JsonSerializer.Serialize(obj, _jssOptions);
    }
    public T? Deserialize<T>(string? jsonString, string? jsonPath = default)
    {
        T? ret = default;
        var json = ProcessSpotifyJson(jsonString, jsonPath);
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
                _logger.LogDebug(ex, "Failed to deserialize JSON. JSON: {Json}, JSON Path: {JsonPath}", json, jsonPath);
        }
        return ret;
    }

    private string? ProcessSpotifyJson(string? json, string? jsonPath = default)
    {
        if (json is null) return default;

        json = json.Trim();

        if (!IsJson(json)) return json;

        var jsonNode = JsonNode.Parse(json);
        jsonNode = RecurseJson(jsonNode, jsonPath);

        AddPolymorphicTypeDiscriminatorProperty(jsonNode);

        return jsonNode?.ToJsonString();
    }
    private void AddPolymorphicTypeDiscriminatorProperty(JsonNode? node)
    {
        if (node is null) return;
        if (string.IsNullOrEmpty(_polymorphicPropertyName)) return;
        if (!jsonValueKinds.Any(k => k == node.GetValueKind())) return;

        if (node is JsonObject jsonObject)
        {
            var hasDiscriminator = jsonObject.TryGetPropertyValue(_polymorphicPropertyName, out _);
            var hasNodeType = jsonObject.TryGetPropertyValue("type", out var nodeType);
            var polymorphicName = _polymorphicTypeNames.FirstOrDefault(i => nodeType?.ToString()?.Equals(i, StringComparison.InvariantCultureIgnoreCase) ?? false);

            foreach (var property in jsonObject)
            {
                if (property.Value is null) continue;

                AddPolymorphicTypeDiscriminatorProperty(property.Value);
            }
            if (hasDiscriminator) return;
            if (!hasNodeType) return;

            if (!string.IsNullOrEmpty(polymorphicName))
            {
                jsonObject.Insert(0, _polymorphicPropertyName, polymorphicName);
            } else if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Could not find a matching polymorphic type name for node with type '{NodeType}'", nodeType);
        }
        else if (node is JsonArray jsonArray)
        {
            foreach (var element in jsonArray)
                AddPolymorphicTypeDiscriminatorProperty(element);
        }
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
}