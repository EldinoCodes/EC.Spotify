using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Enums;
using EC.Spotify.Extensions;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Audiobooks;
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
    private const string SpotifyMyAudiobooksUri = "https://api.spotify.com/v1/me/audiobooks";
    private const string SpotifyMyEpisodesUri = "https://api.spotify.com/v1/me/episodes";
    private const string SpotifyMyShowsUri = "https://api.spotify.com/v1/me/shows";
    private const string SpotifyMyTracksUri = "https://api.spotify.com/v1/me/tracks";
    private const string SpotifyMyTopItemsUri = "https://api.spotify.com/v1/me/top/{0}";

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

    public async Task<SpotifyResult<SpotifyPageResult<Audiobook>>> MyAudiobookGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-read"]);
            if (error is not null) return new SpotifyResult<SpotifyPageResult<Audiobook>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyAudiobookGetAllAsync called with limit: {Limit}, offset: {Offset}", limit, offset);

            if (limit.HasValue && (limit < 1 || limit > 50)) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 50");

            var uri = SpotifyMyAudiobooksUri.ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyAlbumGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Audiobook>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MyAudiobookGetAllAsync failed");
            return new SpotifyResult<SpotifyPageResult<Audiobook>> { Error = ex.ToSpotifyError() };
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

    public async Task<SpotifyResult<SpotifyPageResult<Show>>> MyShowGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-read", "user-read-playback-position"]);
            if (error is not null) return new SpotifyResult<SpotifyPageResult<Show>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyShowGetAllAsync called with limit: {Limit}, offset: {Offset}", limit, offset);

            if (limit.HasValue && (limit < 1 || limit > 50)) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 50");

            var uri = SpotifyMyShowsUri.ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyShowGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Show>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MyShowGetAllAsync failed");
            return new SpotifyResult<SpotifyPageResult<Show>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<SpotifyPageResult<Track>>> MyTrackGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-library-read"]);
            if (error is not null) return new SpotifyResult<SpotifyPageResult<Track>>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyTrackGetAllAsync called with limit: {Limit}, offset: {Offset}", limit, offset);

            if (limit.HasValue && (limit < 1 || limit > 50)) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 50");

            var uri = SpotifyMyTracksUri.ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"}
            });

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyTrackGetAllAsync requesting URI: {Uri}", uri);

            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPageResult<Track>>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MyTrackGetAllAsync failed");
            return new SpotifyResult<SpotifyPageResult<Track>> { Error = ex.ToSpotifyError() };
        }
    }

    public async Task<SpotifyResult<SpotifyPolymorphicPageResult>> MyTopItemGetAllAsync(int? limit = 20, int offset = 0, UserTopType userTopType = UserTopType.Tracks, UserTopTimeRange userTopTimeRange = UserTopTimeRange.MediumTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var error = _options.ValidateScopes(["user-top-read"]);
            if (error is not null) return new SpotifyResult<SpotifyPolymorphicPageResult>() { Error = error };

            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyTopItemGetAllAsync called with limit: {Limit}, offset: {Offset}, userTopType: {UserTopType}, userTopTimeRange: {UserTopTimeRange}", limit, offset, userTopType, userTopTimeRange);

            var uri = string.Format(SpotifyMyTopItemsUri, userTopType.ToString().ToLower()).ToUri(new()
            {
                { "limit", $"{limit}"},
                { "offset", $"{offset}"},
                { "time_range", $"{userTopTimeRange.ToString().Replace("Term", "_term").ToLower()}"}
            });
            if (_options.VerboseLogging && _logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("MyTopItemGetAllAsync requesting URI: {Uri}", uri);
            return await _spotifyProvider.ExecuteSpotifyResultAsync<SpotifyPolymorphicPageResult>("get", uri, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MyTopItemGetAllAsync failed");
            return new SpotifyResult<SpotifyPolymorphicPageResult> { Error = ex.ToSpotifyError() };
        }
    }
}
