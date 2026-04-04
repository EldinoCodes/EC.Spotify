using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T01_AlbumServiceTests
{
    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task T001_AlbumGetAsync_ShouldReturnAlbum(string? id)
    {
        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AlbumGetAsync(id);        
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task T002_AlbumTrackGetAllAsync_ShouldReturnTracks(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AlbumTrackGetAllAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
