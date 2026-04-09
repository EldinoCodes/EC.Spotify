using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class ShowServiceTests
{
    private static readonly List<string> RequiredScopes = ["user-read-playback-position"];

    private MockSpotifyProvider _provider = null!;

    [TestInitialize]
    public void Setup() => _provider = new MockSpotifyProvider();

    [TestMethod]
    public async Task ShowGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        var show = new Show { Id = "show1" };
        _provider.Enqueue(new SpotifyResult<Show> { Data = show });

        var result = await sut.ShowGetAsync("show1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("show1", result.Data.Id);
    }

    [TestMethod]
    public async Task ShowGetAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, []);

        var result = await sut.ShowGetAsync("show1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ShowGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.Enqueue(new SpotifyResult<Show> { Error = new SpotifyError { Status = 404 } });

        var result = await sut.ShowGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ShowGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await sut.ShowGetAsync("show1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ShowEpisodeGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        var page = new SpotifyPageResult<Episode> { Items = [new Episode { Id = "ep1" }], Total = 1 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Episode>> { Data = page });

        var result = await sut.ShowEpisodeGetAllAsync("show1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(1, result.Data.Total);
    }

    [TestMethod]
    public async Task ShowEpisodeGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, []);

        var result = await sut.ShowEpisodeGetAllAsync("show1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ShowEpisodeGetAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Episode>>
        {
            Error = new SpotifyError { Status = 400 }
        });

        var result = await sut.ShowEpisodeGetAllAsync("show1");

        Assert.IsFalse(result.IsSuccess);
    }

    [TestMethod]
    public async Task ShowEpisodeGetAllAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.SetException(new InvalidOperationException("timeout"));

        var result = await sut.ShowEpisodeGetAllAsync("show1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ShowGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.SetRawResult("{\"id\":\"show1\"}");

        var result = await sut.ShowGetRawAsync("show1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ShowGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.SetRawResult(null);

        var result = await sut.ShowGetRawAsync("show1");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task ShowGetRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateShowService(_provider, []);

        var threw = false;
        try { await sut.ShowGetRawAsync("show1"); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task ShowEpisodeGetAllRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.ShowEpisodeGetAllRawAsync("show1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ShowEpisodeGetAllRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        var sut = ServiceFactory.CreateShowService(_provider, RequiredScopes);
        _provider.SetRawResult(null);

        var result = await sut.ShowEpisodeGetAllRawAsync("show1");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task ShowEpisodeGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateShowService(_provider, []);

        var threw = false;
        try { await sut.ShowEpisodeGetAllRawAsync("show1"); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }
}
