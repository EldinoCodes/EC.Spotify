using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Models;
using EC.Spotify.Models.Players;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EC.Spotify.Services;

internal sealed class PlayerService(ILogger<PlayerService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), IPlayerService
{
    private readonly ILogger<PlayerService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;
    private readonly ISpotifyJsonSerializer _spotifyJsonSerializer = spotifyJsonSerializer;

    private const string PlayerUri = "https://api.spotify.com/v1/me/player";

    public async Task<SpotifyResult<PlayerQueue>> QueueGetAsync(CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = $"{PlayerUri}/queue";
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting queue");
        }
        return GenerateResult<PlayerQueue>(ret);
    }
    public async Task<SpotifyResult<bool>> QueueAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>() { { "uri", trackId } };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = BuildUri($"{PlayerUri}/queue", queryParams);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("post", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding track to queue");
        }
        return GenerateResult<bool>(ret);
    }

    public async Task<SpotifyResult<List<Device>>> DeviceGetAllAsync(CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = $"{PlayerUri}/devices";
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting devices");
        }
        return GenerateResult<List<Device>>(ret, "devices");
    }
    public async Task<SpotifyResult<bool>> TransferAsync(string? deviceId, bool play = false, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            var json = _spotifyJsonSerializer.Serialize(new { device_ids = new[] { deviceId }, play });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ret = await _httpProvider.ExecuteAsync("put", PlayerUri, data, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding track to queue");
        }
        return GenerateResult<bool>(ret);
    }

    public async Task<SpotifyResult<bool>> PlayerPlayAsync(string? deviceId, List<string>? trackUris, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = BuildUri($"{PlayerUri}/play", queryParams);
            var json = _spotifyJsonSerializer.Serialize(new { uris = trackUris });
            var content = trackUris?.Count > 0 && !string.IsNullOrEmpty(json)
                ? new StringContent(json, Encoding.UTF8, "application/json")
                : null;
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("put", uri, content, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error playing track");
        }
        return GenerateResult<bool>(ret);
    }
    public async Task<SpotifyResult<bool>> PlayerPauseAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);
            var uri = BuildUri($"{PlayerUri}/pause", queryParams);
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("put", uri, null, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pausing track");
        }
        return GenerateResult<bool>(ret);
    }
    public async Task<SpotifyResult<bool>> PlayerNextAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;        
        try
        {
            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = BuildUri($"{PlayerUri}/next", queryParams);
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("post", uri, null, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error skipping track");
        }
        return GenerateResult<bool>(ret);
    }
    public async Task<SpotifyResult<bool>> PlayerPreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;        
        try
        {
            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = BuildUri($"{PlayerUri}/previous", queryParams);
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("post", uri, null, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error skipping track");
        }
        return GenerateResult<bool>(ret);
    }

    public async Task<SpotifyResult<bool>> PlayerSeekAsync(int positionMs, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;        
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "position_ms", positionMs.ToString() }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = BuildUri($"{PlayerUri}/seek", queryParams);
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("put", uri, null, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeking track");
        }
        return GenerateResult<bool>(ret);
    }
    public async Task<SpotifyResult<bool>> PlayerRepeatAsync(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;        
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "state", playerRepeatMode.ToString().ToLower() }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);
            var uri = BuildUri($"{PlayerUri}/repeat", queryParams);
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("put", uri, null, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing repeat mode");
        }
        return GenerateResult<bool>(ret);
    }
    public async Task<SpotifyResult<bool>> PlayerShuffleAsync(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "state", playerShuffleMode == PlayerShuffleMode.On ? "true" : "false" }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);
            var uri = BuildUri($"{PlayerUri}/shuffle", queryParams);
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("put", uri, null, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing shuffle mode");
        }
        return GenerateResult<bool>(ret);
    }
    public async Task<SpotifyResult<bool>> PlayerVolumeAsync(int volumePercent, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        string? ret = default;        
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "volumePercent", volumePercent.ToString() }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = BuildUri($"{PlayerUri}/volume", queryParams);
            var headers = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("put", uri, null, headers, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing volume");
        }
        return GenerateResult<bool>(ret);
    }
    
}