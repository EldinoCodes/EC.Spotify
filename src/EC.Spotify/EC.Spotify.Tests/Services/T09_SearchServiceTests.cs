using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T09_SearchServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow("The Beatles", SearchType.Artist)]
    [DataRow("Abbey Road", SearchType.Album)]
    [DataRow("Come Together", SearchType.Track)]
    public async Task T001_SearchAsync_ShouldReturnSearchResults(string? query, SearchType searchType = SearchType.Track)
    {
        if (string.IsNullOrEmpty(query))
        {
            Assert.Inconclusive("At least one search parameter must be provided.");
            return;
        }
        var sut = Initializer.Resolve<ISearchService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.SearchAsync(query, searchType, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }

    [TestMethod]
    [DataRow("The Beatles", SearchType.Artist)]
    [DataRow("Abbey Road", SearchType.Album)]
    [DataRow("Come Together", SearchType.Track)]
    public async Task T002_SearchRawAsync_ShouldReturnJson(string? query, SearchType searchType = SearchType.Track)
    {
        if (string.IsNullOrEmpty(query))
        {
            Assert.Inconclusive("At least one search parameter must be provided.");
            return;
        }
        var sut = Initializer.Resolve<ISearchService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var result = await sut.SearchRawAsync(query, searchType, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result, "Expected raw JSON response");
    }
}
