using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T07_PlayerServiceTests
{
    [TestMethod]
    public async Task QueueGetAsync_ShouldReturnQueue()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.QueueGetAsync();
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    [DataRow("spotify:track:4tjcBY787A2ZkRJpPIsGIS")]
    public async Task QueueAddAsync_ShouldAddTrackToQueue(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.QueueAddAsync(id);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    public async Task DeviceGetAllAsync_ShouldReturnDevices()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.DeviceGetAllAsync();
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    [DataRow("test")]
    public async Task TransferAsync_ShouldTransferPlayback(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.TransferAsync(id);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    public async Task PlayerPlayAsync_ShouldStartPlayback()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerPlayAsync(null, null);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    public async Task PlayerPauseAsync_ShouldPausePlayback()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerPauseAsync(null);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    public async Task PlayerNextAsync_ShouldSkipToNextTrack()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerNextAsync(null);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    public async Task PlayerPreviousAsync_ShouldSkipToPreviousTrack()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerPreviousAsync(null);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    public async Task PlayerSeekAsync_ShouldSeekToPosition()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerSeekAsync(60000);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    [DataRow(PlayerRepeatMode.Track)]
    public async Task PlayerRepeatAsync_ShouldSetRepeatMode(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off)
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerRepeatAsync(playerRepeatMode);
        Assert.IsNotNull(result?.Data);
    }

    [TestMethod]
    [DataRow(PlayerShuffleMode.On)]
    public async Task PlayerShuffleAsync_ShouldSetShuffleMode(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off)
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerShuffleAsync(playerShuffleMode);
        Assert.IsNotNull(result?.Data);

    }

    [TestMethod]
    [DataRow(20)]
    [DataRow(80)]
    public async Task PlayerVolumeAsync_ShouldSetVolume(int volumePercent)
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayerVolumeAsync(volumePercent);
        Assert.IsNotNull(result?.Data);
    }
}
