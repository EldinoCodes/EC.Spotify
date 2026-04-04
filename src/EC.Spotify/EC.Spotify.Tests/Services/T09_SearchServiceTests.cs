using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models.Searches;

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

        var searchQuery = new SearchQuery
        {
            Query = query,
            Type = searchType
        };

        var result = await sut.SearchAsync(searchQuery, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(result?.Data, result?.Error?.Message);
    }
}
