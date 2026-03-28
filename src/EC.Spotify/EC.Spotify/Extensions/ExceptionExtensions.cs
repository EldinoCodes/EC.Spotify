using EC.Spotify.Models;

namespace EC.Spotify.Extensions;

internal static class ExceptionExtensions
{
    public static SpotifyError ToSpotifyError(this Exception ex)
    {
        return new SpotifyError()
        {
            Status = 500,
            Message = $"EC.Spotify - {ex.Message}",
            Reason = ex.GetType().Name
        };
    }
}
