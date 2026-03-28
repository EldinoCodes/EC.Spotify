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
    private const string PlayerQueueUri = "https://api.spotify.com/v1/me/player/queue";
    private const string PlayerDevicesUri = "https://api.spotify.com/v1/me/player/devices";
    private const string PlayerCurrentlyPlayingUri = "https://api.spotify.com/v1/me/player/currently-playing";
    private const string PlayerPlayUri = "https://api.spotify.com/v1/me/player/play";
    private const string PlayerPauseUri = "https://api.spotify.com/v1/me/player/pause";
    private const string PlayerNextUri = "https://api.spotify.com/v1/me/player/next";
    private const string PlayerPreviousUri = "https://api.spotify.com/v1/me/player/previous";
    private const string PlayerSeekUri = "https://api.spotify.com/v1/me/player/seek";
    private const string PlayerRepeatUri = "https://api.spotify.com/v1/me/player/repeat";
    private const string PlayerShuffleUri = "https://api.spotify.com/v1/me/player/shuffle";
    private const string PlayerVolumeUri = "https://api.spotify.com/v1/me/player/volume";

    public async Task<SpotifyResult<PlayerQueue>> QueueGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-state", "user-read-currently-playing"]);
            if (error is not null) return new SpotifyResult<PlayerQueue> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("QueueGetAsync requesting URI: {Uri}", PlayerQueueUri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<PlayerQueue>("get", PlayerQueueUri, null, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "QueueGetAsync failed");
            return new SpotifyResult<PlayerQueue> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> QueueAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("QueueAddAsync called with trackId: {TrackId}, deviceId: {DeviceId}", trackId, deviceId);

            var queryParams = new Dictionary<string, string?>() { { "uri", trackId } };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerQueueUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("QueueAddAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("post", uri, null, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "QueueAddAsync failed for trackId: {TrackId}", trackId);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<List<Device>>> DeviceGetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-state"]);
            if (error is not null) return new SpotifyResult<List<Device>> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("DeviceGetAllAsync requesting URI: {Uri}", PlayerDevicesUri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<List<Device>>("get", PlayerDevicesUri, null, ["devices"], cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeviceGetAllAsync failed");
            return new SpotifyResult<List<Device>> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> TransferAsync(string? deviceId, bool play = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("TransferAsync called with deviceId: {DeviceId}, play: {Play}", deviceId, play);

            var json = new { device_ids = new[] { deviceId }, play }.ToJson();
            var data = !string.IsNullOrEmpty(json)
                ? new StringContent(json, Encoding.UTF8, "application/json")
                : null;

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("TransferAsync requesting URI: {Uri}", PlayerUri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", PlayerUri, data, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TransferAsync failed for deviceId: {DeviceId}", deviceId);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<PlayerState>> PlayerStateGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-state"]);
            if (error is not null) return new SpotifyResult<PlayerState> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerStateGetAsync requesting URI: {Uri}", PlayerUri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<PlayerState>("get", PlayerUri, null, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerStateGetAsync failed");
            return new SpotifyResult<PlayerState> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<PlayerState>> CurrentlyPlayingGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-read-playback-state"]);
            if (error is not null) return new SpotifyResult<PlayerState> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("CurrentlyPlayingGetAsync requesting URI: {Uri}", PlayerCurrentlyPlayingUri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<PlayerState>("get", PlayerCurrentlyPlayingUri, null, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CurrentlyPlayingGetAsync failed");
            return new SpotifyResult<PlayerState> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<bool>> PlayerPlayAsync(string? deviceId, List<string>? trackUris, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerPlayAsync called with deviceId: {DeviceId}, trackUris count: {Count}", deviceId, trackUris?.Count ?? 0);

            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerPlayUri.ToUri(queryParams);
            var json = new { uris = trackUris }.ToJson();
            var data = trackUris?.Count > 0 && !string.IsNullOrEmpty(json)
                ? new StringContent(json, Encoding.UTF8, "application/json")
                : null;

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerPlayAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, data, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerPlayAsync failed for deviceId: {DeviceId}", deviceId);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> PlayerPauseAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerPauseAsync called with deviceId: {DeviceId}", deviceId);

            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerPauseUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerPauseAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerPauseAsync failed for deviceId: {DeviceId}", deviceId);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> PlayerNextAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerNextAsync called with deviceId: {DeviceId}", deviceId);

            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerNextUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerNextAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("post", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerNextAsync failed for deviceId: {DeviceId}", deviceId);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> PlayerPreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerPreviousAsync called with deviceId: {DeviceId}", deviceId);

            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerPreviousUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerPreviousAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("post", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerPreviousAsync failed for deviceId: {DeviceId}", deviceId);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<bool>> PlayerSeekAsync(int positionMs, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerSeekAsync called with positionMs: {PositionMs}, deviceId: {DeviceId}", positionMs, deviceId);

            var queryParams = new Dictionary<string, string?>()
            {
                { "position_ms", positionMs.ToString() }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerSeekUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerSeekAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerSeekAsync failed for positionMs: {PositionMs}", positionMs);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> PlayerRepeatAsync(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerRepeatAsync called with mode: {Mode}, deviceId: {DeviceId}", playerRepeatMode, deviceId);

            var queryParams = new Dictionary<string, string?>()
            {
                { "state", playerRepeatMode.ToString().ToLower() }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerRepeatUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerRepeatAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerRepeatAsync failed");
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> PlayerShuffleAsync(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerShuffleAsync called with mode: {Mode}, deviceId: {DeviceId}", playerShuffleMode, deviceId);

            var queryParams = new Dictionary<string, string?>()
            {
                { "state", playerShuffleMode == PlayerShuffleMode.On ? "true" : "false" }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerShuffleUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerShuffleAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerShuffleAsync failed");
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<bool>> PlayerVolumeAsync(int volumePercent, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-modify-playback-state"]);
            if (error is not null) return new SpotifyResult<bool> { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerVolumeAsync called with volumePercent: {VolumePercent}, deviceId: {DeviceId}", volumePercent, deviceId);

            var queryParams = new Dictionary<string, string?>()
            {
                { "volume_percent", volumePercent.ToString() }
            };
            if (!string.IsNullOrEmpty(deviceId)) queryParams.Add("device_id", deviceId);

            var uri = PlayerVolumeUri.ToUri(queryParams);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("PlayerVolumeAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<bool>("put", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlayerVolumeAsync failed for volumePercent: {VolumePercent}", volumePercent);
            return new SpotifyResult<bool> { Error = ex.ToSpotifyError() };
        }
    }
}