using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class AlbumServiceTests
{
    private MockSpotifyProvider _provider = null!;
    private IAlbumService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _provider = new MockSpotifyProvider();
        _sut = ServiceFactory.CreateAlbumService(_provider);
    }

    [TestMethod]
    public async Task AlbumGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var album = new Album { Id = "abc123" };
        _provider.Enqueue(new SpotifyResult<Album> { Data = album });

        var result = await _sut.AlbumGetAsync("abc123");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("abc123", result.Data.Id);
    }

    [TestMethod]
    public async Task AlbumGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var error = new SpotifyError { Status = 404, Message = "Not found" };
        _provider.Enqueue(new SpotifyResult<Album> { Error = error });

        var result = await _sut.AlbumGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(404, result.Error.Status);
    }

    [TestMethod]
    public async Task AlbumGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new InvalidOperationException("connection error"));

        var result = await _sut.AlbumGetAsync("abc123");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }


    [TestMethod]
    public async Task AlbumGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"id\":\"abc123\"}");

        var result = await _sut.AlbumGetRawAsync("abc123");

        Assert.IsNotNull(result);
        Assert.Contains("abc123", result);
    }

    [TestMethod]
    public async Task AlbumGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.AlbumGetRawAsync("abc123");

        Assert.IsNull(result);
    }


    [TestMethod]
    public async Task AlbumTrackGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var page = new SpotifyPageResult<Track> { Items = [new Track { Id = "t1" }], Total = 1 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Track>> { Data = page });

        var result = await _sut.AlbumTrackGetAllAsync("abc123");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(1, result.Data.Total);
    }

    [TestMethod]
    public async Task AlbumTrackGetAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var error = new SpotifyError { Status = 400, Message = "Bad request" };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Track>> { Error = error });

        var result = await _sut.AlbumTrackGetAllAsync("abc123");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task AlbumTrackGetAllAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new HttpRequestException("timeout"));

        var result = await _sut.AlbumTrackGetAllAsync("abc123");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }


    [TestMethod]
    public async Task AlbumTrackGetAllRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"items\":[]}");

        var result = await _sut.AlbumTrackGetAllRawAsync("abc123");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task AlbumTrackGetAllRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.AlbumTrackGetAllRawAsync("abc123");

        Assert.IsNull(result);
    }
}
