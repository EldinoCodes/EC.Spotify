using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.Controllers;

[Route("[controller]")]
[ApiController]
public class AudiobooksController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("{audiobookId}")]
    public async Task<IActionResult> AudiobookGetAsync(string? audiobookId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Audiobooks.AudiobookGetAsync(audiobookId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpGet("{audiobookId}/chapters")]
    public async Task<IActionResult> AudiobookChapterGetAllAsync(string? audiobookId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Audiobooks.AudiobookChapterGetAllAsync(audiobookId, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{audiobookId}/raw")]
    public async Task<IActionResult> AudiobookGetRawAsync(string? audiobookId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Audiobooks.AudiobookGetRawAsync(audiobookId, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{audiobookId}/chapters/raw")]
    public async Task<IActionResult> AudiobookChapterGetAllRawAsync(string? audiobookId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Audiobooks.AudiobookChapterGetAllRawAsync(audiobookId, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }
}
