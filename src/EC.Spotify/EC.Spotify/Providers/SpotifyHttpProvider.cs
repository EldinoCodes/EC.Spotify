using EC.Spotify.Abstractions.Providers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace EC.Spotify.Providers;

internal class SpotifyHttpProvider(ILogger<SpotifyHttpProvider> logger, HttpClient httpClient) : ISpotifyHttpProvider
{
    private readonly ILogger<SpotifyHttpProvider> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(method)) return default;
        if (string.IsNullOrEmpty(uri)) return default;

        using var request = new HttpRequestMessage(new HttpMethod(method.ToUpperInvariant()), uri);

        if (httpContent is not null)
            request.Content = httpContent;

        configureHttpHeaders?.Invoke(request.Headers);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}