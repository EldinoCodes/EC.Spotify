using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class AudiobookServiceTests
{
    [TestMethod]
    [DataRow("587ehiVcGuXjxKMECQneQ6")]
    public async Task AudiobookGetAsync_ShouldReturnAudiobook(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAudiobookService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AudiobookGetAsync(id);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    [DataRow("587ehiVcGuXjxKMECQneQ6")]
    public async Task AudiobookChapterGetAllAsync_ShouldReturnChapters(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAudiobookService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AudiobookChapterGetAllAsync(id);
        Assert.IsNotNull(result?.Data);
    }
}
