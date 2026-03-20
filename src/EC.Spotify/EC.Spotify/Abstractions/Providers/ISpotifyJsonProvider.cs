namespace EC.Spotify.Abstractions.Providers;

public interface ISpotifyJsonProvider
{
    /// <summary>
    /// Deserializes a JSON string into an object of the specified type.
    /// </summary>
    /// <remarks>Ensure that the JSON string is well-formed and matches the structure of the target type T for
    /// successful deserialization.</remarks>
    /// <typeparam name="T">The type of the object to deserialize the JSON string into.</typeparam>
    /// <param name="jsonString">The JSON string to deserialize. This parameter cannot be null or empty.</param>
    /// <param name="jsonPath">An optional JSON path that specifies a subset of the JSON to deserialize. If null, the entire JSON string is
    /// deserialized.</param>
    /// <returns>An instance of type T populated with data from the JSON string, or null if deserialization fails.</returns>
    T? Deserialize<T>(string? jsonString, string? jsonPath = null);
    /// <summary>
    /// Serializes the specified object to its JSON string representation.
    /// </summary>
    /// <remarks>Ensure that the object is serializable; otherwise, an exception may be thrown during
    /// serialization. This method uses a JSON serialization library to convert the object to a string format.</remarks>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize. If null, the method returns null.</param>
    /// <returns>A JSON string that represents the serialized object, or null if the input object is null.</returns>
    string? Serialize<T>(T? obj);
}