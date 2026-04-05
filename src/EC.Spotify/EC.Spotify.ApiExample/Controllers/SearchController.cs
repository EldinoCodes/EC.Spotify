using EC.Spotify.Abstractions;
using EC.Spotify.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[Route("[controller]")]
[ApiController]
public class SearchController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet]
    public async Task<IActionResult> SearchGetAsync(string? query, SearchType? searchTypes = default, int? limit = default, int? offset = default, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Search.SearchAsync(query, searchTypes, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }
}
