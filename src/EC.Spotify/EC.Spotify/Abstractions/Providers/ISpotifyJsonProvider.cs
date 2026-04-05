namespace EC.Spotify.Abstractions.Providers;

public interface ISpotifyJsonProvider
{
    /// <summary>
    /// Deserializes the specified JSON string to an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize. Can be null or empty.</param>
    /// <returns>An instance of type T deserialized from the JSON string, or null if the input is null or empty.</returns>
    T? Deserialize<T>(string? json);
    /// <summary>
    /// Serializes the specified object to its JSON string representation.
    /// </summary>
    /// <remarks>Ensure that the object is serializable; otherwise, an exception may be thrown during
    /// serialization. This method uses a JSON serialization library to convert the object to a string format.</remarks>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize. If null, the method returns null.</param>
    /// <returns>A JSON string that represents the serialized object, or null if the input object is null.</returns>
    string? Serialize<T>(T? obj);
    /// <summary>
    /// Extracts a value from a Spotify JSON payload using the specified JSONPath expression.  Moves type discriminator property to the front of the JSON objects to better support .NET polymorphic handling.
    /// </summary>
    /// <param name="json">The JSON string representing the Spotify data to process. Can be null.</param>
    /// <param name="jsonPath">The JSONPath expression used to select a value from the JSON. If null or empty, the method may return the entire
    /// JSON or null.  Example: "image.height"  </param>
    /// <returns>A string containing the value extracted from the JSON at the specified path, or null if the path is not found or the
    /// input is invalid.</returns>
    string? ProcessSpotifyJson(string? json, string? jsonPath = default);
}