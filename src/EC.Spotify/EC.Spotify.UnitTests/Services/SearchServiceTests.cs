using EC.Spotify.Abstractions.Models;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class SearchServiceTests
{
    private MockSpotifyProvider _provider = null!;
    private ISearchService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _provider = new MockSpotifyProvider();
        _sut = ServiceFactory.CreateSearchService(_provider);
    }

    [TestMethod]
    public async Task SearchAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var page = new SpotifyPageResult<IPolymorphicItem> { Total = 3 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<IPolymorphicItem>> { Data = page });

        var result = await _sut.SearchAsync("test query", SearchType.Track);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(3, result.Data.Total);
    }

    [TestMethod]
    public async Task SearchAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<IPolymorphicItem>>
        {
            Error = new SpotifyError { Status = 400, Message = "Bad request" }
        });

        var result = await _sut.SearchAsync("test", SearchType.Album);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task SearchAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new InvalidOperationException("search failed"));

        var result = await _sut.SearchAsync("test", SearchType.Artist);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task SearchAsync_WithNullQuery_ReturnsResultFromProvider()
    {
        var page = new SpotifyPageResult<IPolymorphicItem> { Total = 0 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<IPolymorphicItem>> { Data = page });

        var result = await _sut.SearchAsync(null, SearchType.Track);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task SearchAsync_WithMultipleSearchTypes_ReturnsSuccessResult()
    {
        var page = new SpotifyPageResult<IPolymorphicItem> { Total = 10 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<IPolymorphicItem>> { Data = page });

        var result = await _sut.SearchAsync("query", SearchType.Track | SearchType.Album);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task SearchRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"tracks\":{\"items\":[]}}");

        var result = await _sut.SearchRawAsync("test", SearchType.Track);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.SearchRawAsync("test", SearchType.Track);

        Assert.IsNull(result);
    }
}
