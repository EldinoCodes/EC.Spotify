using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models.Auth;
using EC.Spotify.Tests.Core.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EC.Spotify.Tests.Mocks.Services;

/// <summary>
/// Provides methods for managing authorization tokens and facilitating authentication with the Spotify API.
/// </summary>
/// <remarks>This class extends the base Spotify.Services.AuthorizationService and implements
/// IAuthorizationService. It is intended for use in scenarios where token retrieval and management are required for
/// Spotify API interactions.</remarks>
/// <param name="logger">The logger used to record authorization-related events and errors.</param>
/// <param name="spotifyOptions">The configuration options containing Spotify API credentials and settings.</param>
/// <param name="httpSpotifyProvider">The provider responsible for executing HTTP requests to the Spotify API.</param>
/// <param name="spotifyJsonProvider">The provider used for serializing and deserializing JSON data from Spotify API responses.</param>
/// <param name="memoryCache">The memory cache used to store authorization tokens and related data for efficient retrieval.</param>
internal class MockAuthorizationService(ILogger<MockAuthorizationService> logger, IConfiguration configuration, IOptions<SpotifyOptions> spotifyOptions, ISpotifyHttpProvider httpSpotifyProvider, ISpotifyJsonProvider spotifyJsonProvider, IMemoryCache memoryCache) 
    : Spotify.Services.AuthorizationService(logger, spotifyOptions, httpSpotifyProvider, spotifyJsonProvider, memoryCache), IAuthorizationService
{
    private readonly IConfiguration _configuration = configuration;
    public new Task<AuthToken?> AuthorizationTokenGetAsync(CancellationToken cancellationToken = default)
        => _configuration.GetValue<bool>("FullEnd2EndTest") 
            ? base.AuthorizationTokenGetAsync(cancellationToken)
            : Task.FromResult(DummyProvider.DummyObject<AuthToken?>());
}
