using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T07_PlayerServiceTests
{
    [TestMethod]
    public async Task T001_QueueGetAsync_ShouldReturnQueue()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.QueueGetAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("spotify:track:4tjcBY787A2ZkRJpPIsGIS")]
    public async Task T002_QueueAddAsync_ShouldAddTrackToQueue(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.QueueAddAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T003_DeviceGetAllAsync_ShouldReturnDevices()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.DeviceGetAllAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("test")]
    public async Task T004_TransferAsync_ShouldTransferPlayback(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.TransferAsync(id);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T005_PlayAsync_ShouldStartPlayback()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlayAsync(null, null);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T006_PauseAsync_ShouldPausePlayback()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PauseAsync(null);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T007_NextAsync_ShouldSkipToNextTrack()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.NextAsync(null);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T008_PreviousAsync_ShouldSkipToPreviousTrack()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PreviousAsync(null);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T009_SeekAsync_ShouldSeekToPosition()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.SeekAsync(60000);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow(PlayerRepeatMode.Track)]
    public async Task T010_RepeatAsync_ShouldSetRepeatMode(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off)
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.RepeatAsync(playerRepeatMode);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow(PlayerShuffleMode.On)]
    public async Task T011_ShuffleAsync_ShouldSetShuffleMode(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off)
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.ShuffleAsync(playerShuffleMode);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow(20)]
    [DataRow(80)]
    public async Task T012_VolumeAsync_ShouldSetVolume(int volumePercent)
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.VolumeAsync(volumePercent);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T013_StateGetAsync_ShouldReturnPlayerState()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.StateGetAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    public async Task T014_CurrentlyPlayingGetAsync_ShouldReturnCurrentlyPlaying()
    {
        var sut = Initializer.Resolve<IPlayerService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.CurrentlyPlayingGetAsync();
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
