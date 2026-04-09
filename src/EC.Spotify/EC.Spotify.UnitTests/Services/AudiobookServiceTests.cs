using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class AudiobookServiceTests
{
    private MockSpotifyProvider _provider = null!;
    private IAudiobookService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _provider = new MockSpotifyProvider();
        _sut = ServiceFactory.CreateAudiobookService(_provider);
    }

    [TestMethod]
    public async Task AudiobookGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var audiobook = new Audiobook { Id = "ab1" };
        _provider.Enqueue(new SpotifyResult<Audiobook> { Data = audiobook });

        var result = await _sut.AudiobookGetAsync("ab1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("ab1", result.Data.Id);
    }

    [TestMethod]
    public async Task AudiobookGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var error = new SpotifyError { Status = 404, Message = "Audiobook not found" };
        _provider.Enqueue(new SpotifyResult<Audiobook> { Error = error });

        var result = await _sut.AudiobookGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(404, result.Error.Status);
    }

    [TestMethod]
    public async Task AudiobookGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await _sut.AudiobookGetAsync("ab1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task AudiobookChapterGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var page = new SpotifyPageResult<Chapter> { Items = [new Chapter { Id = "ch1" }], Total = 1 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Chapter>> { Data = page });

        var result = await _sut.AudiobookChapterGetAllAsync("ab1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(1, result.Data.Total);
    }

    [TestMethod]
    public async Task AudiobookChapterGetAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var error = new SpotifyError { Status = 400, Message = "Bad request" };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Chapter>> { Error = error });

        var result = await _sut.AudiobookChapterGetAllAsync("ab1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task AudiobookChapterGetAllAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new HttpRequestException("timeout"));

        var result = await _sut.AudiobookChapterGetAllAsync("ab1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task AudiobookGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"id\":\"ab1\"}");

        var result = await _sut.AudiobookGetRawAsync("ab1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task AudiobookGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.AudiobookGetRawAsync("ab1");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task AudiobookChapterGetAllRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"items\":[]}");

        var result = await _sut.AudiobookChapterGetAllRawAsync("ab1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task AudiobookChapterGetAllRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.AudiobookChapterGetAllRawAsync("ab1");

        Assert.IsNull(result);
    }
}
