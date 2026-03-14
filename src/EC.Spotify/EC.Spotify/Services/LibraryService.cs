using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using Microsoft.Extensions.Logging;
using System.Web;

namespace EC.Spotify.Services;

internal class LibraryService(ILogger<LibraryService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), ILibraryService
{
    private readonly ILogger<LibraryService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifyLibraryUri = "https://api.spotify.com/v1/me/library";
    private const string SpotifyLibraryContainsUri = "https://api.spotify.com/v1/me/library/contains";

    public async Task<SpotifyResult<bool>> LibraryCheckAsync(LibraryItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await LibraryCheckAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> LibraryCheckAllAsync(List<LibraryItem> libraryItems, CancellationToken cancellationToken = default)
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
                var uri = BuildUri(SpotifyLibraryContainsUri, queryParams);
                var header = await GetAuthorizationHeaderAsync(cancellationToken);
                var res = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);

                var result = GenerateResult<List<bool>>(res);
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

    public async Task<SpotifyResult<bool>> LibraryAddAsync(LibraryItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await LibraryAddAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> LibraryAddAllAsync(List<LibraryItem> libraryItems, CancellationToken cancellationToken = default)
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
                var uri = BuildUri(SpotifyLibraryUri, queryParams);
                var header = await GetAuthorizationHeaderAsync(cancellationToken);
                var res = await _httpProvider.ExecuteAsync("put", uri, null, header, cancellationToken);

                var result = GenerateResult<List<bool>>(res);
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

    public async Task<SpotifyResult<bool>> LibraryRemoveAsync(LibraryItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await LibraryRemoveAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> LibraryRemoveAllAsync(List<LibraryItem> libraryItems, CancellationToken cancellationToken = default)
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
                var uri = BuildUri(SpotifyLibraryUri, queryParams);
                var header = await GetAuthorizationHeaderAsync(cancellationToken);
                var res = await _httpProvider.ExecuteAsync("delete", uri, null, header, cancellationToken);

                var result = GenerateResult<List<bool>>(res);
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
