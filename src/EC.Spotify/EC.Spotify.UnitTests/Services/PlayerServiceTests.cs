using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Players;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class PlayerServiceTests
{
    private static readonly List<string> PlaybackStateScopes = ["user-read-playback-state"];
    private static readonly List<string> ModifyPlaybackScopes = ["user-modify-playback-state"];
    private static readonly List<string> AllPlayerScopes =
        ["user-read-playback-state", "user-modify-playback-state", "user-read-currently-playing"];

    private MockSpotifyProvider _provider = null!;

    [TestInitialize]
    public void Setup() => _provider = new MockSpotifyProvider();

    // ── QueueGetAsync ────────────────────────────────────────────────────────

    [TestMethod]
    public async Task QueueGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, AllPlayerScopes);
        var queue = new PlayerQueue();
        _provider.Enqueue(new SpotifyResult<PlayerQueue> { Data = queue });

        var result = await sut.QueueGetAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task QueueGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, AllPlayerScopes);
        _provider.Enqueue(new SpotifyResult<PlayerQueue> { Error = new SpotifyError { Status = 404 } });

        var result = await sut.QueueGetAsync();

        Assert.IsFalse(result.IsSuccess);
    }

    [TestMethod]
    public async Task QueueGetAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.QueueGetAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task QueueGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, AllPlayerScopes);
        _provider.SetException(new InvalidOperationException("network failure"));

        var result = await sut.QueueGetAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── QueueAddAsync ────────────────────────────────────────────────────────

    [TestMethod]
    public async Task QueueAddAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.QueueAddAsync("track-id");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(result.Data);
    }

    [TestMethod]
    public async Task QueueAddAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.QueueAddAsync("track-id");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task QueueAddAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Error = new SpotifyError { Status = 400 } });

        var result = await sut.QueueAddAsync("track-id");

        Assert.IsFalse(result.IsSuccess);
    }

    // ── DeviceGetAllAsync ────────────────────────────────────────────────────

    [TestMethod]
    public async Task DeviceGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, PlaybackStateScopes);
        var devices = new List<Device> { new() { Id = "dev1" } };
        _provider.Enqueue(new SpotifyResult<List<Device>> { Data = devices });

        var result = await sut.DeviceGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.HasCount(1, result.Data);
    }

    [TestMethod]
    public async Task DeviceGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.DeviceGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── TransferAsync ────────────────────────────────────────────────────────

    [TestMethod]
    public async Task TransferAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.TransferAsync("device-id", play: true);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task TransferAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.TransferAsync("device-id");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── StateGetAsync ────────────────────────────────────────────────────────

    [TestMethod]
    public async Task StateGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, PlaybackStateScopes);
        var state = new PlayerState();
        _provider.Enqueue(new SpotifyResult<PlayerState> { Data = state });

        var result = await sut.StateGetAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task StateGetAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.StateGetAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── CurrentlyPlayingGetAsync ─────────────────────────────────────────────

    [TestMethod]
    public async Task CurrentlyPlayingGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, AllPlayerScopes);
        _provider.Enqueue(new SpotifyResult<PlayerState> { Data = new PlayerState() });

        var result = await sut.CurrentlyPlayingGetAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task CurrentlyPlayingGetAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.CurrentlyPlayingGetAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PlayAsync ────────────────────────────────────────────────────────────

    [TestMethod]
    public async Task PlayAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.PlayAsync("device-id", ["spotify:track:abc"]);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task PlayAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.PlayAsync("device-id", null);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PauseAsync ───────────────────────────────────────────────────────────

    [TestMethod]
    public async Task PauseAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.PauseAsync("device-id");

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task PauseAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.PauseAsync("device-id");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── NextAsync ────────────────────────────────────────────────────────────

    [TestMethod]
    public async Task NextAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.NextAsync();

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task NextAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.NextAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PreviousAsync ────────────────────────────────────────────────────────

    [TestMethod]
    public async Task PreviousAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.PreviousAsync();

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task PreviousAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.PreviousAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── SeekAsync ────────────────────────────────────────────────────────────

    [TestMethod]
    public async Task SeekAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.SeekAsync(30000);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task SeekAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.SeekAsync(30000);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── RepeatAsync ──────────────────────────────────────────────────────────

    [TestMethod]
    public async Task RepeatAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.RepeatAsync(PlayerRepeatMode.Track);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task RepeatAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.RepeatAsync(PlayerRepeatMode.Off);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── ShuffleAsync ─────────────────────────────────────────────────────────

    [TestMethod]
    public async Task ShuffleAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.ShuffleAsync(PlayerShuffleMode.On);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task ShuffleAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.ShuffleAsync(PlayerShuffleMode.Off);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── VolumeAsync ──────────────────────────────────────────────────────────

    [TestMethod]
    public async Task VolumeAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.VolumeAsync(50);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task VolumeAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, []);

        var result = await sut.VolumeAsync(50);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task VolumeAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlayerService(_provider, ModifyPlaybackScopes);
        _provider.SetException(new InvalidOperationException("network failure"));

        var result = await sut.VolumeAsync(50);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }
}
