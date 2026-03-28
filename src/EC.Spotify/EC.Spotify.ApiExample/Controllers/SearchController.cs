using EC.Spotify.Abstractions;
using EC.Spotify.Models.Searches;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[Route("[controller]")]
[ApiController]
public class SearchController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpPost]
    public async Task<IActionResult> SearchPostAsync(SearchQuery? searchQuery, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.Search.SearchAsync(searchQuery, cancellationToken);

        return new JsonResult(ret);
    }
}
