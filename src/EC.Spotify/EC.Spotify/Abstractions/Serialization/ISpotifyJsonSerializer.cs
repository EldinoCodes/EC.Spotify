namespace EC.Spotify.Abstractions.Serialization;

internal interface ISpotifyJsonSerializer
{
    T? Deserialize<T>(string? json, string? jsonPath = null);
    string? Serialize<T>(T? obj);

    List<string?> GetTypeDiscriminatorNames();
}