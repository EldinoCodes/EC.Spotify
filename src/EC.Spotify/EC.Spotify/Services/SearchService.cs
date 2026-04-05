using EC.Spotify.Abstractions.Models;
using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class SearchService(ILogger<SearchService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : ISearchService
{
    private readonly ILogger<SearchService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifySearchUri = "https://api.spotify.com/v1/search";

    public async Task<SpotifyResult<SpotifyPageResult<IPolymorphicItem>>> SearchAsync(string? query, SearchType? searchType = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("SearchAsync called with query: {Query}, type: {Type}, limit: {Limit}, offset: {Offset}", query, searchType, limit, offset);

            var q = new List<string>();
            if (!string.IsNullOrEmpty(query)) q.Add(query);
            var searchQueryType = typeof(SearchType);
            var searchTypes = string.Join(",", Enum.GetValues(searchQueryType)
                .Cast<SearchType>()
                .Where(t => searchType?.HasFlag(t) ?? false)
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

            var res = await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<IPolymorphicItem>>("get", uri, null, [
                "albums",
                "artists",
                "audiobooks",
                "episodes",
                "playlists",
                "shows",
                "tracks"
            ], cancellationToken: cancellationToken);

            res.Data?.Next = res.Data.Next?.Replace("type", "searchType");
            res.Data?.Previous = res.Data.Previous?.Replace("type", "searchType");

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching Spotify");
            return new SpotifyResult<SpotifyPageResult<IPolymorphicItem>>()
            {
                Error = ex.ToSpotifyError()
            };
        }
    }
}
