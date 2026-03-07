using System.Text.Json;
using System.Text.Json.Serialization;

namespace EC.Spotify.Serialization;

internal sealed class SpotifyJsonTypeConverter<T>(SpotifyJsonTypeConverter<T>.JsonTypeConverter converter) : JsonConverter<T>
{
    public delegate Type JsonTypeConverter(ref Utf8JsonReader reader);
    private readonly JsonTypeConverter Converter = converter;

    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeCalculatorReader = reader;
        var actualType = Converter(ref typeCalculatorReader);

        return (T?)JsonSerializer.Deserialize(ref reader, actualType, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null) return;
        writer.WriteRawValue(JsonSerializer.Serialize(value, value.GetType(), options));
    }
}