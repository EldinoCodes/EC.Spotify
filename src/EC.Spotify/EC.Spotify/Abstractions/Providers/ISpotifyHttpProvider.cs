using System.Net.Http.Headers;

namespace EC.Spotify.Abstractions.Providers;

internal interface ISpotifyHttpProvider
{
    Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, CancellationToken cancellationToken = default);
}
