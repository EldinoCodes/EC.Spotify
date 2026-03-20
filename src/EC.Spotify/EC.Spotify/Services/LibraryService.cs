using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace EC.Spotify.Services;

internal class LibraryService(ILogger<LibraryService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : ILibraryService
{
    private readonly ILogger<LibraryService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyLibraryUri = "https://api.spotify.com/v1/me/library";
    private const string SpotifyLibraryContainsUri = "https://api.spotify.com/v1/me/library/contains";

    public async Task<SpotifyResult<bool>> LibraryCheckAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await LibraryCheckAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> LibraryCheckAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<List<bool>>();

        try
        {
            // imposed cap from spotify of 40 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(40))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var queryParams = new Dictionary<string, string?>()
                {
                    { "uris", HttpUtility.UrlEncode(string.Join(",", uris)) }
                };
                var uri = SpotifyLibraryContainsUri.ToUri(queryParams);
                
                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("get", uri, cancellationToken: cancellationToken);
                if (!result.IsSuccess) continue;
                if (result.Data is null) continue;

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking library items");
        }
        return ret;
    }

    public async Task<SpotifyResult<bool>> LibraryAddAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await LibraryAddAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> LibraryAddAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<List<bool>>();

        try
        {
            // imposed cap from spotify of 40 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(40))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var queryParams = new Dictionary<string, string?>()
                {
                    { "uris", HttpUtility.UrlEncode(string.Join(",", uris)) }
                };
                var uri = SpotifyLibraryUri.ToUri(queryParams);
                
                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("put", uri, cancellationToken: cancellationToken);
                if (!result.IsSuccess) continue;
                
                result.Data = [..Enumerable.Range(0, batch.Count()).Select(i => true)];
                
                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding library items");
        }
        return ret;
    }

    public async Task<SpotifyResult<bool>> LibraryRemoveAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await LibraryRemoveAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> LibraryRemoveAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<List<bool>>();

        try
        {
            // imposed cap from spotify of 40 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(40))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var queryParams = new Dictionary<string, string?>()
                {
                    { "uris", HttpUtility.UrlEncode(string.Join(",", uris)) }
                };
                var uri = SpotifyLibraryUri.ToUri(queryParams);
                
                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("delete", uri, cancellationToken: cancellationToken);
                if (!result.IsSuccess) continue;

                result.Data = [.. Enumerable.Range(0, batch.Count()).Select(i => true)];

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking library items");
        }
        return ret;
    }
}
