using System.Text.Json;

namespace EC.Spotify.Api;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions jssOptions = new() { PropertyNameCaseInsensitive = true };
    internal static readonly string[] jsonElementStart = ["{", "["];
    internal static readonly string[] jsonElementEnd = ["}", "]"];

    public static T? FromJson<T>(this string? json)
    
    {
        T? ret = default;
        if (string.IsNullOrEmpty(json)) return ret;

        json = json.Trim();
        if (
            jsonElementStart.Any(json.StartsWith)
            && jsonElementEnd.Any(json.EndsWith)
        )
            ret = JsonSerializer.Deserialize<T?>(json, jssOptions);

        return ret;
    }

    public static string? EncodeBase64(this string? content)
    {
        if (string.IsNullOrEmpty(content)) return content;
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        return Convert.ToBase64String(bytes);
    }
    public static string? DecodeBase64(this string? content)
    {
        if (string.IsNullOrEmpty(content)) return content;
        var bytes = Convert.FromBase64String(content);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
