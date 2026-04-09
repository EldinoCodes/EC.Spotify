using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Models;

namespace EC.Spotify.UnitTests.Mocks;

internal sealed class MockSpotifyProvider : ISpotifyProvider
{
    private readonly Queue<object?> _resultQueue = new();
    private string? _rawResult;
    private Exception? _exception;

    public void Enqueue<T>(SpotifyResult<T> result) => _resultQueue.Enqueue(result);

    public void SetRawResult(string? result) => _rawResult = result;

    public void SetException(Exception ex) => _exception = ex;

    public Task<SpotifyResult<T>> ExecuteSpotifyResultAsync<T>(
        string? method,
        string? uri,
        HttpContent? httpContent = default,
        List<string?>? jsonPaths = default,
        CancellationToken cancellationToken = default)
    {
        if (_exception is not null)
            throw _exception;

        if (_resultQueue.TryDequeue(out var item) && item is SpotifyResult<T> typed)
            return Task.FromResult(typed);

        return Task.FromResult(new SpotifyResult<T>());
    }

    public Task<string?> ExecuteSpotifyRequestAsync(
        string? method,
        string? uri,
        HttpContent? httpContent = default,
        CancellationToken cancellationToken = default)
    {
        if (_exception is not null)
            throw _exception;

        return Task.FromResult(_rawResult);
    }
}
