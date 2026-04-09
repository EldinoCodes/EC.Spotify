using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
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
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ChapterGetAsync called with chapterId: {ChapterId}", chapterId);

            var uri = string.Format(SpotifyChapterUri, chapterId);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ChapterGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Chapter>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ChapterGetAsync failed for chapterId: {ChapterId}", chapterId);
            return new SpotifyResult<Chapter> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<string?> ChapterGetRawAsync(string? chapterId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ChapterGetRawAsync called with chapterId: {ChapterId}", chapterId);

            var uri = string.Format(SpotifyChapterUri, chapterId);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("ChapterGetRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ChapterGetRawAsync failed for chapterId: {ChapterId}", chapterId);
            throw;
        }
    }
}
