using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T08_PlaylistServiceTests
{
    [TestMethod]
    public async Task T001_MyPlaylistGetAllAsync_ShouldReturnPlaylists()
    {
        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyPlaylistGetAllAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T002_MyPlaylistGetAsync_ShouldReturnPlaylist(string? id)
    {
        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistGetAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
