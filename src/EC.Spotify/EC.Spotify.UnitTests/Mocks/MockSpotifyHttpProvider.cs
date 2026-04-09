using EC.Spotify.Abstractions.Providers;
using System.Net.Http.Headers;

namespace EC.Spotify.UnitTests.Mocks;

internal sealed class MockSpotifyHttpProvider : ISpotifyHttpProvider
{
    private string? _response;
    private Exception? _exception;

    public void SetResponse(string? response) => _response = response;

    public void SetException(Exception ex) => _exception = ex;

    public Task<string?> ExecuteAsync(
        string? method,
        string? uri,
        HttpContent? httpContent = null,
        Action<HttpRequestHeaders>? configureHttpHeaders = null,
        CancellationToken cancellationToken = default)
    {
        if (_exception is not null)
            throw _exception;

        return Task.FromResult(_response);
    }
}
