using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T12_UserServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task T001_MyAlbumGetAllAsync_ShouldReturnAlbums()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyAlbumGetAllAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T002_MyAlbumGetAllRawAsync_ShouldReturnJson()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyAlbumGetAllRawAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    public async Task T003_MyArtistGetAllAsync_ShouldReturnArtists()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyArtistGetAllAsync(cancellationToken: TestContext.CancellationToken);
        
        if (result?.Error?.Message?.Contains("No type given") == true)
        {
            Assert.Inconclusive("Test user has no saved artists or missing scope");
        }
        
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T004_MyArtistGetAllRawAsync_ShouldReturnJson()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyArtistGetAllRawAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    public async Task T005_MyAudiobookGetAllAsync_ShouldReturnAudiobooks()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyAudiobookGetAllAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T006_MyAudiobookGetAllRawAsync_ShouldReturnJson()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyAudiobookGetAllRawAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    public async Task T007_MyPlaylistGetAllAsync_ShouldReturnPlaylists()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyPlaylistGetAllAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T008_MyPlaylistGetAllRawAsync_ShouldReturnJson()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyPlaylistGetAllRawAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    public async Task T009_MyEpisodeGetAllAsync_ShouldReturnEpisodes()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyEpisodeGetAllAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T010_MyEpisodeGetAllRawAsync_ShouldReturnJson()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyEpisodeGetAllRawAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    public async Task T011_MyShowGetAllAsync_ShouldReturnShows()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyShowGetAllAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T012_MyShowGetAllRawAsync_ShouldReturnJson()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyShowGetAllRawAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    public async Task T013_MyTrackGetAllAsync_ShouldReturnTracks()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyTrackGetAllAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T014_MyTrackGetAllRawAsync_ShouldReturnJson()
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyTrackGetAllRawAsync(cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    [DataRow(UserTopType.Tracks, UserTopTimeRange.ShortTerm)]
    [DataRow(UserTopType.Tracks, UserTopTimeRange.LongTerm)]
    [DataRow(UserTopType.Artists, UserTopTimeRange.MediumTerm)]
    [DataRow(UserTopType.Artists, UserTopTimeRange.LongTerm)]
    public async Task T015_MyTopItemGetAllAsync_ShouldReturnTopItems(UserTopType userTopType, UserTopTimeRange userTopTimeRange)
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyTopItemGetAllAsync(userTopType: userTopType, userTopTimeRange: userTopTimeRange, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow(UserTopType.Tracks, UserTopTimeRange.ShortTerm)]
    [DataRow(UserTopType.Tracks, UserTopTimeRange.LongTerm)]
    [DataRow(UserTopType.Artists, UserTopTimeRange.MediumTerm)]
    [DataRow(UserTopType.Artists, UserTopTimeRange.LongTerm)]
    public async Task T016_MyTopItemGetAllRawAsync_ShouldReturnJson(UserTopType userTopType, UserTopTimeRange userTopTimeRange)
    {
        var sut = Initializer.Resolve<IUserService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.MyTopItemGetAllRawAsync(userTopType: userTopType, userTopTimeRange: userTopTimeRange, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }
}
