using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class ShowService(ILogger<ShowService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IShowService
{
    private readonly ILogger<ShowService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyShowUri = "https://api.spotify.com/v1/shows/{0}";
    private const string SpotifyShowEpisodesUri = "https://api.spotify.com/v1/shows/{0}/episodes";

    public async Task<SpotifyResult<Show>> ShowGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(SpotifyShowUri, id);

        return await _spotifyProvider.ExecuteSpotifyResultAsync<Show>("get", uri, cancellationToken: cancellationToken);
    }
    public async Task<SpotifyResult<SpotifyPageResult>> ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(SpotifyShowEpisodesUri, id).ToUri(new()
        {
            { "limit", $"{limit}"},
            { "offset", $"{offset }"}
        });

        return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult>("get", uri, cancellationToken: cancellationToken);
    }
}
