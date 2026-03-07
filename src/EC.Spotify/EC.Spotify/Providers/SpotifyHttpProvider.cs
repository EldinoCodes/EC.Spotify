using EC.Spotify.Abstractions.Providers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace EC.Spotify.Providers;

internal sealed class SpotifyHttpProvider(ILogger<SpotifyHttpProvider> logger, HttpClient httpClient) : ISpotifyHttpProvider
{
    private readonly ILogger<SpotifyHttpProvider> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(method)) return default;
        if (string.IsNullOrEmpty(uri)) return default;

        if (configureHttpHeaders != null)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            configureHttpHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
        }

        var response = method.ToLower() switch
        {
            "post" => await _httpClient.PostAsync(uri, httpContent, cancellationToken),            
            "put" => await _httpClient.PutAsync(uri, httpContent, cancellationToken),
            "get" => await _httpClient.GetAsync(uri, cancellationToken),
            _ => throw new NotImplementedException()
        };
        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        return result;
    }
}