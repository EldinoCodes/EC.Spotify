using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Audiobooks;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class AudiobookService(ILogger<AudiobookService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), IAudiobookService
{
    private readonly ILogger<AudiobookService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifyAudiobookUri = "https://api.spotify.com/v1/audiobooks/{0}";
    private const string SpotifyAudiobookChaptersUri = "https://api.spotify.com/v1/audiobooks/{0}/chapters";

    public async Task<SpotifyResult<Audiobook>> AudiobookGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = string.Format(SpotifyAudiobookUri, id);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting audiobook '{audiobook}'", id);
        }
        return GenerateResult<Audiobook>(ret);
    }

    public async Task<SpotifyResult<SpotifyPageResult>> AudiobookChapterGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            };
            var uri = BuildUri(string.Format(SpotifyAudiobookChaptersUri, id), queryParams);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting audiobook chapters for '{audiobook}'", id);
        }
        return GenerateResult<SpotifyPageResult>(ret);
    }
}
