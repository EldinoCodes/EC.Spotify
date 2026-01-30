using EC.Spotify.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PlayerController(ISpotifyService spotifyService) : ControllerBase
{
    private readonly ISpotifyService _spotifyService = spotifyService;

    [HttpGet("devices")]
    public async Task<IActionResult> PlayerDeviceGetAllAsync(CancellationToken cancellationToken = default)
    {
        var res = await ValidateAuthorizationAsync(cancellationToken);
        if (res != null) return res;

        var ret = await _spotifyService.DeviceGetAllAsync(cancellationToken);
        return new JsonResult(ret);
    }
    [HttpGet("state")]
    public async Task<IActionResult> PlayerStateGetAsync(CancellationToken cancellationToken = default)
    {
        var res = await ValidateAuthorizationAsync(cancellationToken);
        if (res != null) return res;

        var ret = await _spotifyService.PlayerStateGetAsync(cancellationToken);
        return new JsonResult(ret);
    }

    [HttpPost("next")]
    public async Task<IActionResult> PlayerNextAsync(string? deviceId, CancellationToken cancellationToken = default)
    {
        var res = await ValidateAuthorizationAsync(cancellationToken);
        if (res != null) return res;

        var ret = await _spotifyService.PlayerNextAsync(deviceId, cancellationToken);
        return new JsonResult(ret);
    }
    [HttpPost("pause")]
    public async Task<IActionResult> PlayerPauseAsync(string? deviceId, CancellationToken cancellationToken = default)
    {
        var res = await ValidateAuthorizationAsync(cancellationToken);
        if (res != null) return res;

        var ret = await _spotifyService.PlayerPauseAsync(deviceId, cancellationToken);
        return new JsonResult(ret);
    }
    [HttpPost("play")]
    public async Task<IActionResult> PlayerPlayAsync(string? deviceId, CancellationToken cancellationToken = default)
    {
        var res = await ValidateAuthorizationAsync(cancellationToken);
        if (res != null) return res;

        var ret = await _spotifyService.PlayerPlayAsync(deviceId, cancellationToken);
        return new JsonResult(ret);
    }
    [HttpPost("previous")]
    public async Task<IActionResult> PlayerPreviousAsync(string? deviceId, CancellationToken cancellationToken = default)
    {
        var res = await ValidateAuthorizationAsync(cancellationToken);
        if (res != null) return res;

        var ret = await _spotifyService.PlayerPreviousAsync(deviceId, cancellationToken);
        return new JsonResult(ret);
    }


    [HttpPost("addTrack")]
    public async Task<IActionResult> PlayerAddTrackAsync(string? trackUri, string? deviceId, CancellationToken cancellationToken = default)
    {
        var res = await ValidateAuthorizationAsync(cancellationToken);
        if (res != null) return res;

        var ret = await _spotifyService.PlayerTrackAddAsync(trackUri, deviceId, cancellationToken);
        return new JsonResult(ret);
    }

    private async Task<IActionResult?> ValidateAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        var authUrl = await _spotifyService.AuthorizationUrlGetAsync(cancellationToken);
        return !string.IsNullOrEmpty(authUrl) 
            ? Unauthorized(new { AuthorizationUrl = authUrl })
            : default(IActionResult?);
    }
}
