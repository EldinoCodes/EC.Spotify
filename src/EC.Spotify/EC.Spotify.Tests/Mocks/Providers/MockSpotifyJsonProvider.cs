using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Providers;
using EC.Spotify.Tests.Core.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Tests.Mocks.Providers;

/// <summary>
/// Provides a mock implementation of the SpotifyJsonProvider for use in unit tests.
/// </summary>
/// <remarks>This class is intended for testing scenarios where a real ISpotifyJsonProvider is not required. It
/// overrides deserialization to return dummy objects, allowing tests to run without actual JSON processing.</remarks>
/// <param name="logger">The logger used to record information and errors during JSON deserialization.</param>
internal class MockSpotifyJsonProvider(ILogger<SpotifyJsonProvider> logger, IConfiguration configuration) : SpotifyJsonProvider(logger), ISpotifyJsonProvider
{
    private IConfiguration _configuration = configuration;
    public new T? Deserialize<T>(string? jsonString) 
        => _configuration.GetValue<bool>("FullEnd2EndTest") 
            ? base.Deserialize<T>(jsonString)
            : DummyProvider.DummyObject<T?>() ?? base.Deserialize<T>(jsonString);
}
