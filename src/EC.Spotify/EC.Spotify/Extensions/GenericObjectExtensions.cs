using System.Text.Json;
using System.Text.Json.Serialization;

namespace EC.Spotify.Extensions;

internal static class GenericObjectExtensions
{
    private readonly static JsonSerializerOptions _jssOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public static string? ToJson<T>(this T? obj)
    {
        if (obj is null) return default;
        if (obj is string ret) return ret;

        return JsonSerializer.Serialize(obj, _jssOptions);
    }
}
