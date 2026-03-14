using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T04_ChapterServiceTests
{
    [TestMethod]
    [DataRow("3OCSAZnatejMEd0Q5Ohlq7")]
    public async Task ChapterGetAsync_ShouldReturnChapter(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IChapterService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ChapterGetAsync(id);
        Assert.IsNotNull(result?.Data);
    }
}
