using EC.Spotify.Abstractions.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class UserService(ILogger<UserService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider)
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyMyAlbumsUri = "https://api.spotify.com/v1/me/albums";

    private const string SpotifyMyLibraryUri = "https://api.spotify.com/v1/me/library";
    private const string SpotifyMyLibraryContainsUri = "https://api.spotify.com/v1/me/library/contains";

/*
    public async Task<SpotifyResult<SpotifyPageResult>> MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        var requiredScopes = new List<string>() { "user-library-read" };
        var missingScopes = _options.Scopes.Except(requiredScopes);
        if (missingScopes.Any()) throw new Exception($"Missing required scopes: {string.Join(", ", missingScopes)}");

        var queryParams = new Dictionary<string, string?>()
        {
            { "limit", $"{limit}"},
            { "offset", $"{offset }"}
        };
        var uri = BuildUri(SpotifyMyAlbumsUri, queryParams);
        var header = await GetAuthorizationHeaderAsync(cancellationToken);
        var ret = await _httpSpotifyProvider.ExecuteAsync<SpotifyPageResult>("get", uri, null, header, null, cancellationToken);

        return ret;
    }


    public async Task<SpotifyResult<bool>> LibraryCheckAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await LibraryCheckAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken);
        if (res.IsSuccess)
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
                var uri = BuildUri(SpotifyMyLibraryContainsUri, queryParams);
                var header = await GetAuthorizationHeaderAsync(cancellationToken);
                var result = await _httpSpotifyProvider.ExecuteAsync<List<bool>>("get", uri, null, header, null, cancellationToken);
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

        var res = await LibraryAddAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken);
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
                var uri = BuildUri(SpotifyMyLibraryUri, queryParams);
                var header = await GetAuthorizationHeaderAsync(cancellationToken);
                var result = await _httpSpotifyProvider.ExecuteAsync<List<bool>>("put", uri, null, header, null, cancellationToken);
                if (!result.IsSuccess) continue;

                result.Data = [.. Enumerable.Range(0, batch.Count()).Select(i => true)];

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

        var res = await LibraryRemoveAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken);
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
                var uri = BuildUri(SpotifyMyLibraryUri, queryParams);
                var header = await GetAuthorizationHeaderAsync(cancellationToken);
                var result = await _httpSpotifyProvider.ExecuteAsync<List<bool>>("delete", uri, null, header, null, cancellationToken);
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
*/
}
