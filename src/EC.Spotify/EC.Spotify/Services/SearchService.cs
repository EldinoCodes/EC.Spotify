using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Searches;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace EC.Spotify.Services;

internal class SearchService(ILogger<SearchService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : ISearchService
{
    private readonly ILogger<SearchService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifySearchUri = "https://api.spotify.com/v1/search";

    public async Task<SpotifyResult<SpotifyPageResult>> SearchAsync(SearchQuery? searchQuery, CancellationToken cancellationToken = default)
    {
        var q = new List<string>();
        if (!string.IsNullOrEmpty(searchQuery?.ArtistName)) q.Add($"artist:{searchQuery.ArtistName}");
        if (!string.IsNullOrEmpty(searchQuery?.AlbumName)) q.Add($"album:{searchQuery.AlbumName}");
        if (!string.IsNullOrEmpty(searchQuery?.TrackName)) q.Add($"track:{searchQuery.TrackName}");
        if (!string.IsNullOrEmpty(searchQuery?.Genre)) q.Add($"genre:{searchQuery.Genre}");
        var searchQueryType = typeof(SearchType);
        var searchTypes = string.Join(",", Enum.GetValues(searchQueryType)
            .Cast<SearchType>()
            .Where(t => searchQuery?.Type.HasFlag(t) ?? false)
            .Select(t => Enum.GetName(searchQueryType, t)?.ToLower())
        );

        var queryParams = new Dictionary<string, string?>()
        {
            { "q", HttpUtility.UrlEncode(string.Join(" ", q)) },
            { "type", HttpUtility.UrlEncode(searchTypes) },
            { "limit", $"{searchQuery?.Limit ?? 20}"},
            { "offset", $"{searchQuery?.Offset ?? 0 }"}
        };

        var uri = SpotifySearchUri.ToUri(queryParams);

        return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult>("get", uri, null, new() { 
            "albums", 
            "artists", 
            "audiobooks", 
            "episodes", 
            "playlists",
            "shows", 
            "tracks" 
        }, cancellationToken: cancellationToken);
    }
}
