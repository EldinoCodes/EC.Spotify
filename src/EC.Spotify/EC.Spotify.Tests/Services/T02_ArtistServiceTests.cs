using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T02_ArtistServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("0X380XXQSNBYuleKzav5UO")]
    public async Task T001_ArtistGetAsync_ShouldReturnArtist(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IArtistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ArtistGetAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("0X380XXQSNBYuleKzav5UO")]
    public async Task T002_ArtistAlbumGetAllAsync_ShouldReturnAlbums(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IArtistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ArtistAlbumGetAllAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
