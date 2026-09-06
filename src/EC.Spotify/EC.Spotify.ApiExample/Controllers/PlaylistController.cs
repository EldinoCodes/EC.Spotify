using EC.Spotify.Abstractions;
using EC.Spotify.Models.Library;
using EC.Spotify.Models.Playlists;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaylistController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("{playlistId}")]
    public async Task<IActionResult> PlaylistGetAsync(string? playlistId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistGetAsync(playlistId, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{playlistId}/items")]
    public async Task<IActionResult> PlaylistItemGetAllAsync(string? playlistId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistItemGetAllAsync(playlistId, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpPut("{playlistId}")]
    public async Task<IActionResult> PlaylistDetailUpdateAsync(string? playlistId, [FromBody] PlaylistDetail? playlistDetail, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistDetailUpdateAsync(playlistId, playlistDetail, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpPost("{playlistId}/items")]
    public async Task<IActionResult> PlaylistItemAddAsync(string? playlistId, [FromBody] ReferenceItem? libraryItem, int? position = null, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistItemAddAsync(playlistId, libraryItem, position, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpPost("{playlistId}/items/batch")]
    public async Task<IActionResult> PlaylistItemAddAllAsync(string? playlistId, [FromBody] List<ReferenceItem> libraryItems, int? position = null, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistItemAddAllAsync(playlistId, libraryItems, position, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpDelete("{playlistId}/items")]
    public async Task<IActionResult> PlaylistItemRemoveAsync(string? playlistId, [FromBody] ReferenceItem? libraryItem, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistItemRemoveAsync(playlistId, libraryItem, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpDelete("{playlistId}/items/batch")]
    public async Task<IActionResult> PlaylistItemRemoveAllAsync(string? playlistId, [FromBody] List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistItemRemoveAllAsync(playlistId, libraryItems, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpPut("{playlistId}/image")]
    public async Task<IActionResult> PlaylistImageAddAsync(string? playlistId, IFormFile image, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream, cancellationToken);
        var ret = await _spotifyClient.Playlists.PlaylistImageAddAsync(playlistId, memoryStream.ToArray(), cancellationToken);

        return new JsonResult(ret);
    }
    [HttpGet("{playlistId}/image")]
    public async Task<IActionResult> PlaylistImageGetAllAsync(string? playlistId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistImageGetAllAsync(playlistId, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{playlistId}/raw")]
    public async Task<IActionResult> PlaylistGetRawAsync(string? playlistId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistGetRawAsync(playlistId, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{playlistId}/items/raw")]
    public async Task<IActionResult> PlaylistItemGetAllRawAsync(string? playlistId, int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistItemGetAllRawAsync(playlistId, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("{playlistId}/image/raw")]
    public async Task<IActionResult> PlaylistImageGetAllRawAsync(string? playlistId, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistImageGetAllRawAsync(playlistId, cancellationToken);

        return new JsonResult(ret);
    }

    // Create playlist
    [HttpPost("create")]
    public async Task<IActionResult> PlaylistCreateAsync([FromBody] PlaylistCreate playlistCreate, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Playlists.PlaylistCreateAsync(playlistCreate, cancellationToken);
        return new JsonResult(ret);
    }
}
