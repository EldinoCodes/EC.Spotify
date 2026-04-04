using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T02_ArtistServiceTests
{
    [TestMethod]
    [DataRow("0X380XXQSNBYuleKzav5UO")]
    public async Task T001_ArtistGetAsync_ShouldReturnArtist(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IArtistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ArtistGetAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("0X380XXQSNBYuleKzav5UO")]
    public async Task T002_ArtistAlbumGetAllAsync_ShouldReturnAlbums(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IArtistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ArtistAlbumGetAllAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
