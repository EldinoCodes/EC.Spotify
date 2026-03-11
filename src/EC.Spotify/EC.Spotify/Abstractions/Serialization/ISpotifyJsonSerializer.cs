namespace EC.Spotify.Abstractions.Serialization;

public interface ISpotifyJsonSerializer
{
    T? Deserialize<T>(string? json, string? jsonPath = null);
    string? Serialize<T>(T? obj);

    List<string?> GetPolymorphicTypeNames();
}