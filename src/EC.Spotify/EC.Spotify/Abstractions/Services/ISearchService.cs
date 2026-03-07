using EC.Spotify.Models;
using EC.Spotify.Models.Searches;

namespace EC.Spotify.Abstractions.Services;

public interface ISearchService
{
    /// <summary>
    /// Performs an asynchronous search on Spotify using the specified query parameters.
    /// </summary>
    /// <param name="searchQuery">The search criteria to use for querying Spotify. If null, a default search may be performed or no results will
    /// be returned, depending on implementation.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the search operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="SpotifyResult{SearchResult}"/> with the search results from Spotify.</returns>
    Task<SpotifyResult<SearchResult>> SearchAsync(SearchQuery? searchQuery, CancellationToken cancellationToken = default);
}
