using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Players;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace EC.Spotify.Services;

internal sealed class PlayerService(ILogger<PlayerService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IPlayerService
{
    private readonly ILogger<PlayerService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string PlayerUri = "https://api.spotify.com/v1/me/player";

    public async Task<SpotifyResult<PlayerQueue>> QueueGetAsync(CancellationToken cancellationToken = default)
    {
        var uri = $"{PlayerUri}/queue";
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<PlayerQueue>("get", uri, null, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> QueueAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>() { { "uri", trackId } };
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/queue".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("post", uri, null, cancellationToken: cancellationToken);
    }

    public async Task<SpotifyResult<List<Device>>> DeviceGetAllAsync(CancellationToken cancellationToken = default)
    {
        var uri = $"{PlayerUri}/devices";
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<List<Device>>("get", uri, null, ["devices"], cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> TransferAsync(string? deviceId, bool play = false, CancellationToken cancellationToken = default)
    {        
        var json = new { device_ids = new[] { deviceId }, play }.ToJson();
        var data = !string.IsNullOrEmpty(json) 
            ? new StringContent(json, Encoding.UTF8, "application/json")
            : null;
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", PlayerUri, data, cancellationToken: cancellationToken);
    }

    public async Task<SpotifyResult<bool>> PlayerPlayAsync(string? deviceId, List<string>? trackUris, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/play".ToUri(queryParams);
        var json = new { uris = trackUris }.ToJson();
        var data = trackUris?.Count > 0 && !string.IsNullOrEmpty(json)
            ? new StringContent(json, Encoding.UTF8, "application/json")
            : null;        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, data, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> PlayerPauseAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/pause".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> PlayerNextAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/next".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("post", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> PlayerPreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/previous".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("post", uri, cancellationToken: cancellationToken);
    }

    public async Task<SpotifyResult<bool>> PlayerSeekAsync(int positionMs, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>()
        {
            { "position_ms", positionMs.ToString() }
        };
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/seek".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> PlayerRepeatAsync(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)
    {        
        var queryParams = new Dictionary<string, string?>()
        {
            { "state", playerRepeatMode.ToString().ToLower() }
        };
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);
        
        var uri = $"{PlayerUri}/repeat".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> PlayerShuffleAsync(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>()
        {
            { "state", playerShuffleMode == PlayerShuffleMode.On ? "true" : "false" }
        };
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/shuffle".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<bool>> PlayerVolumeAsync(int volumePercent, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>()
        {
            { "volumePercent", volumePercent.ToString() }
        };
        if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

        var uri = $"{PlayerUri}/volume".ToUri(queryParams);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
    }    
}