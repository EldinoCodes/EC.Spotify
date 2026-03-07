using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.Controllers;

[Route("[controller]")]
[ApiController]
public class EpisodesController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("{episodeId}")]
    public async Task<IActionResult> EpisodeGetAsync(string? episodeId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Episodes.EpisodeGetAsync(episodeId, cancellationToken);

        return new JsonResult(ret);
    }
}
