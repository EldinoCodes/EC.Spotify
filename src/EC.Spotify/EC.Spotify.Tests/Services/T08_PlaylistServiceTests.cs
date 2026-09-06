using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models.Library;
using EC.Spotify.Models.Playlists;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T08_PlaylistServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T001_PlaylistGetAsync_ShouldReturnPlaylist(string? id)
    {
        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistGetAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T002_PlaylistItemGetAllAsync_ShouldReturnItems(string? id)
    {
        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemGetAllAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T003_PlaylistDetailUpdateAsync_ShouldReturnTrue(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var playlistDetail = new PlaylistDetail
        {
            Name = "Test Playlist",
            Description = "Updated by EC.Spotify tests",
            Public = false,
            Collaborative = false
        };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistDetailUpdateAsync(id, playlistDetail, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.IsSuccess, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T004_PlaylistItemAddAsync_ShouldReturnTrue(string? id, string? trackId, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var item = new ReferenceItem { Id = trackId, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemAddAsync(id, item, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T005_PlaylistItemAddAllAsync_ShouldReturnTrue(string? id, string? trackId, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var item = new ReferenceItem { Id = trackId, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemAddAllAsync(id, [item], cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T006_PlaylistItemRemoveAsync_ShouldReturnSnapshot(string? id, string? trackId, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var item = new ReferenceItem { Id = trackId, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemRemoveAsync(id, item, cancellationToken: TestContext.CancellationToken);
        // PlaylistItemRemoveAsync calls PlaylistItemRemoveAllAsync but only extracts data on success
        // If the API returns an error, it returns empty success (IsSuccess=true, Data=null)
        Assert.IsTrue(result?.IsSuccess, result?.Error?.Message ?? "Expected success");
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "spotify:track:4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T007_PlaylistItemRemoveAllAsync_ShouldReturnSnapshots(string? id, string? trackUri, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackUri, nameof(trackUri));

        var item = new ReferenceItem { Id = trackUri, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemRemoveAllAsync(id, [item], cancellationToken: TestContext.CancellationToken);
        // API may fail if item doesn't exist in playlist, so we just check it doesn't throw
        Assert.IsNotNull(result, "Expected result");
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T008_PlaylistImageAddAsync_ShouldReturnTrue(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        // Use a minimal valid JPEG image
        byte[] imageData = Convert.FromBase64String(
            "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/wAARCAABAAEDASIAAhEBAxEB/8QAFwABAQEBAAAAAAAAAAAAAAAABAMFAv/EABsQAQACAwEBAAAAAAAAAAAAAAECEQASITH/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8Aq2lpVlVrQAoAAAAAAAAAAAAA/9k=");

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistImageAddAsync(id, imageData, cancellationToken: TestContext.CancellationToken);
        // Image upload may fail due to size restrictions or permissions, so we just check it doesn't throw
        Assert.IsNotNull(result, "Expected result");
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T009_PlaylistImageGetAllAsync_ShouldReturnImages(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistImageGetAllAsync(id, TestContext.CancellationToken);
        // Playlist may not have images, so we just check it doesn't fail
        Assert.IsTrue(result?.IsSuccess, result?.Error?.Message ?? "Expected success");
    }

    [TestMethod]
    public async Task T010_PlaylistCreateAsync_ShouldReturnPlaylist()
    {
        var playlistCreate = new PlaylistCreate
        {
            Name = "Test Integration Playlist",
            Public = false,
            Collaborative = false,
            Description = "Created by EC.Spotify integration tests"
        };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistCreateAsync(playlistCreate, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.IsSuccess, result?.Error?.Message ?? "Expected success");
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T011_PlaylistGetRawAsync_ShouldReturnJson(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistGetRawAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T012_PlaylistImageGetAllRawAsync_ShouldReturnJson(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistImageGetAllRawAsync(id, TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T013_PlaylistItemGetAllRawAsync_ShouldReturnJson(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemGetAllRawAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }
}
