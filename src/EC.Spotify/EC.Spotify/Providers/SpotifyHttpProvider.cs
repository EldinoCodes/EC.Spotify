using EC.Spotify.Abstractions.Providers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace EC.Spotify.Providers;

internal class SpotifyHttpProvider(ILogger<SpotifyHttpProvider> logger, IHttpClientFactory httpClientFactory) : ISpotifyHttpProvider
{
    private readonly ILogger<SpotifyHttpProvider> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(method)) return default;
        if (string.IsNullOrEmpty(uri)) return default;

        using var httpClient = _httpClientFactory.CreateClient(nameof(SpotifyHttpProvider));
        using var request = new HttpRequestMessage(new HttpMethod(method.ToUpperInvariant()), uri);

        if (httpContent is not null)
            request.Content = httpContent;

        configureHttpHeaders?.Invoke(request.Headers);

        var response = await httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            _logger.LogWarning("Received HTTP {StatusCode} from {Uri}", (int)response.StatusCode, uri);

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}