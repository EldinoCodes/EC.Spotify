using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models.Auth;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;
using Microsoft.Extensions.Caching.Memory;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class AuthorizationServiceTests
{
    private IMemoryCache _cache = null!;
    private MockSpotifyHttpProvider _httpProvider = null!;
    private MockSpotifyJsonProvider _jsonProvider = null!;
    private IAuthorizationService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _httpProvider = new MockSpotifyHttpProvider();
        _jsonProvider = new MockSpotifyJsonProvider();
        _sut = ServiceFactory.CreateAuthorizationService(_httpProvider, _jsonProvider, _cache);
    }

    [TestCleanup]
    public void Cleanup() => _cache.Dispose();

    [TestMethod]
    public void AuthorizationCodeUrl_ReturnsNonEmptyUrl()
    {
        var url = _sut.AuthorizationCodeUrl();

        Assert.IsNotNull(url);
        Assert.Contains("accounts.spotify.com", url);
        Assert.Contains("authorize", url);
    }

    [TestMethod]
    public void AuthorizationCodeUrl_IncludesClientId()
    {
        var url = _sut.AuthorizationCodeUrl();

        Assert.IsNotNull(url);
        Assert.Contains("client_id=test-client-id", url);
    }

    [TestMethod]
    public async Task AuthorizationCodeAddAsync_WithValidCode_ReturnsTrue()
    {
        var added = await _sut.AuthorizationCodeAddAsync("valid-code");

        Assert.IsTrue(added);
    }

    [TestMethod]
    public async Task AuthorizationCodeAddAsync_WithNullCode_ReturnsFalse()
    {
        var added = await _sut.AuthorizationCodeAddAsync(null);

        Assert.IsFalse(added);
    }

    [TestMethod]
    public async Task AuthorizationCodeAddAsync_WithEmptyCode_ReturnsFalse()
    {
        var added = await _sut.AuthorizationCodeAddAsync(string.Empty);

        Assert.IsFalse(added);
    }

    [TestMethod]
    public async Task AuthorizationCodeGetAsync_WhenNoCodeAdded_ReturnsNull()
    {
        var code = await _sut.AuthorizationCodeGetAsync();

        Assert.IsNull(code);
    }

    [TestMethod]
    public async Task AuthorizationCodeGetAsync_WhenCodeAdded_ReturnsCode()
    {
        await _sut.AuthorizationCodeAddAsync("my-code");

        var code = await _sut.AuthorizationCodeGetAsync();

        Assert.AreEqual("my-code", code);
    }

    [TestMethod]
    public async Task AuthorizationCodeRemoveAsync_WhenCodeExists_ReturnsTrue()
    {
        await _sut.AuthorizationCodeAddAsync("my-code");

        var removed = await _sut.AuthorizationCodeRemoveAsync();

        Assert.IsTrue(removed);
    }

    [TestMethod]
    public async Task AuthorizationCodeRemoveAsync_AfterRemoval_CodeIsGone()
    {
        await _sut.AuthorizationCodeAddAsync("my-code");
        await _sut.AuthorizationCodeRemoveAsync();

        var code = await _sut.AuthorizationCodeGetAsync();

        Assert.IsNull(code);
    }

    [TestMethod]
    public async Task AuthorizationTokenGetAsync_WhenNoCodePresent_ReturnsNull()
    {
        var token = await _sut.AuthorizationTokenGetAsync();

        Assert.IsNull(token);
    }

    [TestMethod]
    public async Task AuthorizationTokenReset_ReturnsTrue()
    {
        var result = await _sut.AuthorizationTokenReset();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task Validate_WhenNoCodePresent_ReturnsAuthorizationUrl()
    {
        var url = await _sut.ValidateAsync();

        Assert.IsNotNull(url);
        Assert.Contains("accounts.spotify.com", url);
    }

    [TestMethod]
    public async Task Validate_WhenCodeAddedButNoToken_ReturnsAuthorizationUrl()
    {
        await _sut.AuthorizationCodeAddAsync("some-code");

        var url = await _sut.ValidateAsync();

        Assert.IsNotNull(url);
    }

    [TestMethod]
    public async Task AuthorizationCodeAddAsync_WithValidState_ReturnsTrue()
    {
        var url = _sut.AuthorizationCodeUrl();
        Assert.IsNotNull(url);

        var uri = new Uri(url);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var state = query["state"];

        var added = await _sut.AuthorizationCodeAddAsync("auth-code", state);

        Assert.IsTrue(added);
    }

    [TestMethod]
    public async Task AuthorizationCodeAddAsync_WithInvalidState_ReturnsFalse()
    {
        _sut.AuthorizationCodeUrl();

        var added = await _sut.AuthorizationCodeAddAsync("auth-code", "wrong-state");

        Assert.IsFalse(added);
    }
}
