using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Searches;
using Microsoft.Extensions.Logging;
using System.Web;

namespace EC.Spotify.Services;

internal class SearchService(ILogger<SearchService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), ISearchService
{
    private readonly ILogger<SearchService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;
    private readonly List<string?> _polymorphicTypeNames = spotifyJsonSerializer.GetPolymorphicTypeNames().Select(n => !string.IsNullOrEmpty(n) ? n + "s" : n).ToList() ?? [];


    private const string SpotifySearchUri = "https://api.spotify.com/v1/search";

    public async Task<SpotifyResult<SpotifyPageResult>> SearchAsync(SearchQuery? searchQuery, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        if (searchQuery is null) return GenerateResult<SpotifyPageResult>(ret);

        try
        {
            var q = new List<string>();
            if (!string.IsNullOrEmpty(searchQuery.ArtistName)) q.Add($"artist%3A{searchQuery.ArtistName}");
            if (!string.IsNullOrEmpty(searchQuery.AlbumName)) q.Add($"album%3A{searchQuery.AlbumName}");
            if (!string.IsNullOrEmpty(searchQuery.TrackName)) q.Add($"track%3A{searchQuery.TrackName}");
            if (!string.IsNullOrEmpty(searchQuery.Genre)) q.Add($"genre%3A{searchQuery.Genre}");
            var searchQueryType = typeof(SearchType);
            var searchTypes = string.Join(",", Enum.GetValues(searchQueryType)
                .Cast<SearchType>()
                .Where(t => searchQuery.Type.HasFlag(t))
                .Select(t => Enum.GetName(searchQueryType, t)?.ToLower())
            );

            var queryParams = new Dictionary<string, string?>()
            {
                { "q", HttpUtility.UrlEncode(string.Join(" ", q)) },
                { "type", HttpUtility.UrlEncode(searchTypes) },
                { "limit", $"{searchQuery.Limit ?? 20}"},
                { "offset", $"{searchQuery.Offset ?? 0 }"}
            };

            var uri = BuildUri(SpotifySearchUri, queryParams);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for track");
        }
        return GenerateResult<SpotifyPageResult>(ret, _polymorphicTypeNames);
    }
}
