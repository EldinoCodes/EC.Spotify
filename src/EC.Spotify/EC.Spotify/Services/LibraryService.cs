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
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("LibraryCheckAsync called with item URI: {Uri}", libraryItem?.Uri);

            var ret = new SpotifyResult<bool>();

            var res = await LibraryCheckAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
            if (res.IsSuccess == true)
                ret.Data = res.Data?.FirstOrDefault() ?? false;

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LibraryCheckAsync failed for item URI: {Uri}", libraryItem?.Uri);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<List<bool>>> LibraryCheckAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-read", "user-follow-read", "playlist-read-private"]);
            if (error is not null) return new SpotifyResult<List<bool>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("LibraryCheckAllAsync called with {Count} items", libraryItems.Count);

            var ret = new SpotifyResult<List<bool>>();

            // imposed cap from spotify of 40 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(40))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var queryParams = new Dictionary<string, string?>()
                {
                    { "uris", string.Join(",", uris) }
                };
                var uri = SpotifyLibraryContainsUri.ToUri(queryParams);

                if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug("LibraryCheckAllAsync requesting URI: {Uri}", uri);

                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("get", uri, cancellationToken: cancellationToken);
                if (!result.IsSuccess) return result;
                if (result.Data is null) continue;

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LibraryCheckAllAsync failed");
            return new SpotifyResult<List<bool>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<bool>> LibraryAddAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("LibraryAddAsync called with item URI: {Uri}", libraryItem?.Uri);

            var ret = new SpotifyResult<bool>();

            var res = await LibraryAddAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
            if (res.IsSuccess == true)
                ret.Data = res.Data?.FirstOrDefault() ?? false;

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LibraryAddAsync failed for item URI: {Uri}", libraryItem?.Uri);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<List<bool>>> LibraryAddAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-modify", "user-follow-modify", "playlist-modify-public"]);
            if (error is not null) return new SpotifyResult<List<bool>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("LibraryAddAllAsync called with {Count} items", libraryItems.Count);

            var ret = new SpotifyResult<List<bool>>();

            // imposed cap from spotify of 40 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(40))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var queryParams = new Dictionary<string, string?>()
                {
                    { "uris", string.Join(",", uris) }
                };
                var uri = SpotifyLibraryUri.ToUri(queryParams);

                if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug("LibraryAddAllAsync requesting URI: {Uri}", uri);

                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("put", uri, cancellationToken: cancellationToken);
                if (!result.IsSuccess) return result;

                result.Data = [..Enumerable.Range(0, batch.Count()).Select(i => true)];

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LibraryAddAllAsync failed");
            return new SpotifyResult<List<bool>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<bool>> LibraryRemoveAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("LibraryRemoveAsync called with item URI: {Uri}", libraryItem?.Uri);

            var ret = new SpotifyResult<bool>();

            var res = await LibraryRemoveAllAsync(libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
            if (res.IsSuccess == true)
                ret.Data = res.Data?.FirstOrDefault() ?? false;

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LibraryRemoveAsync failed for item URI: {Uri}", libraryItem?.Uri);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<List<bool>>> LibraryRemoveAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-modify", "user-follow-modify", "playlist-modify-public"]);
            if (error is not null) return new SpotifyResult<List<bool>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("LibraryRemoveAllAsync called with {Count} items", libraryItems.Count);

            var ret = new SpotifyResult<List<bool>>();

            // imposed cap from spotify of 40 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(40))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var queryParams = new Dictionary<string, string?>()
                {
                    { "uris", string.Join(",", uris) }
                };
                var uri = SpotifyLibraryUri.ToUri(queryParams);

                if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug("LibraryRemoveAllAsync requesting URI: {Uri}", uri);

                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("delete", uri, cancellationToken: cancellationToken);
                if (!result.IsSuccess) return result;

                result.Data = [.. Enumerable.Range(0, batch.Count()).Select(i => true)];

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LibraryRemoveAllAsync failed");
            return new SpotifyResult<List<bool>> { Error = ex.ToSpotifyError() };
        }
    }
}
