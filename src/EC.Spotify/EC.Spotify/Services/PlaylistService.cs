using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using EC.Spotify.Models.Playlists;
using EC.Spotify.Models.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace EC.Spotify.Services;

internal class PlaylistService(ILogger<PlaylistService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IPlaylistService
{
    private readonly ILogger<PlaylistService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyPlaylistUri = "https://api.spotify.com/v1/playlists/{0}";
    private const string SpotifyPlaylistItemsUri = "https://api.spotify.com/v1/playlists/{0}/items";
    private const string SpotifyPlaylistImagesUri = "https://api.spotify.com/v1/playlists/{0}/images";

    public async Task<SpotifyResult<Playlist>> PlaylistGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistGetAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyPlaylistUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Playlist>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistGetAsync failed for id: {Id}", id);
            return new SpotifyResult<Playlist> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> PlaylistDetailUpdateAsync(string? id, PlaylistDetail? playlistDetail, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["playlist-modify-public", "playlist-modify-private"]);
            if (error is not null) return new SpotifyResult<bool>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistDetailUpdateAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyPlaylistUri, id);
            var json = playlistDetail.ToJson();
            var data = !string.IsNullOrEmpty(json)
                ? new StringContent(json, Encoding.UTF8, "application/json")
                : null;

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistDetailUpdateAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("PUT", uri, data, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistDetailUpdateAsync failed for id: {Id}", id);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<PlaylistPageResult>> PlaylistItemGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["playlist-read-private"]);
            if (error is not null) return new SpotifyResult<PlaylistPageResult>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemGetAllAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            var uri = string.Format(SpotifyPlaylistItemsUri, id).ToUri(new ()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<PlaylistPageResult>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistItemGetAllAsync failed for id: {Id}", id);
            return new SpotifyResult<PlaylistPageResult> { Error = ex.ToSpotifyError() };
        }
    }    
        

    public async Task<SpotifyResult<bool>> PlaylistItemAddAsync(string? id, ReferenceItem? libraryItem, int? position = default, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemAddAsync called with id: {Id}, item URI: {Uri}", id, libraryItem?.Uri);

            var ret = new SpotifyResult<bool>();

            var res = await PlaylistItemAddAllAsync(id, libraryItem is not null ? [libraryItem] : [], position, cancellationToken: cancellationToken);
            if (res.IsSuccess == true)
                ret.Data = res.Data?.FirstOrDefault() ?? false;

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistItemAddAsync failed for id: {Id}", id);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<List<bool>>> PlaylistItemAddAllAsync(string? id, List<ReferenceItem> libraryItems, int? position = default, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["playlist-modify-public", "playlist-modify-private"]);
            if (error is not null) return new SpotifyResult<List<bool>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemAddAllAsync called with id: {Id}, {Count} items, position: {Position}", id, libraryItems.Count, position);

            var ret = new SpotifyResult<List<bool>>();
            var uri = string.Format(SpotifyPlaylistItemsUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemAddAllAsync requesting URI: {Uri}", uri);

            // imposed cap from spotify of 100 items per request, so we need to chunk the list and make multiple requests if necessary
            SpotifyError? lastError = null;
            foreach (var batch in libraryItems.Chunk(100))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var json = new { Uris = uris, Position = position }.ToJson();
                var data = !string.IsNullOrEmpty(json)
                    ? new StringContent(json, Encoding.UTF8, "application/json")
                    : null;

                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("POST", uri, data, cancellationToken: cancellationToken);
                if (!result.IsSuccess) { lastError = result.Error; continue; }

                ret.Data ??= [];
                if (result.Data is not null)
                    ret.Data.AddRange(result.Data);
            }

            if (ret.Data is null && lastError is not null)
                ret.Error = lastError;

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistItemAddAllAsync failed for id: {Id}", id);
            return new SpotifyResult<List<bool>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<bool>> PlaylistItemRemoveAsync(string? id, ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemRemoveAsync called with id: {Id}, item URI: {Uri}", id, libraryItem?.Uri);

            var ret = new SpotifyResult<bool>();

            var res = await PlaylistItemRemoveAllAsync(id, libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
            if (res.IsSuccess == true)
                ret.Data = res.Data?.FirstOrDefault() ?? false;

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistItemRemoveAsync failed for id: {Id}", id);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<List<bool>>> PlaylistItemRemoveAllAsync(string? id, List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["playlist-modify-public", "playlist-modify-private"]);
            if (error is not null) return new SpotifyResult<List<bool>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemRemoveAllAsync called with id: {Id}, {Count} items", id, libraryItems.Count);

            var ret = new SpotifyResult<List<bool>>();
            var uri = string.Format(SpotifyPlaylistItemsUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistItemRemoveAllAsync requesting URI: {Uri}", uri);

            // imposed cap from spotify of 100 items per request, so we need to chunk the list and make multiple requests if necessary
            SpotifyError? lastError = null;
            foreach (var batch in libraryItems.Chunk(100))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var json = new { Uris = uris }.ToJson();
                var data = !string.IsNullOrEmpty(json)
                    ? new StringContent(json, Encoding.UTF8, "application/json")
                    : null;

                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("delete", uri, data, cancellationToken: cancellationToken);
                if (!result.IsSuccess) { lastError = result.Error; continue; }

                result.Data = [.. Enumerable.Range(0, batch.Length).Select(i => true)];

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }

            if (ret.Data is null && lastError is not null)
                ret.Error = lastError;

            return ret;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistItemRemoveAllAsync failed for id: {Id}", id);
            return new SpotifyResult<List<bool>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<bool>> PlaylistImageAddAsync(string? id, byte[]? imageData, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["ugc-image-upload", "playlist-modify-public", "playlist-modify-private"]);
            if (error is not null) return new SpotifyResult<bool>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistImageAddAsync called with id: {Id}, imageData size: {Size} bytes", id, imageData?.Length ?? 0);

            var uri = string.Format(SpotifyPlaylistImagesUri, id);
            var data = imageData is not null
                ? new StringContent(Convert.ToBase64String(imageData), Encoding.UTF8, "image/jpg")
                : null;

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistImageAddAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, data, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistImageAddAsync failed for id: {Id}", id);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<List<Image>>> PlaylistImageGetAllAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistImageGetAllAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyPlaylistImagesUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlaylistImageGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<List<Image>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaylistImageGetAllAsync failed for id: {Id}", id);
            return new SpotifyResult<List<Image>> { Error = ex.ToSpotifyError() };
        }
    }
}