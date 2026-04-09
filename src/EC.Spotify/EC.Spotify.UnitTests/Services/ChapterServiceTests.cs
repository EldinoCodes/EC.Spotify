using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class ChapterServiceTests
{
    private MockSpotifyProvider _provider = null!;
    private IChapterService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _provider = new MockSpotifyProvider();
        _sut = ServiceFactory.CreateChapterService(_provider);
    }

    [TestMethod]
    public async Task ChapterGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var chapter = new Chapter { Id = "ch1" };
        _provider.Enqueue(new SpotifyResult<Chapter> { Data = chapter });

        var result = await _sut.ChapterGetAsync("ch1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("ch1", result.Data.Id);
    }

    [TestMethod]
    public async Task ChapterGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var error = new SpotifyError { Status = 404, Message = "Chapter not found" };
        _provider.Enqueue(new SpotifyResult<Chapter> { Error = error });

        var result = await _sut.ChapterGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(404, result.Error.Status);
    }

    [TestMethod]
    public async Task ChapterGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await _sut.ChapterGetAsync("ch1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ChapterGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"id\":\"ch1\"}");

        var result = await _sut.ChapterGetRawAsync("ch1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ChapterGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.ChapterGetRawAsync("ch1");

        Assert.IsNull(result);
    }
}
