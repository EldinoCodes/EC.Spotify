using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[Route("[controller]")]
[ApiController]
public class ShowsController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("{showId}")]
    public async Task<IActionResult> ShowGetAsync(string? showId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Shows.ShowGetAsync(showId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpGet("{showId}/episodes")]
    public async Task<IActionResult> ShowEpisodeGetAllAsync(string? showId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Shows.ShowEpisodeGetAllAsync(showId, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{showId}/raw")]
    public async Task<IActionResult> ShowGetRawAsync(string? showId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Shows.ShowGetRawAsync(showId, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{showId}/episodes/raw")]
    public async Task<IActionResult> ShowEpisodeGetAllRawAsync(string? showId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Shows.ShowEpisodeGetAllRawAsync(showId, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }
}
