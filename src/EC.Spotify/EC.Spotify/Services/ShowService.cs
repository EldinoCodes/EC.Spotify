using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;

namespace EC.Spotify.Services;

internal class ShowService(ILogger<ShowService> logger, ISpotifyHttpProvider httpProvider, IAuthorizationService authorizationService, ISpotifyJsonSerializer spotifyJsonSerializer) : BaseSpotifyService(authorizationService, spotifyJsonSerializer), IShowService
{
    private readonly ILogger<ShowService> _logger = logger;
    private readonly ISpotifyHttpProvider _httpProvider = httpProvider;

    private const string SpotifyShowUri = "https://api.spotify.com/v1/shows/{0}";
    private const string SpotifyShowEpisodesUri = "https://api.spotify.com/v1/shows/{0}/episodes";

    public async Task<SpotifyResult<Show>> ShowGetAsync(string? id, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var uri = string.Format(SpotifyShowUri, id);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting show '{show}'", id);
        }
        return GenerateResult<Show>(ret);
    }

    public async Task<SpotifyResult<SpotifyPageResult<Episode>>> ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        string? ret = default;
        try
        {
            var queryParams = new Dictionary<string, string?>()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset }"}
            };
            var uri = BuildUri(string.Format(SpotifyShowEpisodesUri, id), queryParams);
            var header = await GetAuthorizationHeaderAsync(cancellationToken);
            ret = await _httpProvider.ExecuteAsync("get", uri, null, header, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting show episodes for show '{show}'", id);
        }
        return GenerateResult<SpotifyPageResult<Episode>>(ret);
    }
}
