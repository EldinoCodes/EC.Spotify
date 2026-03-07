using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class ChapterService(ILogger<ChapterService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), IChapterService
{
    private readonly ILogger<ChapterService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifyChapterUri = "https://api.spotify.com/v1/chapters/{0}";

    public async Task<SpotifyResult<Chapter>> ChapterGetAsync(string? chapterId, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = string.Format(SpotifyChapterUri, chapterId);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting chapter '{chapter}'", chapterId);
        }
        return GenerateResult<Chapter>(ret);
    }
}
