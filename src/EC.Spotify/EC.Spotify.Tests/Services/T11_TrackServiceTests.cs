using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T11_TrackServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS")]
    public async Task T001_TrackGetAsync_ShouldReturnTrack(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<ITrackService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.TrackGetAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS")]
    public async Task T002_TrackGetRawAsync_ShouldReturnJson(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<ITrackService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.TrackGetRawAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }
}
