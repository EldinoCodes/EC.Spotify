    using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[Route("[controller]")]
[ApiController]
public class ChaptersController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("{chapterId}")]
    public async Task<IActionResult> ChapterGetAsync(string? chapterId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Chapters.ChapterGetAsync(chapterId, cancellationToken);

        return new JsonResult(ret);
    }
}
