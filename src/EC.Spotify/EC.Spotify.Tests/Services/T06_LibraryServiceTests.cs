using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models.Library;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T06_LibraryServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T001_LibraryAddAsync_ShouldReturnTrue(string? id, ReferenceItemType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new ReferenceItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryAddAsync(item, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data, result?.Error?.Message);
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T002_LibraryCheckAsync_ShouldReturnTrue(string? id, ReferenceItemType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new ReferenceItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryCheckAsync(item, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data, result?.Error?.Message);
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T003_LibraryRemoveAsync_ShouldReturnTrue(string? id, ReferenceItemType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new ReferenceItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryRemoveAsync(item, cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data, result?.Error?.Message);
    }


    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T004_LibraryAddAllAsync_ShouldReturnTrue(string? id, ReferenceItemType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new ReferenceItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryAddAllAsync([item], cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data?.All(i => i == true), result?.Error?.Message);
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T005_LibraryCheckAllAsync_ShouldReturnTrue(string? id, ReferenceItemType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new ReferenceItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryCheckAllAsync([item], cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data?.All(i => i == true), result?.Error?.Message);
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", ReferenceItemType.Track)]
    public async Task T006_LibraryRemoveAllAsync_ShouldReturnTrue(string? id, ReferenceItemType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new ReferenceItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryRemoveAllAsync([item], cancellationToken: TestContext.CancellationToken);
        Assert.IsTrue(result?.Data?.All(i => i == true), result?.Error?.Message);
    }
}
