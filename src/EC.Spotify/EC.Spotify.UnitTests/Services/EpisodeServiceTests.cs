using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class EpisodeServiceTests
{
    private MockSpotifyProvider _provider = null!;

    private static readonly List<string> EpisodeScopes = ["user-read-playback-position"];
    private static readonly List<string> MyEpisodeScopes = ["user-library-read"];

    [TestInitialize]
    public void Setup() => _provider = new MockSpotifyProvider();

    [TestMethod]
    public async Task EpisodeGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, EpisodeScopes);
        var episode = new Episode { Id = "ep1" };
        _provider.Enqueue(new SpotifyResult<Episode> { Data = episode });

        var result = await sut.EpisodeGetAsync("ep1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("ep1", result.Data.Id);
    }

    [TestMethod]
    public async Task EpisodeGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, EpisodeScopes);
        var error = new SpotifyError { Status = 404, Message = "Episode not found" };
        _provider.Enqueue(new SpotifyResult<Episode> { Error = error });

        var result = await sut.EpisodeGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(404, result.Error.Status);
    }

    [TestMethod]
    public async Task EpisodeGetAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, []);

        var result = await sut.EpisodeGetAsync("ep1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(401, result.Error.Status);
    }

    [TestMethod]
    public async Task EpisodeGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, EpisodeScopes);
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await sut.EpisodeGetAsync("ep1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task EpisodeGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, EpisodeScopes);
        _provider.SetRawResult("{\"id\":\"ep1\"}");

        var result = await sut.EpisodeGetRawAsync("ep1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task EpisodeGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, EpisodeScopes);
        _provider.SetRawResult(null);

        var result = await sut.EpisodeGetRawAsync("ep1");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task EpisodeGetRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, []);

        var threw = false;
        try { await sut.EpisodeGetRawAsync("ep1"); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    // ── MyEpisodeGetAllAsync ─────────────────────────────────────────────────

    [TestMethod]
    public async Task MyEpisodeGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, MyEpisodeScopes);
        var page = new SpotifyPageResult<Episode> { Items = [new Episode { Id = "ep1" }], Total = 1 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Episode>> { Data = page });

        var result = await sut.MyEpisodeGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(1, result.Data.Total);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, []);

        var result = await sut.MyEpisodeGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(401, result.Error.Status);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, MyEpisodeScopes);
        var error = new SpotifyError { Status = 400, Message = "Bad request" };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Episode>> { Error = error });

        var result = await sut.MyEpisodeGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(400, result.Error.Status);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, MyEpisodeScopes);
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await sut.MyEpisodeGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── MyEpisodeGetAllRawAsync ──────────────────────────────────────────────

    [TestMethod]
    public async Task MyEpisodeGetAllRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, MyEpisodeScopes);
        _provider.SetRawResult("{\"items\":[{\"id\":\"ep1\"}],\"total\":1}");

        var result = await sut.MyEpisodeGetAllRawAsync();

        Assert.IsNotNull(result);
        Assert.Contains("ep1", result);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, []);

        var threw = false;
        try { await sut.MyEpisodeGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, MyEpisodeScopes);
        _provider.SetRawResult(null);

        var result = await sut.MyEpisodeGetAllRawAsync();

        Assert.IsNull(result);
    }
}
