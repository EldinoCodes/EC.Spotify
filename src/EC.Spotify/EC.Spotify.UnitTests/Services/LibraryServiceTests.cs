using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using EC.Spotify.UnitTests.Helpers;
using EC.Spotify.UnitTests.Mocks;

namespace EC.Spotify.UnitTests.Services;

[TestClass]
public sealed class LibraryServiceTests
{
    private static readonly List<string> RequiredAddScopes =
        ["user-library-modify", "user-follow-modify", "playlist-modify-public"];

    private static readonly List<string> RequiredCheckScopes =
        ["user-library-read", "user-follow-read", "playlist-read-private"];

    private MockSpotifyProvider _provider = null!;

    [TestInitialize]
    public void Setup() => _provider = new MockSpotifyProvider();

    // ── LibraryCheckAsync ────────────────────────────────────────────────────

    [TestMethod]
    public async Task LibraryCheckAsync_WhenItemSaved_ReturnsTrue()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredCheckScopes);
        var item = new ReferenceItem { Id = "t1", Type = ReferenceItemType.Track };
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [true] });

        var result = await sut.LibraryCheckAsync(item);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(result.Data);
    }

    [TestMethod]
    public async Task LibraryCheckAsync_WhenItemNotSaved_ReturnsFalse()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredCheckScopes);
        var item = new ReferenceItem { Id = "t1", Type = ReferenceItemType.Track };
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [false] });

        var result = await sut.LibraryCheckAsync(item);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.Data);
    }

    [TestMethod]
    public async Task LibraryCheckAsync_WhenItemIsNull_ReturnsFalse()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredCheckScopes);
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [] });

        var result = await sut.LibraryCheckAsync(null);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.Data);
    }

    [TestMethod]
    public async Task LibraryCheckAsync_WhenScopesMissing_ReturnsSuccessWithFalse()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, []);

        var result = await sut.LibraryCheckAsync(new ReferenceItem { Id = "t1" });

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.Data);
    }

    // ── LibraryCheckAllAsync ─────────────────────────────────────────────────

    [TestMethod]
    public async Task LibraryCheckAllAsync_WhenProviderReturnsData_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredCheckScopes);
        var items = new List<ReferenceItem>
        {
            new() { Id = "t1", Type = ReferenceItemType.Track },
            new() { Id = "t2", Type = ReferenceItemType.Track }
        };
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [true, false] });

        var result = await sut.LibraryCheckAllAsync(items);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.HasCount(2, result.Data);
    }

    [TestMethod]
    public async Task LibraryCheckAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, []);

        var result = await sut.LibraryCheckAllAsync([new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task LibraryCheckAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredCheckScopes);
        _provider.Enqueue(new SpotifyResult<List<bool>> { Error = new SpotifyError { Status = 500 } });

        var result = await sut.LibraryCheckAllAsync([new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
    }

    // ── LibraryAddAsync ──────────────────────────────────────────────────────

    [TestMethod]
    public async Task LibraryAddAsync_WhenItemAdded_ReturnsTrue()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        var item = new ReferenceItem { Id = "t1", Type = ReferenceItemType.Track };
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [] });

        var result = await sut.LibraryAddAsync(item);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task LibraryAddAsync_WhenItemIsNull_ReturnsSuccessWithFalse()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [] });

        var result = await sut.LibraryAddAsync(null);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.Data);
    }

    [TestMethod]
    public async Task LibraryAddAsync_WhenScopesMissing_ReturnsSuccessWithFalse()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, []);

        var result = await sut.LibraryAddAsync(new ReferenceItem { Id = "t1" });

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.Data);
    }

    // ── LibraryAddAllAsync ───────────────────────────────────────────────────

    [TestMethod]
    public async Task LibraryAddAllAsync_WhenProviderSucceeds_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        var items = new List<ReferenceItem>
        {
            new() { Id = "t1", Type = ReferenceItemType.Track },
            new() { Id = "t2", Type = ReferenceItemType.Track }
        };
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [] });

        var result = await sut.LibraryAddAllAsync(items);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.HasCount(2, result.Data);
        Assert.IsTrue(result.Data.All(b => b));
    }

    [TestMethod]
    public async Task LibraryAddAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, []);

        var result = await sut.LibraryAddAllAsync([new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task LibraryAddAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        _provider.Enqueue(new SpotifyResult<List<bool>> { Error = new SpotifyError { Status = 500 } });

        var result = await sut.LibraryAddAllAsync([new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
    }

    // ── LibraryRemoveAsync ───────────────────────────────────────────────────

    [TestMethod]
    public async Task LibraryRemoveAsync_WhenItemRemoved_ReturnsSuccess()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        var item = new ReferenceItem { Id = "t1", Type = ReferenceItemType.Track };
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [] });

        var result = await sut.LibraryRemoveAsync(item);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task LibraryRemoveAsync_WhenItemIsNull_ReturnsSuccessWithFalse()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [] });

        var result = await sut.LibraryRemoveAsync(null);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.Data);
    }

    [TestMethod]
    public async Task LibraryRemoveAsync_WhenScopesMissing_ReturnsSuccessWithFalse()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, []);

        var result = await sut.LibraryRemoveAsync(new ReferenceItem { Id = "t1" });

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.Data);
    }

    // ── LibraryRemoveAllAsync ────────────────────────────────────────────────

    [TestMethod]
    public async Task LibraryRemoveAllAsync_WhenProviderSucceeds_ReturnsSuccessResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        var items = new List<ReferenceItem>
        {
            new() { Id = "t1", Type = ReferenceItemType.Track },
            new() { Id = "t2", Type = ReferenceItemType.Track }
        };
        _provider.Enqueue(new SpotifyResult<List<bool>> { Data = [] });

        var result = await sut.LibraryRemoveAllAsync(items);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.HasCount(2, result.Data);
        Assert.IsTrue(result.Data.All(b => b));
    }

    [TestMethod]
    public async Task LibraryRemoveAllAsync_WhenScopesMissing_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, []);

        var result = await sut.LibraryRemoveAllAsync([new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public async Task LibraryRemoveAllAsync_WhenProviderReturnsError_ReturnsErrorResult()
    {
        var sut = ServiceFactory.CreateLibraryService(_provider, RequiredAddScopes);
        _provider.Enqueue(new SpotifyResult<List<bool>> { Error = new SpotifyError { Status = 500 } });

        var result = await sut.LibraryRemoveAllAsync([new() { Id = "t1" }]);

        Assert.IsFalse(result.IsSuccess);
    }
}
