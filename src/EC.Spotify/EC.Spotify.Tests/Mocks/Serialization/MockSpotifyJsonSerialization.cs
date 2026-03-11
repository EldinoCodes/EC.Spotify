using EC.Spotify.Abstractions.Models;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Tests.Core.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EC.Spotify.Tests.Mocks.Serialization;

internal class MockSpotifyJsonSerialization : ISpotifyJsonSerializer
{
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
        => (T?)ReflectionProvider.PopulateObjectRecursive(typeof(T));

    public List<string?> GetPolymorphicTypeNames() => _polymorphicTypeNames;
}
