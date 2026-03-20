namespace EC.Spotify.Extensions;

internal static class StringExtensions
{
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

    public static string? ToUri(this string? uriString, Dictionary<string, string?>? query = default)
    {
        if (string.IsNullOrEmpty(uriString)) return default;
        if (!Uri.TryCreate(uriString, UriKind.Absolute, out var uri)) return default;
        if (query is null || query.Count == 0) return uri.ToString();

        return new UriBuilder(uri)
        {
            Query = string.Join("&", query.Select(i => $"{i.Key}={i.Value}"))
        }.ToString();
    }
}
