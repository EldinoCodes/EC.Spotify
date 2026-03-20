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
        var uri = string.Format(SpotifyAudiobookUri, id);

        return await _spotifyProvider.ExecuteSpotifyResultAsync<Audiobook>("get", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<SpotifyPageResult>> AudiobookChapterGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(SpotifyAudiobookChaptersUri, id).ToUri(new()
        {
            { "limit", $"{limit}"},
            { "offset", $"{offset}"}
        });
        return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult>("get", uri, cancellationToken: cancellationToken);
    }
}
