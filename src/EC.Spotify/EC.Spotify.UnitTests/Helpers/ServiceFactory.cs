using EC.Spotify.Services;
using EC.Spotify.UnitTests.Mocks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace EC.Spotify.UnitTests.Helpers;

internal static class ServiceFactory
{
    internal static SpotifyOptions DefaultOptions(List<string>? scopes = null) => new()
    {
        ClientId = "test-client-id",
        ClientSecret = "test-client-secret",
        RedirectUri = "http://localhost/callback",
        Scopes = scopes ?? []
    };

    internal static IOptions<SpotifyOptions> OptionsFor(List<string>? scopes = null)
        => Options.Create(DefaultOptions(scopes));

    internal static AlbumService CreateAlbumService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<AlbumService>.Instance, OptionsFor(scopes), CreateUserService(provider, scopes), provider);

    internal static ArtistService CreateArtistService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<ArtistService>.Instance, OptionsFor(scopes), CreateUserService(provider, scopes), provider);

    internal static AudiobookService CreateAudiobookService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<AudiobookService>.Instance, OptionsFor(scopes), CreateUserService(provider, scopes), provider);

    internal static ChapterService CreateChapterService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<ChapterService>.Instance, OptionsFor(scopes), provider);

    internal static EpisodeService CreateEpisodeService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<EpisodeService>.Instance, OptionsFor(scopes), CreateUserService(provider, scopes), provider);

    internal static LibraryService CreateLibraryService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<LibraryService>.Instance, OptionsFor(scopes), provider);

    internal static PlayerService CreatePlayerService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<PlayerService>.Instance, OptionsFor(scopes), provider);

    internal static PlaylistService CreatePlaylistService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<PlaylistService>.Instance, OptionsFor(scopes), provider);

    internal static SearchService CreateSearchService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<SearchService>.Instance, OptionsFor(scopes), provider);

    internal static ShowService CreateShowService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<ShowService>.Instance, OptionsFor(scopes), provider);

    internal static TrackService CreateTrackService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<TrackService>.Instance, OptionsFor(scopes), provider);

    internal static UserService CreateUserService(MockSpotifyProvider provider, List<string>? scopes = null)
        => new(NullLogger<UserService>.Instance, OptionsFor(scopes), provider);

    internal static AuthorizationService CreateAuthorizationService(
        MockSpotifyHttpProvider? httpProvider = null,
        MockSpotifyJsonProvider? jsonProvider = null,
        IMemoryCache? memoryCache = null,
        List<string>? scopes = null)
    {
        httpProvider ??= new MockSpotifyHttpProvider();
        jsonProvider ??= new MockSpotifyJsonProvider();
        memoryCache ??= new MemoryCache(new MemoryCacheOptions());
        return new AuthorizationService(
            NullLogger<AuthorizationService>.Instance,
            OptionsFor(scopes),
            httpProvider,
            jsonProvider,
            memoryCache);
    }
}
