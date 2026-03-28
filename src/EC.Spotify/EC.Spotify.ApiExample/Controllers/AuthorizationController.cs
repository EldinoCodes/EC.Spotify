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
        var authUrl = await _spotifyClient.Authorization.Validate(cancellationToken);

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
}
