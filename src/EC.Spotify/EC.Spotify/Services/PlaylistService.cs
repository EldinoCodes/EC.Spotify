using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Library;
using EC.Spotify.Models.Playlists;
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
    private const string SpotifyMyPlaylistUri = "https://api.spotify.com/v1/me/playlists";
    private const string SpotifyPlaylistItemsUri = "https://api.spotify.com/v1/playlists/{0}/items";
    private const string SpotifyPlaylistImagesUri = "https://api.spotify.com/v1/playlists/{0}/images";

    public async Task<SpotifyResult<SpotifyPageResult>> MyPlaylistGetAllAsync(CancellationToken cancellationToken = default)
    {
        var uri = SpotifyMyPlaylistUri;

        return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult>("get", uri, cancellationToken: cancellationToken);
    }


    public async Task<SpotifyResult<Playlist>> PlaylistGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(SpotifyPlaylistUri, id);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<Playlist>("get", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<SpotifyPageResult>> PlaylistItemGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<SpotifyPageResult>();

        var scopes = new List<string>()
        {
            "playlist-modify-private"
        }.Except(_options.Scopes ?? []).ToList();
        if (scopes.Count > 0)
        {
            ret.Error = new() { Message = "Missing required scope" };
            return ret;
        }

        var uri = string.Format(SpotifyPlaylistItemsUri, id).ToUri(new ()
        {
            { "limit", $"{limit}"},
            { "offset", $"{offset }"}
        });
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult>("get", uri, cancellationToken: cancellationToken);
    }

    public async Task<SpotifyResult<bool>> PlaylistDetailUpdateAsync(string? id, PlaylistDetail? playlistDetail, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();
        var scopes = new List<string>()
        {
            "playlist-modify-public",
            "playlist-modify-private"
        }.Except(_options.Scopes ?? []).ToList();
        if (scopes.Count > 0)
        {
            ret.Error = new() { Message = "Missing required scope" };
            return ret;
        }

        var uri = string.Format(SpotifyPlaylistUri, id);
        
        var json = playlistDetail.ToJson();
        var data = !string.IsNullOrEmpty(json)
            ? new StringContent(json, Encoding.UTF8, "application/json")
            : null;

        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("PUT", uri, data, cancellationToken: cancellationToken);
    }
        

    public async Task<SpotifyResult<bool>> PlaylistItemAddAsync(string? id, ReferenceItem? libraryItem, int? position = default, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await PlaylistItemAddAllAsync(id, libraryItem is not null ? [libraryItem] : [], position, cancellationToken: cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> PlaylistItemAddAllAsync(string? id, List<ReferenceItem> libraryItems, int? position = default, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<List<bool>>();
        var scopes = new List<string>()
        {
            "playlist-modify-public",
            "playlist-modify-private"
        }.Except(_options.Scopes ?? []).ToList();
        if (scopes.Count > 0)
        {
            ret.Error = new() { Message = "Missing required scope" };
            return ret;
        }
        try
        {
            var uri = string.Format(SpotifyPlaylistItemsUri, id);

            // imposed cap from spotify of 100 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(100))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var json = new { Uris = uris, Position = position }.ToJson();
                var data = !string.IsNullOrEmpty(json)
                    ? new StringContent(json, Encoding.UTF8, "application/json")
                    : null;
                
                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("POST", uri, data, cancellationToken: cancellationToken);
                if (!result.IsSuccess) continue;

                result.Data = [.. Enumerable.Range(0, batch.Length).Select(i => true)];

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding playlist items");
        }
        return ret;
    }

    public async Task<SpotifyResult<bool>> PlaylistItemRemoveAsync(string? id, ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var res = await PlaylistItemRemoveAllAsync(id, libraryItem is not null ? [libraryItem] : [], cancellationToken: cancellationToken);
        if (res.IsSuccess == true)
            ret.Data = res.Data?.FirstOrDefault() ?? false;

        return ret;
    }
    public async Task<SpotifyResult<List<bool>>> PlaylistItemRemoveAllAsync(string? id, List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<List<bool>>();
        var scopes = new List<string>()
        {
            "playlist-modify-public",
            "playlist-modify-private"
        }.Except(_options.Scopes ?? []).ToList();
        if (scopes.Count > 0)
        {
            ret.Error = new() { Message = "Missing required scope" };
            return ret;
        }
        try
        {
            var uri = string.Format(SpotifyPlaylistItemsUri, id);

            // imposed cap from spotify of 100 items per request, so we need to chunk the list and make multiple requests if necessary
            foreach (var batch in libraryItems.Chunk(100))
            {
                var uris = batch.Select(x => x.Uri).ToList();
                var json = new { Uris = uris }.ToJson();
                var data = !string.IsNullOrEmpty(json)
                    ? new StringContent(json, Encoding.UTF8, "application/json")
                    : null;
                
                var result = await _spotifyProvider.ExecuteSpotifyResultAsync<List<bool>>("delete", uri, data, cancellationToken: cancellationToken);
                if (!result.IsSuccess) continue;

                result.Data = [.. Enumerable.Range(0, batch.Count()).Select(i => true)];

                ret.Data ??= [];
                ret.Data.AddRange(result.Data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing playlist items");
        }
        return ret;
    }

    public async Task<SpotifyResult<bool>> PlaylistImageAddAsync(string? id, byte[]? imageData, CancellationToken cancellationToken = default)
    {
        var ret = new SpotifyResult<bool>();

        var scopes = new List<string>()
        {
            "ugc-image-upload",
            "playlist-modify-public",
            "playlist-modify-private"
        }.Except(_options.Scopes ?? []).ToList();
        if (scopes.Count > 0) 
        {
            ret.Error = new() { Message = "Missing required scope" };
            return ret;
        }
        
        var uri = string.Format(SpotifyPlaylistImagesUri, id);
        var data = imageData is not null 
            ? new StringContent(Convert.ToBase64String(imageData), Encoding.UTF8, "image/jpg")
            : null;

        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, data, cancellationToken: cancellationToken);
    }
}