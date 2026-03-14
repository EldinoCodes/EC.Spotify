using EC.Spotify.Abstractions;
using EC.Spotify.Enums;
using EC.Spotify.Models.Library;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("check")]
    public async Task<IActionResult> LibraryCheckAsync(string? id, LibraryType type = LibraryType.Track, CancellationToken cancellationToken = default)
    {
        var libraryItem = new LibraryItem { Id = id, Type = type };
        var ret = await _spotifyClient.Library.LibraryCheckAsync(libraryItem, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("check/batch")]
    public async Task<IActionResult> LibraryCheckAllAsync([FromBody] List<LibraryItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Library.LibraryCheckAllAsync(libraryItems, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpPost]
    public async Task<IActionResult> LibraryAddAsync(string? id, LibraryType type = LibraryType.Track, CancellationToken cancellationToken = default)
    {
        var libraryItem = new LibraryItem { Id = id, Type = type };
        var ret = await _spotifyClient.Library.LibraryAddAsync(libraryItem, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> LibraryAddAllAsync([FromBody] List<LibraryItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Library.LibraryAddAllAsync(libraryItems, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpDelete]
    public async Task<IActionResult> LibraryRemoveAsync(string? id, LibraryType type = LibraryType.Track, CancellationToken cancellationToken = default)
    {
        var libraryItem = new LibraryItem { Id = id, Type = type };
        var ret = await _spotifyClient.Library.LibraryRemoveAsync(libraryItem, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpDelete("batch")]
    public async Task<IActionResult> LibraryRemoveAllAsync([FromBody] List<LibraryItem> libraryItems, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Library.LibraryRemoveAllAsync(libraryItems, cancellationToken);

        return new JsonResult(ret);
    }
}
