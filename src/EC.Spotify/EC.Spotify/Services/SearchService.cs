using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Searches;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class SearchService(ILogger<SearchService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : ISearchService
{
    private readonly ILogger<SearchService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifySearchUri = "https://api.spotify.com/v1/search";

    public async Task<SpotifyResult<SpotifyPolymorphicPageResult>> SearchAsync(SearchQuery? searchQuery, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<SpotifyPolymorphicPageResult>();
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("SearchAsync called with query: {Query}, type: {Type}, limit: {Limit}, offset: {Offset}",
                    searchQuery?.Query, searchQuery?.Type, searchQuery?.Limit, searchQuery?.Offset);

            var limit = searchQuery?.Limit;
            var offset = searchQuery?.Offset;

            var q = new List<string>();
            if (!string.IsNullOrEmpty(searchQuery?.Query)) q.Add(searchQuery.Query);
            var searchQueryType = typeof(SearchType);
            var searchTypes = string.Join(",", Enum.GetValues(searchQueryType)
                .Cast<SearchType>()
                .Where(t => searchQuery?.Type.HasFlag(t) ?? false)
                .Select(t => Enum.GetName(searchQueryType, t)?.ToLower())
            );

            var queryParams = new Dictionary<string, string?>()
            {
                { "q", string.Join(" ", q) },
                { "type", searchTypes },
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            };

            var uri = SpotifySearchUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("SearchAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPolymorphicPageResult>("get", uri, null, [
                "albums",
                "artists",
                "audiobooks",
                "episodes",
                "playlists",
                "shows",
                "tracks"
            ], cancellationToken: cancellationToken);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching Spotify");

            ret.Error = ex.ToSpotifyError();
        }
        return ret;
    }
}
