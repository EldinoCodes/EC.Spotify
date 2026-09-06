using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.Controllers;

[Route("[controller]")]
[ApiController]
public class AlbumsController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    // Existing methods
    [HttpGet("{albumId}")]
    public async Task<IActionResult> AlbumGetAsync(string? albumId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Albums.AlbumGetAsync(albumId, cancellationToken);
        return new JsonResult(ret);
    }

    [HttpGet("{albumId}/tracks")]
    public async Task<IActionResult> AlbumTrackGetAllAsync(string? albumId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Albums.AlbumTrackGetAllAsync(albumId, limit, offset, cancellationToken);
        return new JsonResult(ret);
    }

    [HttpGet("{albumId}/raw")]
    public async Task<IActionResult> AlbumGetRawAsync(string? albumId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Albums.AlbumGetRawAsync(albumId, cancellationToken);
        return new JsonResult(ret);
    }

    [HttpGet("{albumId}/tracks/raw")]
    public async Task<IActionResult> AlbumTrackGetAllRawAsync(string? albumId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Albums.AlbumTrackGetAllRawAsync(albumId, limit, offset, cancellationToken);
        return new JsonResult(ret);
    }
}
