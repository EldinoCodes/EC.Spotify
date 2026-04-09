using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class TrackServiceTests
{
    private MockSpotifyProvider _provider = null!;
    private ITrackService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _provider = new MockSpotifyProvider();
        _sut = ServiceFactory.CreateTrackService(_provider);
    }

    [TestMethod]
    public async Task TrackGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var track = new Track { Id = "tr1" };
        _provider.Enqueue(new SpotifyResult<Track> { Data = track });

        var result = await _sut.TrackGetAsync("tr1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("tr1", result.Data.Id);
    }

    [TestMethod]
    public async Task TrackGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        _provider.Enqueue(new SpotifyResult<Track> { Error = new SpotifyError { Status = 404 } });

        var result = await _sut.TrackGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task TrackGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        _provider.SetException(new InvalidOperationException("provider failure"));

        var result = await _sut.TrackGetAsync("tr1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task TrackGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        _provider.SetRawResult("{\"id\":\"tr1\"}");

        var result = await _sut.TrackGetRawAsync("tr1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task TrackGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        _provider.SetRawResult(null);

        var result = await _sut.TrackGetRawAsync("tr1");

        Assert.IsNull(result);
    }
}
