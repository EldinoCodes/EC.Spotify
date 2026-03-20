using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace EC.Spotify.Tests.Mocks.Providers;


/// <summary>
/// Provides a mock implementation of the ISpotifyHttpProvider interface for testing purposes.
/// </summary>
/// <remarks>This class simulates HTTP interactions with the Spotify API, allowing for controlled testing without
/// actual network calls. It is designed to return predefined results, making it useful for unit tests and scenarios
/// where real API calls are not feasible.</remarks>
internal class MockSpotifyHttpProvider(ILogger<SpotifyHttpProvider> logger, IConfiguration configuration, HttpClient httpClient) : SpotifyHttpProvider(logger, httpClient), ISpotifyHttpProvider
{
    private readonly IConfiguration _configuration = configuration;
    public async new Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, CancellationToken cancellationToken = default)
        => _configuration.GetValue<bool>("FullEnd2EndTest")
            ? await base.ExecuteAsync(method, uri, httpContent, configureHttpHeaders, cancellationToken)
            : await Task.FromResult(string.Empty);
}
