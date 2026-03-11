using EC.Spotify.Abstractions.Models;
using EC.Spotify.Abstractions.Serialization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace EC.Spotify.Serialization;

internal sealed class SpotifyJsonSerializer(ILogger<SpotifyJsonSerializer> logger) : ISpotifyJsonSerializer
{
    private readonly ILogger<SpotifyJsonSerializer> _logger = logger;
    private readonly JsonSerializerOptions _jssOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly List<string?> _polymorphicTypeNames = Attribute
        .GetCustomAttributes(typeof(IPolymorphicItem), typeof(JsonDerivedTypeAttribute))
        .Select(a => ((JsonDerivedTypeAttribute)a)?.TypeDiscriminator?.ToString())?.ToList() ?? [];

    internal readonly string[] jsonBools = ["true", "false"];
    internal readonly string[] jsonElementStart = ["{", "["];
    internal readonly string[] jsonElementEnd = ["}", "]"];
    internal readonly JsonValueKind[] jsonValueKinds = [JsonValueKind.Array, JsonValueKind.Object];

    public string? Serialize<T>(T? obj)
    {
        if (obj is null) return default;
        if (obj is string ret) return ret;

        return JsonSerializer.Serialize(obj, _jssOptions);
    }
    public T? Deserialize<T>(string? json, string? jsonPath = default)
    {
        T? ret = default;
        json = ProcessJson(json, jsonPath);
        if (string.IsNullOrEmpty(json)) return ret;

        var type = typeof(T?);
        try 
        {
            ret = json.GetType() == type
                ? (T?)Convert.ChangeType(json, type)
                : JsonSerializer.Deserialize<T?>(json, _jssOptions);
        }
        catch (Exception)
        {
            Debug.WriteLine($"Failed to deserialize JSON. JSON: {json}, JSON Path: {jsonPath}");
        }
        return ret;
    }

    public List<string?> GetPolymorphicTypeNames() => _polymorphicTypeNames;

    private string? ProcessJson(string? json, string? jsonPath = default)
    {
        if (json is null) return default;
        if (
            !jsonElementStart.Any(json.StartsWith) 
            && !jsonElementEnd.Any(json.EndsWith) 
            && !jsonBools.Any(json.Equals)
        ) return json;

        var node = JsonNode.Parse(json.Trim());
        var steps = (jsonPath ?? "")
            .Replace("[", ".").Replace("]", ".")
            .Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
        foreach (var step in steps)
        {
            if (node is null) continue;
            node = node is JsonArray arr && int.TryParse(step, out int idx)
                ? arr.ElementAtOrDefault(idx)
                : node[step];
        }

        AddPolymorphicTypeDiscriminatorProperty(node);

        return node?.ToJsonString();
    }
    private void AddPolymorphicTypeDiscriminatorProperty(JsonNode? node)
    {
        if (node is null) return;        
        if (!jsonValueKinds.Any(k => k == node.GetValueKind())) return;

        if (node is JsonObject jsonObject)
        {
            string? type = null;
            foreach (var property in jsonObject)
            {
                if (property.Value is null) continue;

                if (property.Key.Equals("type", StringComparison.InvariantCultureIgnoreCase)) 
                {
                    var propertyValue = property.Value?.ToString();
                    if(_polymorphicTypeNames.Any(i => propertyValue?.Equals(i, StringComparison.InvariantCultureIgnoreCase) ?? false)) type = propertyValue;
                }
                AddPolymorphicTypeDiscriminatorProperty(property.Value);
            }
            if (!string.IsNullOrEmpty(type)) jsonObject.Insert(0, "$type", type);
        }
        else if (node is JsonArray jsonArray)
        {
            foreach (var element in jsonArray)
                AddPolymorphicTypeDiscriminatorProperty(element);
        }
    }
}
