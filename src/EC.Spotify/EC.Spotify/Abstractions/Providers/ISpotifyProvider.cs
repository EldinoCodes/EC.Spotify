using EC.Spotify.Models;

namespace EC.Spotify.Abstractions.Providers;

internal interface ISpotifyProvider
{
    Task<SpotifyResult<T>> ExecuteSpotifyResultAsync<T>(string? method, string? uri, HttpContent? httpContent = null, List<string?>? jsonPaths = null, CancellationToken cancellationToken = default);
}