using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T12_UserServiceTests
{
    [TestMethod]
    public async Task T001_MyAlbumGetAllAsync_ShouldReturnAlbums()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyAlbumGetAllAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T002_MyAudiobookGetAllAsync_ShouldReturnAudiobooks()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyAudiobookGetAllAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T003_MyEpisodeGetAllAsync_ShouldReturnEpisodes()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyEpisodeGetAllAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T004_MyShowGetAllAsync_ShouldReturnShows()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyShowGetAllAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T005_MyTrackGetAllAsync_ShouldReturnTracks()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyTrackGetAllAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow(UserTopType.Tracks, UserTopTimeRange.ShortTerm)]
    [DataRow(UserTopType.Tracks, UserTopTimeRange.LongTerm)]
    [DataRow(UserTopType.Artists, UserTopTimeRange.MediumTerm)]
    [DataRow(UserTopType.Artists, UserTopTimeRange.LongTerm)]
    public async Task T006_MyTopItemGetAllAsync_ShouldReturnTopItems(UserTopType userTopType, UserTopTimeRange userTopTimeRange)
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyTopItemGetAllAsync(userTopType: userTopType, userTopTimeRange: userTopTimeRange);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
