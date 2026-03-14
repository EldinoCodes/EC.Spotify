using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models.Library;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T06_LibraryServiceTests
{
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", LibraryType.Track)]
    public async Task T001_LibraryAddAsync_ShouldReturnTrue(string? id, LibraryType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new LibraryItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryAddAsync(item);
        Assert.IsTrue(result?.Data);
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", LibraryType.Track)]
    public async Task T002_LibraryCheckAsync_ShouldReturnTrue(string? id, LibraryType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new LibraryItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryCheckAsync(item);
        Assert.IsTrue(result?.Data);
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", LibraryType.Track)]
    public async Task T003_LibraryRemoveAsync_ShouldReturnTrue(string? id, LibraryType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new LibraryItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryRemoveAsync(item);
        Assert.IsTrue(result?.Data);
    }


    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", LibraryType.Track)]
    public async Task T004_LibraryAddAllAsync_ShouldReturnTrue(string? id, LibraryType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new LibraryItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryAddAllAsync([item]);
        Assert.IsTrue(result?.Data?.All(i => i == true));
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", LibraryType.Track)]
    public async Task T005_LibraryCheckAllAsync_ShouldReturnTrue(string? id, LibraryType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new LibraryItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryCheckAllAsync([item]);
        Assert.IsTrue(result?.Data?.All(i => i == true));
    }
    [TestMethod]
    [DataRow("4tjcBY787A2ZkRJpPIsGIS", LibraryType.Track)]
    public async Task T006_LibraryRemoveAllAsync_ShouldReturnTrue(string? id, LibraryType libraryType)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var item = new LibraryItem { Id = id, Type = libraryType };

        var sut = Initializer.Resolve<ILibraryService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.LibraryRemoveAllAsync([item]);
        Assert.IsTrue(result?.Data?.All(i => i == true));
    }
}
