using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using EC.Spotify.Models.Playlists;
using EC.Spotify.Models.Shared;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class PlaylistServiceTests
{
    private static readonly List<string> ReadScopes = ["playlist-read-private"];
    private static readonly List<string> ModifyScopes =
        ["playlist-modify-public", "playlist-modify-private"];
    private static readonly List<string> ImageScopes =
        ["ugc-image-upload", "playlist-modify-public", "playlist-modify-private"];

    private MockSpotifyProvider _provider = null!;

    [TestInitialize]
    public void Setup() => _provider = new MockSpotifyProvider();

    // ── PlaylistGetAsync ─────────────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistGetAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        var playlist = new Playlist { Id = "pl1" };
        _provider.Enqueue(new SpotifyResult<Playlist> { Data = playlist });

        var result = await sut.PlaylistGetAsync("pl1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("pl1", result.Data.Id);
    }

    [TestMethod]
    public async Task PlaylistGetAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        _provider.Enqueue(new SpotifyResult<Playlist> { Error = new SpotifyError { Status = 404 } });

        var result = await sut.PlaylistGetAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task PlaylistGetAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        _provider.SetException(new InvalidOperationException("network failure"));

        var result = await sut.PlaylistGetAsync("pl1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PlaylistItemGetAllAsync ──────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistItemGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ReadScopes);
        var page = new PlaylistPageResult { Total = 5 };
        _provider.Enqueue(new SpotifyResult<PlaylistPageResult> { Data = page });

        var result = await sut.PlaylistItemGetAllAsync("pl1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(5, result.Data.Total);
    }

    [TestMethod]
    public async Task PlaylistItemGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, []);

        var result = await sut.PlaylistItemGetAllAsync("pl1");

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task PlaylistItemGetAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ReadScopes);
        _provider.Enqueue(new SpotifyResult<PlaylistPageResult> { Error = new SpotifyError { Status = 400 } });

        var result = await sut.PlaylistItemGetAllAsync("pl1");

        Assert.IsFalse(result.IsSuccess);
    }

    // ── PlaylistDetailUpdateAsync ────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistDetailUpdateAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ModifyScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });
        var detail = new PlaylistDetail { Name = "New Name" };

        var result = await sut.PlaylistDetailUpdateAsync("pl1", detail);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task PlaylistDetailUpdateAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, []);

        var result = await sut.PlaylistDetailUpdateAsync("pl1", new PlaylistDetail());

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PlaylistItemAddAsync ─────────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistItemAddAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ModifyScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });
        var item = new ReferenceItem { Id = "t1", Type = ReferenceItemType.Track };

        var result = await sut.PlaylistItemAddAsync("pl1", item);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task PlaylistItemAddAsync_WhenScopesMissing_ReturnsEmptySuccess()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, []);

        var result = await sut.PlaylistItemAddAsync("pl1", new ReferenceItem { Id = "t1" });

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(result.Data);
    }

    [TestMethod]
    public async Task PlaylistItemAddAsync_WhenItemIsNull_ReturnsEmptySuccess()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ModifyScopes);

        var result = await sut.PlaylistItemAddAsync("pl1", null);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(result.Data);
    }

    // ── PlaylistItemAddAllAsync ──────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistItemAddAllAsync_WhenProviderSucceeds_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ModifyScopes);
        var items = new List<ReferenceItem>
        {
            new() { Id = "t1", Type = ReferenceItemType.Track },
            new() { Id = "t2", Type = ReferenceItemType.Track }
        };
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.PlaylistItemAddAllAsync("pl1", items);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task PlaylistItemAddAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, []);

        var result = await sut.PlaylistItemAddAllAsync("pl1", [new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PlaylistItemRemoveAsync ──────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistItemRemoveAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ModifyScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });
        var item = new ReferenceItem { Id = "t1", Type = ReferenceItemType.Track };

        var result = await sut.PlaylistItemRemoveAsync("pl1", item);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task PlaylistItemRemoveAsync_WhenScopesMissing_ReturnsEmptySuccess()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, []);

        var result = await sut.PlaylistItemRemoveAsync("pl1", new ReferenceItem { Id = "t1" });

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(result.Data);
    }

    // ── PlaylistItemRemoveAllAsync ───────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistItemRemoveAllAsync_WhenProviderSucceeds_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ModifyScopes);
        var items = new List<ReferenceItem>
        {
            new() { Id = "t1", Type = ReferenceItemType.Track },
            new() { Id = "t2", Type = ReferenceItemType.Track }
        };
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.PlaylistItemRemoveAllAsync("pl1", items);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task PlaylistItemRemoveAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, []);

        var result = await sut.PlaylistItemRemoveAllAsync("pl1", [new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PlaylistImageAddAsync ────────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistImageAddAsync_WhenProviderSucceeds_ReturnsTrue()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ImageScopes);
        _provider.Enqueue(new SpotifyResult<bool> { Data = true });

        var result = await sut.PlaylistImageAddAsync("pl1", [0xFF, 0xD8, 0xFF]);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task PlaylistImageAddAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, []);

        var result = await sut.PlaylistImageAddAsync("pl1", [0xFF]);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── PlaylistImageGetAllAsync ─────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistImageGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        var images = new List<Image> { new() { Url = "https://example.com/img.jpg" } };
        _provider.Enqueue(new SpotifyResult<List<Image>> { Data = images });

        var result = await sut.PlaylistImageGetAllAsync("pl1");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.HasCount(1, result.Data);
    }

    [TestMethod]
    public async Task PlaylistImageGetAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        _provider.Enqueue(new SpotifyResult<List<Image>> { Error = new SpotifyError { Status = 404 } });

        var result = await sut.PlaylistImageGetAllAsync("invalid");

        Assert.IsFalse(result.IsSuccess);
    }

    // ── Raw methods ──────────────────────────────────────────────────────────

    [TestMethod]
    public async Task PlaylistGetRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        _provider.SetRawResult("{\"id\":\"pl1\"}");

        var result = await sut.PlaylistGetRawAsync("pl1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task PlaylistGetRawAsync_WhenProviderReturnsNull_ReturnsNull()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        _provider.SetRawResult(null);

        var result = await sut.PlaylistGetRawAsync("pl1");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task PlaylistItemGetAllRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider, ReadScopes);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.PlaylistItemGetAllRawAsync("pl1");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task PlaylistImageGetAllRawAsync_WhenProviderReturnsData_ReturnsJson()
    {
        var sut = ServiceFactory.CreatePlaylistService(_provider);
        _provider.SetRawResult("[{\"url\":\"https://img\"}]");

        var result = await sut.PlaylistImageGetAllRawAsync("pl1");

        Assert.IsNotNull(result);
    }
}
