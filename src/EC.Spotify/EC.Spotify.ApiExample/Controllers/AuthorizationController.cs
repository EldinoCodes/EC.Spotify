using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorizationController(ILogger<AuthorizationController> logger, ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ILogger<AuthorizationController> _logger = logger;
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet]
    [Route("validate", Name = "authorizationValidate")]
    public async Task<IActionResult> GetResponseAsync(CancellationToken cancellationToken = default)
    {
        var authUrl = await _spotifyClient.Authorization.ValidateAsync(cancellationToken);

        return !string.IsNullOrEmpty(authUrl) 
            ? Redirect(authUrl) 
            : Ok("Authorized");
    }

    [HttpGet]
    [Route("response")]
    public async Task<IActionResult> GetResponseAsync([FromQuery] string? code, [FromQuery] string? state, CancellationToken cancellationToken = default)
    {
        await _spotifyClient.Authorization.AuthorizationCodeAddAsync(code, state, cancellationToken);
        return RedirectToRoute("authorizationValidate");
    }

    [HttpGet]
    [Route("code-url")]
    public IActionResult AuthorizationCodeUrl()
    {
        var url = _spotifyClient.Authorization.AuthorizationCodeUrl();

        return new JsonResult(url);
    }

    [HttpGet]
    [Route("code")]
    public async Task<IActionResult> AuthorizationCodeGetAsync(CancellationToken cancellationToken = default)
    {
        var code = await _spotifyClient.Authorization.AuthorizationCodeGetAsync(cancellationToken);

        return new JsonResult(code);
    }

    [HttpDelete]
    [Route("code")]
    public async Task<IActionResult> AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Authorization.AuthorizationCodeRemoveAsync(cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet]
    [Route("token")]
    public async Task<IActionResult> AuthorizationTokenGetAsync(CancellationToken cancellationToken = default)
    {
        var token = await _spotifyClient.Authorization.AuthorizationTokenGetAsync(cancellationToken);

        return new JsonResult(token);
    }

    [HttpPost]
    [Route("token/reset")]
    public async Task<IActionResult> AuthorizationTokenResetAsync()
    {
        var ret = await _spotifyClient.Authorization.AuthorizationTokenReset();

        return new JsonResult(ret);
    }
}
