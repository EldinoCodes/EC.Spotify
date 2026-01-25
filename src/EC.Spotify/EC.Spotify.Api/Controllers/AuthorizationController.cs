using EC.Spotify.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorizationController(ILogger<AuthorizationController> logger, IConfiguration configuration, IAuthorizationService authorizationService, ISpotifyService spotifyService) : ControllerBase
{
    private readonly ILogger<AuthorizationController> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthorizationService _authorizationService = authorizationService;
    private readonly ISpotifyService _spotifyService = spotifyService;

    [HttpGet]
    [Route("validate", Name = "authorizationValidate")]
    public async Task<IActionResult> GetResponseAsync(CancellationToken cancellationToken = default)
    {
        var authUrl = await _spotifyService.AuthorizationUrlGetAsync(cancellationToken);        
        return string.IsNullOrEmpty(authUrl) 
            ? Ok("Authorized")
            : Redirect(authUrl);
    }

    [HttpGet]
    [Route("response")]
    public async Task<IActionResult> GetResponseAsync([FromRoute] string? topic, [FromQuery] string? code, CancellationToken cancellationToken = default)
    {
        await _authorizationService.AuthorizationCodeRemoveAsync(cancellationToken);
        await _authorizationService.AuthorizationCodeAddAsync(code, cancellationToken);

        return RedirectToRoute("authorizationValidate");
    }
}
