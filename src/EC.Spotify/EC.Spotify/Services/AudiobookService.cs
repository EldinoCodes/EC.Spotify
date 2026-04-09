using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class AudiobookService(ILogger<AudiobookService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IAudiobookService
{
    private readonly ILogger<AudiobookService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyAudiobookUri = "https://api.spotify.com/v1/audiobooks/{0}";
    private const string SpotifyAudiobookChaptersUri = "https://api.spotify.com/v1/audiobooks/{0}/chapters";

    public async Task<SpotifyResult<Audiobook>> AudiobookGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookGetAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyAudiobookUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookGetAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<Audiobook>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AudiobookGetAsync failed for id: {Id}", id);
            return new SpotifyResult<Audiobook> { Error = ex.ToSpotifyError() };
        }
    }
    public async Task<SpotifyResult<SpotifyPageResult<Chapter>>> AudiobookChapterGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookChapterGetAllAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            var uri = string.Format(SpotifyAudiobookChaptersUri, id).ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookChapterGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Chapter>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AudiobookChapterGetAllAsync failed for id: {Id}", id);
            return new SpotifyResult<SpotifyPageResult<Chapter>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<string?> AudiobookGetRawAsync(string? id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookGetRawAsync called with id: {Id}", id);

            var uri = string.Format(SpotifyAudiobookUri, id);

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookGetRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AudiobookGetRawAsync failed for id: {Id}", id);
            throw;
        }
    }
    public async Task<string?> AudiobookChapterGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookChapterGetAllRawAsync called with id: {Id}, limit: {Limit}, offset: {Offset}", id, limit, offset);

            var uri = string.Format(SpotifyAudiobookChaptersUri, id).ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("AudiobookChapterGetAllRawAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyRequestAsync("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AudiobookChapterGetAllRawAsync failed for id: {Id}", id);
            throw;
        }
    }
}
