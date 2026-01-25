using EC.Spotify.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SearchController(ISpotifyService spotifyService) : ControllerBase
{
    private readonly ISpotifyService _spotifyService = spotifyService;

    [HttpPost()]
    public async Task<IActionResult> SearchAsync(string? artistName, string? albumName, string? trackName, string? genre = null, CancellationToken cancellationToken = default)
    {
        var authUrl = await _spotifyService.AuthorizationUrlGetAsync(cancellationToken);
        if (!string.IsNullOrEmpty(authUrl)) return Unauthorized(new { AuthorizationUrl = authUrl });

        var ret = await _spotifyService.TrackSearchAllAsync(artistName, albumName, trackName, genre, cancellationToken);
        return new JsonResult(ret);
    }
    [HttpGet("track")]
    public async Task<IActionResult> TrackGetAsync(string? trackId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(trackId)) return BadRequest("trackId is required");

        var authUrl = await _spotifyService.AuthorizationUrlGetAsync(cancellationToken);
        if (!string.IsNullOrEmpty(authUrl)) return Unauthorized(new { AuthorizationUrl = authUrl });

        var ret = await _spotifyService.TrackGetAsync(trackId, cancellationToken);
        return new JsonResult(ret);
    }
}
