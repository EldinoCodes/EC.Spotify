using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Models.Auth;
using EC.Spotify.Providers;
using EC.Spotify.Tests.Core.Providers;
using System.Text.Json;

namespace EC.Spotify.Tests.Providers;

[TestClass]
public sealed class SpotifyJsonProviderTests
{
    #region Serialize

    [TestMethod]
    public void Serialize_NullObject_ReturnsNull()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var result = sut.Serialize<AuthToken>(null);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Serialize_StringValue_ReturnsSameString()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var expected = "hello world";
        var result = sut.Serialize(expected);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Serialize_ValidObject_ReturnsJsonString()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var token = new AuthToken { AccessToken = "test_token", TokenType = "Bearer", ExpiresIn = 3600 };
        var result = sut.Serialize(token);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("test_token"));
        Assert.IsTrue(result.Contains("Bearer"));
    }

    [TestMethod]
    public void Serialize_ValidObject_ProducesRoundTrippableJson()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var token = new AuthToken { AccessToken = "abc", TokenType = "Bearer", ExpiresIn = 3600 };
        var json = sut.Serialize(token);
        DummyProvider.AddOneTimeDummy<AuthToken>(null);
        var deserialized = sut.Deserialize<AuthToken>(json);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(token.AccessToken, deserialized.AccessToken);
        Assert.AreEqual(token.TokenType, deserialized.TokenType);
        Assert.AreEqual(token.ExpiresIn, deserialized.ExpiresIn);
    }

    #endregion

    #region Deserialize

    [TestMethod]
    public void Deserialize_NullJson_ReturnsDefault()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        DummyProvider.AddOneTimeDummy<AuthToken>(null);
        var result = sut.Deserialize<AuthToken>(null);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Deserialize_EmptyJson_ReturnsDefault()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        DummyProvider.AddOneTimeDummy<AuthToken>(null);
        var result = sut.Deserialize<AuthToken>(string.Empty);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Deserialize_ValidJson_ReturnsObject()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"access_token":"xyz","token_type":"Bearer","expires_in":3600}""";

        DummyProvider.AddOneTimeDummy<AuthToken>(null);
        var result = sut.Deserialize<AuthToken>(json);

        Assert.IsNotNull(result);
        Assert.AreEqual("xyz", result.AccessToken);
        Assert.AreEqual("Bearer", result.TokenType);
        Assert.AreEqual(3600, result.ExpiresIn);
    }

    [TestMethod]
    public void Deserialize_InvalidJson_ReturnsDefault()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        DummyProvider.AddOneTimeDummy<AuthToken>(null);
        var result = sut.Deserialize<AuthToken>("{not valid json!!}");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Deserialize_StringType_ReturnsRawString()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var input = """{"id":"123"}""";

        DummyProvider.AddOneTimeDummy(input);
        var result = sut.Deserialize<string>(input);

        Assert.AreEqual(input, result);
    }

    #endregion

    #region ProcessSpotifyJson

    [TestMethod]
    public void ProcessSpotifyJson_NullJson_ReturnsNull()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var result = sut.ProcessSpotifyJson(null);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void ProcessSpotifyJson_NonJsonString_ReturnsSameString()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var input = "not json at all";
        var result = sut.ProcessSpotifyJson(input);

        Assert.AreEqual(input, result);
    }

    [TestMethod]
    public void ProcessSpotifyJson_WhitespacePaddedJson_ReturnsProcessedJson()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var result = sut.ProcessSpotifyJson("""   {"id":"123"}   """);

        Assert.IsNotNull(result);
        var doc = JsonDocument.Parse(result);
        Assert.AreEqual("123", doc.RootElement.GetProperty("id").GetString());
    }

    [TestMethod]
    public void ProcessSpotifyJson_BooleanJson_ReturnsBoolean()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        Assert.AreEqual("true", sut.ProcessSpotifyJson("true"));
        Assert.AreEqual("false", sut.ProcessSpotifyJson("false"));
    }

    [TestMethod]
    public void ProcessSpotifyJson_ValidJsonNoPath_ReturnsProcessedJson()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"id":"123","name":"Test Album"}""";
        var result = sut.ProcessSpotifyJson(json);

        Assert.IsNotNull(result);
        var doc = JsonDocument.Parse(result);
        Assert.AreEqual("123", doc.RootElement.GetProperty("id").GetString());
        Assert.AreEqual("Test Album", doc.RootElement.GetProperty("name").GetString());
    }

    [TestMethod]
    public void ProcessSpotifyJson_JsonWithPropertyPath_ReturnsValueAtPath()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"limit":20,"offset":0}""";
        var result = sut.ProcessSpotifyJson(json, "limit");

        Assert.AreEqual("20", result);
    }

    [TestMethod]
    public void ProcessSpotifyJson_JsonWithNestedPath_ReturnsNestedValue()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"external_urls":{"spotify":"https://open.spotify.com/album/123"}}""";
        var result = sut.ProcessSpotifyJson(json, "external_urls.spotify");

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("open.spotify.com"));
    }

    [TestMethod]
    public void ProcessSpotifyJson_JsonArrayWithIndex_ReturnsElementAtIndex()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """[{"id":"a"},{"id":"b"},{"id":"c"}]""";
        var result = sut.ProcessSpotifyJson(json, "[1]");

        Assert.IsNotNull(result);
        var doc = JsonDocument.Parse(result);
        Assert.AreEqual("b", doc.RootElement.GetProperty("id").GetString());
    }

    [TestMethod]
    public void ProcessSpotifyJson_PathNotFound_ReturnsNull()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"id":"123"}""";
        var result = sut.ProcessSpotifyJson(json, "missing_property");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void ProcessSpotifyJson_PolymorphicTypeProperty_MovedToFront()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"id":"123","name":"Test","type":"album"}""";
        var result = sut.ProcessSpotifyJson(json);

        Assert.IsNotNull(result);
        var first = JsonDocument.Parse(result).RootElement.EnumerateObject().First();
        Assert.AreEqual("type", first.Name);
        Assert.AreEqual("album", first.Value.GetString());
    }

    [TestMethod]
    public void ProcessSpotifyJson_NestedPolymorphicTypeProperty_MovedToFront()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"items":[{"id":"1","name":"A","type":"track"},{"id":"2","name":"B","type":"episode"}]}""";
        var result = sut.ProcessSpotifyJson(json);

        Assert.IsNotNull(result);
        foreach (var element in JsonDocument.Parse(result).RootElement.GetProperty("items").EnumerateArray())
        {
            var first = element.EnumerateObject().First();
            Assert.AreEqual("type", first.Name);
        }
    }

    [TestMethod]
    public void ProcessSpotifyJson_JsonWithoutTypeProperty_ReturnsAllProperties()
    {
        var sut = Initializer.Resolve<ISpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut);

        var json = """{"id":"abc","name":"No Type Here"}""";
        var result = sut.ProcessSpotifyJson(json);

        Assert.IsNotNull(result);
        var doc = JsonDocument.Parse(result);
        Assert.AreEqual("abc", doc.RootElement.GetProperty("id").GetString());
        Assert.AreEqual("No Type Here", doc.RootElement.GetProperty("name").GetString());
    }

    #endregion
}
