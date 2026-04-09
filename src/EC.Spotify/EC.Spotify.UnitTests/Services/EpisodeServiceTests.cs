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

    private static readonly List<string> RequiredScopes = ["user-read-playback-position"];

    [TestInitialize]
    public void Setup() => _provider = new MockSpotifyProvider();

    [TestMethod]
    public async Task EpisodeGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, RequiredScopes);
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
        var sut = ServiceFactory.CreateEpisodeService(_provider, RequiredScopes);
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
        var sut = ServiceFactory.CreateEpisodeService(_provider, RequiredScopes);
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await sut.EpisodeGetAsync("ep1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task EpisodeGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, RequiredScopes);
        _provider.SetRawResult("{\"id\":\"ep1\"}");

        var result = await sut.EpisodeGetRawAsync("ep1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task EpisodeGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        var sut = ServiceFactory.CreateEpisodeService(_provider, RequiredScopes);
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
}
