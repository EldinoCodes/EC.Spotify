using System.Net.Http.Headers;

namespace EC.Spotify.Api.Providers;

public interface IHttpProvider
{
    Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, bool throwException = false, CancellationToken cancellationToken = default);
}

internal class HttpProvider(HttpClient httpClient) : IHttpProvider
{
    protected readonly HttpClient _httpClient = httpClient;

    public virtual async Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, bool throwException = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(method)) return default;
        if (string.IsNullOrEmpty(uri)) return default;

        // allow override
        if (configureHttpHeaders != null)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            configureHttpHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
        }

        var response = method.ToLower() switch
        {
            "post" => await _httpClient.PostAsync(uri, httpContent, cancellationToken),
            "get" => await _httpClient.GetAsync(uri, cancellationToken),
            "put" => await _httpClient.PutAsync(uri, httpContent, cancellationToken),
            _ => throw new NotImplementedException()
        };
        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        if (throwException && !response.IsSuccessStatusCode)
            throw new Exception(result);

        return result;
    }
}