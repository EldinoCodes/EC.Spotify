using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class ArtistServiceTests
{
    private MockSpotifyProvider _provider = null!;
    private IArtistService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _provider = new MockSpotifyProvider();
        _sut = ServiceFactory.CreateArtistService(_provider);
    }

    [TestMethod]
    public async Task ArtistGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var artist = new Artist { Id = "artist1", Name = "Test Artist" };
        _provider.Enqueue(new SpotifyResult<Artist> { Data = artist });

        var result = await _sut.ArtistGetAsync("artist1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("artist1", result.Data.Id);
    }

    [TestMethod]
    public async Task ArtistGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var error = new SpotifyError { Status = 404, Message = "Artist not found" };
        _provider.Enqueue(new SpotifyResult<Artist> { Error = error });

        var result = await _sut.ArtistGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(404, result.Error.Status);
    }

    [TestMethod]
    public async Task ArtistGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await _sut.ArtistGetAsync("artist1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ArtistAlbumGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var page = new SpotifyPageResult<Album> { Items = [new Album { Id = "a1" }], Total = 1 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Album>> { Data = page });

        var result = await _sut.ArtistAlbumGetAllAsync("artist1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(1, result.Data.Total);
    }

    [TestMethod]
    public async Task ArtistAlbumGetAllAsync_WithAlbumTypeFilter_ReturnsSuccessResult()
    {
        var page = new SpotifyPageResult<Album> { Items = [new Album { Id = "a2" }], Total = 1 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Album>> { Data = page });

        var result = await _sut.ArtistAlbumGetAllAsync("artist1", AlbumType.Single);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task ArtistAlbumGetAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var error = new SpotifyError { Status = 400, Message = "Bad request" };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Album>> { Error = error });

        var result = await _sut.ArtistAlbumGetAllAsync("artist1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ArtistAlbumGetAllAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new HttpRequestException("connection lost"));

        var result = await _sut.ArtistAlbumGetAllAsync("artist1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task ArtistGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"id\":\"artist1\"}");

        var result = await _sut.ArtistGetRawAsync("artist1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ArtistGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.ArtistGetRawAsync("artist1");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task ArtistAlbumGetAllRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"items\":[]}");

        var result = await _sut.ArtistAlbumGetAllRawAsync("artist1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ArtistAlbumGetAllRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.ArtistAlbumGetAllRawAsync("artist1");

        Assert.IsNull(result);
    }
}
