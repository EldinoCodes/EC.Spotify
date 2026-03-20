
namespace EC.Spotify.Extensions;

internal static class ByteArrayExtensions
{
    public static double? GetSize(this byte[]? data)
    {
        if (data is null) return 0;

        return (double)data.Length / 1024;
    }
}
