using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T05_EpisodeServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("3UcmY44Vwv4Ldh0Jd1HZ4m")]
    public async Task T001_EpisodeGetAsync_ShouldReturnEpisode(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IEpisodeService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.EpisodeGetAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
