using EC.Spotify.Abstractions.Providers;

namespace EC.Spotify.UnitTests.Mocks;

internal sealed class MockSpotifyJsonProvider : ISpotifyJsonProvider
{
    public T? Deserialize<T>(string? json) => default;

    public string? Serialize<T>(T? obj) => null;

    public string? ProcessSpotifyJson(string? json, string? jsonPath = default) => null;
}
