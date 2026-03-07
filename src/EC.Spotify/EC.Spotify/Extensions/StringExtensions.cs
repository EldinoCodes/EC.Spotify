
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
}
