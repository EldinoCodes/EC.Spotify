using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T03_AudiobookServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("587ehiVcGuXjxKMECQneQ6")]
    public async Task T001_AudiobookGetAsync_ShouldReturnAudiobook(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAudiobookService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AudiobookGetAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("587ehiVcGuXjxKMECQneQ6")]
    public async Task T002_AudiobookChapterGetAllAsync_ShouldReturnChapters(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAudiobookService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AudiobookChapterGetAllAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("587ehiVcGuXjxKMECQneQ6")]
    public async Task T003_AudiobookGetRawAsync_ShouldReturnJson(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAudiobookService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AudiobookGetRawAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    [DataRow("587ehiVcGuXjxKMECQneQ6")]
    public async Task T004_AudiobookChapterGetAllRawAsync_ShouldReturnJson(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAudiobookService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AudiobookChapterGetAllRawAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }
}
