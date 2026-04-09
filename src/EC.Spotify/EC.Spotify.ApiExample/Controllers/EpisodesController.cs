using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

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

    [HttpGet("{episodeId}/raw")]
    public async Task<IActionResult> EpisodeGetRawAsync(string? episodeId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Episodes.EpisodeGetRawAsync(episodeId, cancellationToken);

        return new JsonResult(ret);
    }
}
