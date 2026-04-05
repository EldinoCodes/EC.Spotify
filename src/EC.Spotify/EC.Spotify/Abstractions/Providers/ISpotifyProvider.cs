using EC.Spotify.Models;

namespace EC.Spotify.Abstractions.Providers;

internal interface ISpotifyProvider
{
    Task<SpotifyResult<T>> ExecuteSpotifyResultAsync<T>(string? method, string? uri, HttpContent? httpContent = default, List<string?>? jsonPaths = default, CancellationToken cancellationToken = default);
    Task<string?> ExecuteSpotifyRequestAsync(string? method, string? uri, HttpContent? httpContent = default, CancellationToken cancellationToken = default);
}