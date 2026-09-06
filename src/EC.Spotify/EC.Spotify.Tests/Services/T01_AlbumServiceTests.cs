using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T01_AlbumServiceTests
{
    public TestContext TestContext { get; set; }

    // Existing tests
    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task T001_AlbumGetAsync_ShouldReturnAlbum(string? id)
    {
        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AlbumGetAsync(id, cancellationToken: TestContext.CancellationToken);        
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task T002_AlbumTrackGetAllAsync_ShouldReturnTracks(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AlbumTrackGetAllAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task T003_AlbumGetRawAsync_ShouldReturnJson(string? id)
    {
        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AlbumGetRawAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    [TestMethod]
    [DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    public async Task T004_AlbumTrackGetAllRawAsync_ShouldReturnJson(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var sut = Initializer.Resolve<IAlbumService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.AlbumTrackGetAllRawAsync(id, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }

    //[TestMethod]
    //[DataRow("7a7arAXDE0BiaMgHLhdjGF")]
    //public async Task T004_MyAlbumContainsAsync_ShouldCheckIfAlbumIsSaved(string albumId)
    //{
    //    var sut = Initializer.Resolve<IAlbumService>();
    //    ArgumentNullException.ThrowIfNull(sut, nameof(sut));

    //    var result = await sut.MyAlbumContainsAsync([albumId], cancellationToken: TestContext.CancellationToken);
    //    Assert.IsNotNull(result?.Data, result?.Error?.Message);
    //    Assert.IsTrue(result.Data?.Count == 1, "Should return one boolean for one album ID");
    //}
}
