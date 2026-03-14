using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models.Searches;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T08_SearchServiceTests
{
    [TestMethod]
    [DataRow("The Beatles", null, null, null, SearchType.Artist)]
    [DataRow(null, "Abbey Road", null, null, SearchType.Album)]
    [DataRow(null, null, "Come Together", null, SearchType.Track)]
    public async Task SearchAsync_ShouldReturnSearchResults(string? artistName, string? albumName, string? trackName, string? genre, SearchType searchType = SearchType.Track)
    {
        if (string.IsNullOrEmpty(artistName) && string.IsNullOrEmpty(albumName) && string.IsNullOrEmpty(trackName) && string.IsNullOrEmpty(genre))
        {
            Assert.Inconclusive("At least one search parameter must be provided.");
            return;
        }
        var sut = Initializer.Resolve<ISearchService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var query = new SearchQuery
        {
            ArtistName = artistName,
            AlbumName = albumName,
            TrackName = trackName,
            Genre = genre,
            Type = searchType
        };

        var result = await sut.SearchAsync(query);
        Assert.IsNotNull(result?.Data);
    }
}
