using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T9_TrackServiceTests
{
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS")]
    public async Task TrackGetAsync_ShouldReturnTrack(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<ITrackService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.TrackGetAsync(id);
        Assert.IsNotNull(result?.Data);
    }
}
