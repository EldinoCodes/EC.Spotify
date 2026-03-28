using EC.Spotify.Models;

namespace EC.Spotify.Extensions;

internal static class SpotifyOptionExtensions
{
    public static SpotifyError? ValidateScopes(this SpotifyOptions? options, List<string?>? scopes)
    {
        if (options is null) return default;
        if (scopes is null) return default;

        var missingScopes = scopes.Except(options.Scopes ?? []).ToList();
        if (missingScopes.Count == 0) return default;

        return new SpotifyError
        {
            Status = 401,
            Message = $"Missing required scopes: {string.Join(", ", missingScopes)}"
        };
    }
}
