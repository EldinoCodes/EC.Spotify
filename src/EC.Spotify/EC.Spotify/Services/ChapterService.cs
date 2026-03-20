using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class ChapterService(ILogger<ChapterService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IChapterService
{
    private readonly ILogger<ChapterService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyChapterUri = "https://api.spotify.com/v1/chapters/{0}";

    public async Task<SpotifyResult<Chapter>> ChapterGetAsync(string? chapterId, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(SpotifyChapterUri, chapterId);
        
        return await _spotifyProvider.ExecuteSpotifyResultAsync<Chapter>("get", uri, cancellationToken: cancellationToken);
    }
}
