using EC.Spotify.Api.Models;
using EC.Spotify.Api.Providers;
using System.Net.Http.Headers;

namespace EC.Spotify.Api.Services;

public interface ISpotifyService
{
    Task<string?> AuthorizationUrlGetAsync(CancellationToken cancellationToken = default);

    Task<List<Device>> DeviceGetAllAsync(CancellationToken cancellationToken = default);
    Task<PlayerState?> PlayerStateGetAsync(CancellationToken cancellationToken = default);
    Task<bool> PlayerNextAsync(string? deviceId = null, CancellationToken cancellationToken = default);
    Task<bool> PlayerPauseAsync(string? deviceId = null, CancellationToken cancellationToken = default);
    Task<bool> PlayerPlayAsync(string? deviceId = null, CancellationToken cancellationToken = default);
    Task<bool> PlayerPreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default);
    Task<bool> PlayerTrackAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default);

    Task<Track?> TrackGetAsync(string? trackId, CancellationToken cancellationToken = default);    
    Task<List<Track>> TrackSearchAllAsync(string? artistName, string? albumName, string? trackName, string? genre = null, CancellationToken cancellationToken = default);
}

public class SpotifyService(ILogger<SpotifyService> logger, IConfiguration configuration, IAuthorizationService authorizationService, ITokenService tokenService, IHttpProvider httpProvider) : ISpotifyService
{
    private readonly ILogger<SpotifyService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthorizationService _authorizationService = authorizationService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IHttpProvider _httpProvider = httpProvider;

    private readonly string _playerUri = configuration.GetValue<string?>("AppSettings:PlayerUri") ?? throw new ArgumentNullException("AppSettings:PlayerUri");
    private readonly string _trackUri = configuration.GetValue<string?>("AppSettings:TrackUri") ?? throw new ArgumentNullException("AppSettings:TrackUri");
    private readonly string _searchUri = configuration.GetValue<string?>("AppSettings:SearchUri") ?? throw new ArgumentNullException("AppSettings:SearchUri");

    public async Task<string?> AuthorizationUrlGetAsync(CancellationToken cancellationToken = default)
    {
        var uri = _authorizationService.AuthorizationCodeUrl();
        ArgumentException.ThrowIfNullOrEmpty(uri, nameof(uri));

        var authorizationCode = await _authorizationService.AuthorizationCodeGetAsync(cancellationToken);
        if (authorizationCode is null) return uri;

        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        if (userToken is null) return uri;

        return default;
    }

    public async Task<List<Device>> DeviceGetAllAsync(CancellationToken cancellationToken = default)
    {
        var ret = new List<Device>();
        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var res = await _httpProvider.ExecuteAsync("get", $"{_playerUri}/devices", null, (h) =>
            {
                h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);
            }, false, cancellationToken);

            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            var response = res.FromJson<DeviceResult>();
            if (response?.Devices?.Count > 0)
                ret.AddRange(response.Devices.OrderBy(d => d.Name));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting devices");
        }
        return ret;
    }
    public async Task<PlayerState?> PlayerStateGetAsync(CancellationToken cancellationToken = default)
    {
        var ret = default(PlayerState?);
        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var res = await _httpProvider.ExecuteAsync("get", $"{_playerUri}/queue", null, (h) =>
            {
                h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);
            }, false, cancellationToken);

            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            ret = res.FromJson<PlayerState>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding track to queue");
        }
        return ret;
    }
    public async Task<bool> PlayerNextAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var ret = false;
        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var uri = new UriBuilder($"{_playerUri}/next");
            if (!string.IsNullOrEmpty(deviceId))
                uri.Query = $"device_id={deviceId}";

            void headers(HttpRequestHeaders h) => h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);

            var res = await _httpProvider.ExecuteAsync("post", uri.ToString(), null, headers, false, cancellationToken);
            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            ret = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error skipping track");
        }
        return ret;
    }
    public async Task<bool> PlayerPauseAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var ret = false;
        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var uri = new UriBuilder($"{_playerUri}/pause");
            if (!string.IsNullOrEmpty(deviceId))
                uri.Query = $"device_id={deviceId}";

            void headers(HttpRequestHeaders h) => h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);

            var res = await _httpProvider.ExecuteAsync("put", uri.ToString(), null, headers, false, cancellationToken);
            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            ret = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pausing track");
        }
        return ret;
    }
    public async Task<bool> PlayerPlayAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var ret = false;
        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var uri = new UriBuilder($"{_playerUri}/play");
            if (!string.IsNullOrEmpty(deviceId))
                uri.Query = $"device_id={deviceId}";

            var json = new { context_id = deviceId, position_ms = 0 }.ToJson();
            ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            void headers(HttpRequestHeaders h) => h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);

            var res = await _httpProvider.ExecuteAsync("put", uri.ToString(), content, headers, false, cancellationToken);
            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            ret = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error playing track");
        }
        return ret;
    }
    public async Task<bool> PlayerPreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var ret = false;
        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var uri = new UriBuilder($"{_playerUri}/previous");
            if (!string.IsNullOrEmpty(deviceId))
                uri.Query = $"device_id={deviceId}";            

            void headers(HttpRequestHeaders h) => h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);

            var res = await _httpProvider.ExecuteAsync("post", uri.ToString(), null, headers, false, cancellationToken);
            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            ret = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error skipping track");
        }
        return ret;
    }
    public async Task<bool> PlayerTrackAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var ret = false;
        ArgumentException.ThrowIfNullOrEmpty(trackId, nameof(trackId));

        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var query = $"uri={trackId}";
            if (!string.IsNullOrEmpty(deviceId))
                query += $"&device_id={deviceId}";

            var uri = new UriBuilder($"{_playerUri}/queue")
            {
                Query = query
            }.ToString();

            void headers(HttpRequestHeaders h) => h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);

            var res = await _httpProvider.ExecuteAsync("post", uri, null, headers, false, cancellationToken);
            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            ret = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding track to queue");
        }
        return ret;
    }

    public async Task<Track?> TrackGetAsync(string? trackId, CancellationToken cancellationToken = default)
    {
        var ret = default(Track);
        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var res = await _httpProvider.ExecuteAsync("get", $"{_trackUri}/{trackId}", null, (h) =>
            {
                h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);
            }, false, cancellationToken);

            var errorResult = res.FromJson<ErrorResult>();
            if (errorResult?.Error is not null)
            {
                if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
            }

            var track = res.FromJson<Track>();
            if (track is null) return default;

            ret = track;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current track");
        }
        return ret;
    }    
    public async Task<List<Track>> TrackSearchAllAsync(string? artistName, string? albumName, string? trackName, string? genre = default, CancellationToken cancellationToken = default)
    {
        var ret = new List<Track>();

        ArgumentException.ThrowIfNullOrEmpty(trackName, nameof(trackName));

        var userToken = await _tokenService.AuthUserTokenGetAsync(cancellationToken);
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(userToken?.TokenType, nameof(userToken.TokenType));
            ArgumentException.ThrowIfNullOrEmpty(userToken?.AccessToken, nameof(userToken.AccessToken));

            var query = new List<string>();
            if (!string.IsNullOrEmpty(artistName)) query.Add($"artist:\"{artistName}\"");
            if (!string.IsNullOrEmpty(albumName)) query.Add($"album:\"{albumName}\"");
            if (!string.IsNullOrEmpty(trackName)) query.Add($"track:\"{trackName}\"");
            if (!string.IsNullOrEmpty(genre)) query.Add($"genre:\"{trackName}\"");

            var uri = new UriBuilder(_searchUri)
            {
                Query = $"q={string.Join(" ", query)}&type=artist,album,track"
            }.ToString();

            void headers(HttpRequestHeaders h) => h.Authorization = new AuthenticationHeaderValue(userToken.TokenType, userToken.AccessToken);

            for (var i = 0; i < 5; i++)
            {
                var res = await _httpProvider.ExecuteAsync("get", uri, null, headers, false, cancellationToken);
                var errorResult = res.FromJson<ErrorResult>();
                if (errorResult?.Error is not null)
                {
                    if (errorResult.Error.Status == 401) _tokenService.AuthTokenReset();

                    throw new Exception($"Error: {errorResult.Error.Message} Reason: {errorResult.Error.Reason} Status: {errorResult.Error.Status}");
                }

                var result = res.FromJson<QueryResult>();
                if (result?.Tracks?.Items?.Count > 0)
                    foreach (var item in result.Tracks.Items.OrderByDescending(i => i.Popularity))
                    {
                        if (item is null) continue;

                        ret.Add(item);
                    }

                if (string.IsNullOrEmpty(result?.Tracks?.Next)) break;

                uri = result.Tracks.Next;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for track");
        }
        return ret;
    }
}