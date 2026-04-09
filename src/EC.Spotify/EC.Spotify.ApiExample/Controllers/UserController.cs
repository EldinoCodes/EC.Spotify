using EC.Spotify.Abstractions;
using EC.Spotify.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    [HttpGet("albums")]
    public async Task<IActionResult> MyAlbumGetAllAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyAlbumGetAllAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("audiobooks")]
    public async Task<IActionResult> MyAudiobookGetAllAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyAudiobookGetAllAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("episodes")]
    public async Task<IActionResult> MyEpisodeGetAllAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyEpisodeGetAllAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }


    [HttpGet("playlists")]
    public async Task<IActionResult> MyPlaylistGetAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = new JsonResult(await _spotifyClient.User.MyPlaylistGetAllAsync(limit, offset, cancellationToken));

        return new JsonResult(ret);
    }

    [HttpGet("shows")]
    public async Task<IActionResult> MyShowGetAllAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyShowGetAllAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("tracks")]
    public async Task<IActionResult> MyTrackGetAllAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyTrackGetAllAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("top")]
    public async Task<IActionResult> MyTopItemGetAllAsync(UserTopType userTopType, UserTopTimeRange userTopTimeRange, int? limit, int offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyTopItemGetAllAsync(userTopType, userTopTimeRange, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("albums/raw")]
    public async Task<IActionResult> MyAlbumGetAllRawAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyAlbumGetAllRawAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("audiobooks/raw")]
    public async Task<IActionResult> MyAudiobookGetAllRawAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyAudiobookGetAllRawAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("episodes/raw")]
    public async Task<IActionResult> MyEpisodeGetAllRawAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyEpisodeGetAllRawAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("playlists/raw")]
    public async Task<IActionResult> MyPlaylistGetAllRawAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyPlaylistGetAllRawAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("shows/raw")]
    public async Task<IActionResult> MyShowGetAllRawAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyShowGetAllRawAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("tracks/raw")]
    public async Task<IActionResult> MyTrackGetAllRawAsync(int? limit, int? offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyTrackGetAllRawAsync(limit, offset, cancellationToken);

        return new JsonResult(ret);
    }

    [HttpGet("top/raw")]
    public async Task<IActionResult> MyTopItemGetAllRawAsync(UserTopType userTopType, UserTopTimeRange userTopTimeRange, int? limit, int offset, CancellationToken cancellationToken = default)
    {
        var ret = await _spotifyClient.User.MyTopItemGetAllRawAsync(userTopType, userTopTimeRange, limit, offset, cancellationToken);

        return new JsonResult(ret);
    }
}
