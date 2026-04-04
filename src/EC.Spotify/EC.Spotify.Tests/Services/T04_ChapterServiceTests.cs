using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T04_ChapterServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("3OCSAZnatejMEd0Q5Ohlq7")]
    public async Task T001_ChapterGetAsync_ShouldReturnChapter(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IChapterService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ChapterGetAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
