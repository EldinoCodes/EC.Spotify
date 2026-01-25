using System.Text.Json;
using System.Text.Json.Serialization;

namespace EC.Spotify.Api;

public static class GenericObjectExtensions
{
    private readonly static JsonSerializerOptions jssOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public static string? ToJson<T>(this T? obj)
    {
        if (obj is null) return default;
        if (obj is string ret) return ret;

        return JsonSerializer.Serialize(obj, jssOptions);
    }
}
