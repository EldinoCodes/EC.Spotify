using EC.Spotify.Abstractions;
using EC.Spotify.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("queue")]
    public async Task<IActionResult> PlayerQueueGetAsync(CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.QueueGetAsync(cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("queue")]
    public async Task<IActionResult> PlayerQueueAddAsync(string? trackUri, string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.QueueAddAsync(trackUri, deviceId, cancellationToken);

        return new JsonResult(ret);
    }


    [HttpGet("devices")]
    public async Task<IActionResult> PlayerDeviceGetAllAsync(CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.DeviceGetAllAsync(cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("transfer")]
    public async Task<IActionResult> PlayerTransferAsync(string? deviceId, bool play, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.TransferAsync(deviceId, play, cancellationToken);

        return new JsonResult(ret);
    }
    
    [HttpPost("play")]
    public async Task<IActionResult> PlayerPlayAsync(string? deviceId, List<string>? playList, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerPlayAsync(deviceId, playList, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("pause")]
    public async Task<IActionResult> PlayerPauseAsync(string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerPauseAsync(deviceId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("next")]
    public async Task<IActionResult> PlayerNextAsync(string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerNextAsync(deviceId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("previous")]
    public async Task<IActionResult> PlayerPreviousAsync(string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerPreviousAsync(deviceId, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpPost("seek")]
    public async Task<IActionResult> PlayerSeekAsync(int positionMilliseconds, string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerSeekAsync(positionMilliseconds, deviceId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("repeat")]
    public async Task<IActionResult> PlayerRepeatAsync(PlayerRepeatMode playerRepeatMode, string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerRepeatAsync(playerRepeatMode, deviceId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("shuffle")]
    public async Task<IActionResult> PlayerShuffleAsync(PlayerShuffleMode playerShuffleMode, string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerShuffleAsync(playerShuffleMode, deviceId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpPost("volume")]
    public async Task<IActionResult> PlayerVolumeAsync(int volume, string? deviceId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Player.PlayerVolumeAsync(volume, deviceId, cancellationToken);

        return new JsonResult(ret);
    }
}
