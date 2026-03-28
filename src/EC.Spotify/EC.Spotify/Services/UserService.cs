using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Shows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Services;

internal class UserService(ILogger<UserService> logger, IOptions<SpotifyOptions> options, ISpotifyProvider spotifyProvider) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly SpotifyOptions _options = options.Value;
    private readonly ISpotifyProvider _spotifyProvider = spotifyProvider;

    private const string SpotifyMyAlbumsUri = "https://api.spotify.com/v1/me/albums";
    private const string SpotifyMyEpisodesUri = "https://api.spotify.com/v1/me/episodes";

    public async Task<SpotifyResult<SpotifyPageResult<Album>>> MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-read"]);
            if (error is not null) return new SpotifyResult<SpotifyPageResult<Album>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyAlbumGetAllAsync called with limit: {Limit}, offset: {Offset}", limit, offset);

            if (limit.HasValue && (limit < 1 || limit > 50)) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 50");

            var uri = SpotifyMyAlbumsUri.ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyAlbumGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Album>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MyAlbumGetAllAsync failed");
            return new SpotifyResult<SpotifyPageResult<Album>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<SpotifyPageResult<Episode>>> MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-read", "user-read-playback-position"]);
            if (error is not null) return new SpotifyResult<SpotifyPageResult<Episode>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyEpisodeGetAllAsync called with limit: {Limit}, offset: {Offset}", limit, offset);

            if (limit.HasValue && (limit < 1 || limit > 50)) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 50");

            var uri = SpotifyMyEpisodesUri.ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyEpisodeGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Episode>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MyEpisodeGetAllAsync failed");
            return new SpotifyResult<SpotifyPageResult<Episode>> { Error = ex.ToSpotifyError() };
        }
    }

    }
