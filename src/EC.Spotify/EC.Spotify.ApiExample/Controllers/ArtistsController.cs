using EC.Spotify.Abstractions;
using EC.Spotify.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.Controllers;

[Route("[controller]")]
[ApiController]
public class ArtistsController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("{artistId}")]
    public async Task<IActionResult> ArtistGetAsync(string? artistId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Artists.ArtistGetAsync(artistId, cancellationToken);

        return new JsonResult(ret);
    }
    [HttpGet("{artistId}/albums")]
    public async Task<IActionResult> ArtistAlbumGetAllAsync(string? artistId, int? limit, int? offset, AlbumType? albumTypes = default, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Artists.ArtistAlbumGetAllAsync(artistId, limit, offset, albumTypes, cancellationToken);

        return new JsonResult(ret);
    }
}
