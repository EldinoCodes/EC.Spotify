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
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "7a3LWj5xSFhFRYmztS8wgK", ReferenceItemType.Track)]
    public async Task T004_PlaylistItemAddAsync_ShouldReturnTrue(string? id, string? trackId, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var item = new ReferenceItem { Id = trackId, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemAddAsync(id, item, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "7a3LWj5xSFhFRYmztS8wgK", ReferenceItemType.Track)]
    public async Task T005_PlaylistItemAddAllAsync_ShouldReturnTrue(string? id, string? trackId, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var item = new ReferenceItem { Id = trackId, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemAddAllAsync(id, [item], cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data?.All(i => i == true), result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "7a3LWj5xSFhFRYmztS8wgK", ReferenceItemType.Track)]
    public async Task T006_PlaylistItemRemoveAsync_ShouldReturnTrue(string? id, string? trackId, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var item = new ReferenceItem { Id = trackId, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemRemoveAsync(id, item, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb", "7a3LWj5xSFhFRYmztS8wgK", ReferenceItemType.Track)]
    public async Task T007_PlaylistItemRemoveAllAsync_ShouldReturnTrue(string? id, string? trackId, ReferenceItemType itemType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var item = new ReferenceItem { Id = trackId, Type = itemType };

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistItemRemoveAllAsync(id, [item], cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data?.All(i => i == true), result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T008_PlaylistImageAddAsync_ShouldReturnTrue(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        byte[] imageData = Convert.FromBase64String(
            "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDB" +
            "kSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/wAAR" +
            "CAABAAEDASIAAhEBAxEB/8QAFwABAQEBAAAAAAAAAAAAAAAABAMFAv/EABsQAQAC" +
            "AwEBAAAAAAAAAAAAAAECEQASITH/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEB" +
            "AAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8Aq2lpVlVrQAoAAAAAAAAAAAAA/9k=");

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistImageAddAsync(id, imageData, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.IsSuccess, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("74Ofg2hLcn32RUvFJOxdlb")]
    public async Task T009_PlaylistImageGetAllAsync_ShouldReturnTrue(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IPlaylistService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.PlaylistImageGetAllAsync(id, TestContext.CancellationToken);
        Assert.IsGreaterThan(0, result?.Data?.Count ?? 0, result?.Error?.Message);
    }
}
