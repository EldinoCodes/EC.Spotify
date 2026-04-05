using EC.Spotify.Abstractions.Models;
using EC.Spotify.Enums;
using EC.Spotify.Models;

namespace EC.Spotify.Abstractions.Services;

public interface ISearchService
{
    /// <summary>
    /// Performs an asynchronous search against the Spotify catalog using the specified query and search type.
    /// </summary>
    /// <param name="query">The search query string to use. Can be null or empty to return no results.</param>
    /// <param name="searchType">The type of item to search for, such as album, artist, or track. If null, a default search type may be used.</param>
    /// <param name="limit">The maximum number of items to return. Must be a non-negative integer. The default is 5.</param>
    /// <param name="offset">The index of the first item to return. Must be a non-negative integer. The default is 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SpotifyResult with a polymorphic
    /// page of search results matching the query and search type. The result may be empty if no items match the search
    /// criteria.</returns>
    Task<SpotifyResult<SpotifyPageResult<IPolymorphicItem>>> SearchAsync(string? query, SearchType? searchType = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default);
}
