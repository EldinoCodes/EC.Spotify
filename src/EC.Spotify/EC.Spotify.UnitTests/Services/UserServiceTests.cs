using EC.Spotify.Abstractions.Models;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.Models.Playlists;
using EC.Spotify.Models.Shows;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class UserServiceTests
{
    private static readonly List<string> LibraryReadScope = ["user-library-read"];
    private static readonly List<string> PlaylistReadScope = ["playlist-read-private"];
    private static readonly List<string> TopReadScope = ["user-top-read"];

    private static readonly List<string> AllScopes =
    [
        "user-library-read",
        "user-read-playback-position",
        "playlist-read-private",
        "user-top-read"
    ];

    private MockSpotifyProvider _provider = null!;

    [TestInitialize]
    public void Setup() => _provider = new MockSpotifyProvider();

    // ── MyAlbumGetAllAsync ───────────────────────────────────────────────────

    [TestMethod]
    public async Task MyAlbumGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);
        var page = new SpotifyPageResult<Album> { Total = 5 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Album>> { Data = page });

        var result = await sut.MyAlbumGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(5, result.Data.Total);
    }

    [TestMethod]
    public async Task MyAlbumGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var result = await sut.MyAlbumGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task MyAlbumGetAllAsync_WhenLimitOutOfRange_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);

        var result = await sut.MyAlbumGetAllAsync(limit: 0);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── MyAudiobookGetAllAsync ───────────────────────────────────────────────

    [TestMethod]
    public async Task MyAudiobookGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);
        var page = new SpotifyPageResult<Audiobook> { Total = 2 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Audiobook>> { Data = page });

        var result = await sut.MyAudiobookGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task MyAudiobookGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var result = await sut.MyAudiobookGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task MyAudiobookGetAllAsync_WhenLimitOutOfRange_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);

        var result = await sut.MyAudiobookGetAllAsync(limit: 51);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── MyEpisodeGetAllAsync ─────────────────────────────────────────────────

    [TestMethod]
    public async Task MyEpisodeGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, ["user-library-read", "user-read-playback-position"]);
        var page = new SpotifyPageResult<Episode> { Total = 3 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Episode>> { Data = page });

        var result = await sut.MyEpisodeGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var result = await sut.MyEpisodeGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── MyPlaylistGetAllAsync ────────────────────────────────────────────────

    [TestMethod]
    public async Task MyPlaylistGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, PlaylistReadScope);
        var page = new SpotifyPageResult<Playlist> { Total = 10 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Playlist>> { Data = page });

        var result = await sut.MyPlaylistGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(10, result.Data.Total);
    }

    [TestMethod]
    public async Task MyPlaylistGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var result = await sut.MyPlaylistGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── MyShowGetAllAsync ────────────────────────────────────────────────────

    [TestMethod]
    public async Task MyShowGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, ["user-library-read", "user-read-playback-position"]);
        var page = new SpotifyPageResult<Show> { Total = 4 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Show>> { Data = page });

        var result = await sut.MyShowGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
    }

    [TestMethod]
    public async Task MyShowGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var result = await sut.MyShowGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task MyShowGetAllAsync_WhenLimitOutOfRange_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, ["user-library-read", "user-read-playback-position"]);

        var result = await sut.MyShowGetAllAsync(limit: 0);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── MyTrackGetAllAsync ───────────────────────────────────────────────────

    [TestMethod]
    public async Task MyTrackGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);
        var page = new SpotifyPageResult<Track> { Total = 8 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<Track>> { Data = page });

        var result = await sut.MyTrackGetAllAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(8, result.Data.Total);
    }

    [TestMethod]
    public async Task MyTrackGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var result = await sut.MyTrackGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task MyTrackGetAllAsync_WhenLimitOutOfRange_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);

        var result = await sut.MyTrackGetAllAsync(limit: 51);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── MyTopItemGetAllAsync ─────────────────────────────────────────────────

    [TestMethod]
    public async Task MyTopItemGetAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, TopReadScope);
        var page = new SpotifyPageResult<IPolymorphicItem> { Total = 20 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<IPolymorphicItem>> { Data = page });

        var result = await sut.MyTopItemGetAllAsync(UserTopType.Tracks, UserTopTimeRange.ShortTerm);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(20, result.Data.Total);
    }

    [TestMethod]
    public async Task MyTopItemGetAllAsync_ForArtists_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, TopReadScope);
        var page = new SpotifyPageResult<IPolymorphicItem> { Total = 10 };
        _provider.Enqueue(new SpotifyResult<SpotifyPageResult<IPolymorphicItem>> { Data = page });

        var result = await sut.MyTopItemGetAllAsync(UserTopType.Artists);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task MyTopItemGetAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var result = await sut.MyTopItemGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task MyTopItemGetAllAsync_WhenProviderThrows_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateUserService(_provider, TopReadScope);
        _provider.SetException(new InvalidOperationException("network failure"));

        var result = await sut.MyTopItemGetAllAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    // ── Raw methods ──────────────────────────────────────────────────────────

    [TestMethod]
    public async Task MyAlbumGetAllRawAsync_WhenScopesPresent_ReturnsJson()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.MyAlbumGetAllRawAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task MyAlbumGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var threw = false;
        try { await sut.MyAlbumGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task MyAudiobookGetAllRawAsync_WhenScopesPresent_ReturnsJson()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.MyAudiobookGetAllRawAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task MyAudiobookGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var threw = false;
        try { await sut.MyAudiobookGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllRawAsync_WhenScopesPresent_ReturnsJson()
    {
        var sut = ServiceFactory.CreateUserService(_provider, ["user-library-read", "user-read-playback-position"]);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.MyEpisodeGetAllRawAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task MyEpisodeGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var threw = false;
        try { await sut.MyEpisodeGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task MyPlaylistGetAllRawAsync_WhenScopesPresent_ReturnsJson()
    {
        var sut = ServiceFactory.CreateUserService(_provider, PlaylistReadScope);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.MyPlaylistGetAllRawAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task MyPlaylistGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var threw = false;
        try { await sut.MyPlaylistGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task MyShowGetAllRawAsync_WhenScopesPresent_ReturnsJson()
    {
        var sut = ServiceFactory.CreateUserService(_provider, ["user-library-read", "user-read-playback-position"]);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.MyShowGetAllRawAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task MyShowGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var threw = false;
        try { await sut.MyShowGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task MyTrackGetAllRawAsync_WhenScopesPresent_ReturnsJson()
    {
        var sut = ServiceFactory.CreateUserService(_provider, LibraryReadScope);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.MyTrackGetAllRawAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task MyTrackGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var threw = false;
        try { await sut.MyTrackGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }

    [TestMethod]
    public async Task MyTopItemGetAllRawAsync_WhenScopesPresent_ReturnsJson()
    {
        var sut = ServiceFactory.CreateUserService(_provider, TopReadScope);
        _provider.SetRawResult("{\"items\":[]}");

        var result = await sut.MyTopItemGetAllRawAsync();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task MyTopItemGetAllRawAsync_WhenScopesMissing_Throws()
    {
        var sut = ServiceFactory.CreateUserService(_provider, []);

        var threw = false;
        try { await sut.MyTopItemGetAllRawAsync(); }
        catch (InvalidOperationException) { threw = true; }
        Assert.IsTrue(threw);
    }
}
