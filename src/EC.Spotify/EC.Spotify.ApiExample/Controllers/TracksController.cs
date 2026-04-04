using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[Route("[controller]")]
[ApiController]
public class TracksController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("{trackId}")]
    public async Task<IActionResult> TrackGetAsync(string? trackId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Tracks.TrackGetAsync(trackId, cancellationToken);

        return new JsonResult(ret);
    }
}
