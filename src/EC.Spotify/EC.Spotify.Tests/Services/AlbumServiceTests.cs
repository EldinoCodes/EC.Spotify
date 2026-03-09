using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class AlbumServiceTests
{

    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task AlbumGetAsync_ShouldReturnAlbum(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var album = await sut.AlbumGetAsync(id);        
        Assert.IsNotNull(album?.Data);
    }

    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task AlbumTrackGetAllAsync_ShouldReturnTracks(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var album = await sut.AlbumTrackGetAllAsync(id);
        Assert.IsNotNull(album?.Data);
    }
}
