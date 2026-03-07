using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Searches;
using Microsoft.Extensions.Logging;
using System.Web;

namespace EC.Spotify.Services;

internal class SearchService(ILogger<SearchService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), ISearchService
{
    private readonly ILogger<SearchService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifySearchUri = "https://api.spotify.com/v1/search"; 

    public async Task<SpotifyResult<SearchResult>> SearchAsync(SearchQuery? searchQuery, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        if (searchQuery is null) return GenerateResult<SearchResult>(ret);

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

            /*
            ret = ret ?? new SearchResult();
            while (true)
            {
                
                ret.Bind(res);

                var errorResult = res.FromJson<ErrorResult>();
                if (errorResult?.Error is not null)
                {
                    if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();
            
                    throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
                }
            
                var tmpResult = res.FromJson<SearchResult>();
                if (tmpResult is null) break;

                if (tmpResult.Albums?.Items is not null)
                {
                    ret.Albums ??= new SpotifyPageResult<Album>();
                    ret.Albums.Items.AddRange(tmpResult.Albums.Items);
                    if (!string.IsNullOrEmpty(ret.Albums.Next)) nextLinks.Enqueue(ret.Albums.Next);
                }
                if (tmpResult.Artists?.Items is not null)
                {
                    ret.Artists ??= new SpotifyPageResult<Artist>();
                    ret.Artists.Items.AddRange(tmpResult.Artists.Items);
                    if (!string.IsNullOrEmpty(ret.Artists.Next)) nextLinks.Enqueue(ret.Artists.Next);
                }
                if (tmpResult.Audiobooks?.Items is not null)
                {
                    ret.Audiobooks ??= new SpotifyPageResult<Audiobook>();
                    ret.Audiobooks.Items.AddRange(tmpResult.Audiobooks.Items);
                    if (!string.IsNullOrEmpty(ret.Audiobooks.Next)) nextLinks.Enqueue(ret.Audiobooks.Next);
                }
                if (tmpResult.Episodes?.Items is not null)
                {
                    ret.Episodes ??= new SpotifyPageResult<Episode>();
                    ret.Episodes.Items.AddRange(tmpResult.Episodes.Items);
                    if (!string.IsNullOrEmpty(ret.Episodes.Next)) nextLinks.Enqueue(ret.Episodes.Next);
                }
                if (tmpResult.Playlists?.Items is not null)
                {
                    ret.Playlists ??= new SpotifyPageResult<Playlist>();
                    ret.Playlists.Items.AddRange(tmpResult.Playlists.Items);
                    if (!string.IsNullOrEmpty(ret.Playlists.Next)) nextLinks.Enqueue(ret.Playlists.Next);
                }
                if (tmpResult.Shows?.Items is not null)
                {
                    ret.Shows ??= new SpotifyPageResult<Show>();
                    ret.Shows.Items.AddRange(tmpResult.Shows.Items);
                    if (!string.IsNullOrEmpty(ret.Shows.Next)) nextLinks.Enqueue(ret.Shows.Next);
                }
                if (tmpResult.Tracks?.Items is not null)
                {
                    ret.Tracks ??= new SpotifyPageResult<Track>();
                    ret.Tracks.Items.AddRange(tmpResult.Tracks.Items);
                    if (!string.IsNullOrEmpty(ret.Tracks.Next)) nextLinks.Enqueue(ret.Tracks.Next);
                }
                uri = nextLinks.Count > 0 ? nextLinks.Dequeue() : null;
                if (string.IsNullOrEmpty(uri)) break;
            }
            */
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for track");
        }
        return GenerateResult<SearchResult>(ret);
    }
}
