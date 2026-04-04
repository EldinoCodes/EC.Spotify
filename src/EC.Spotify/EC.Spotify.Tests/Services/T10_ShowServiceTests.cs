using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T10_ShowServiceTests
{
    [TestMethod]
    [DataRow("2zBUqgc1ZmvqqEdP4g2jjA")]
    public async Task T001_ShowGetAsync_ShouldReturnShow(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IShowService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ShowGetAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("2zBUqgc1ZmvqqEdP4g2jjA")]
    public async Task T002_ShowEpisodeGetAllAsync_ShouldReturnEpisodes(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IShowService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ShowEpisodeGetAllAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
