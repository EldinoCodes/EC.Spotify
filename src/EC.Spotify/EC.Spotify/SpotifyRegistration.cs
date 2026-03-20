using EC.Spotify.Abstractions;
using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Providers;
using EC.Spotify.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EC.Spotify.Tests")]

namespace EC.Spotify;

public static class SpotifyRegistration
{
    public static IServiceCollection AddSpotify(this IServiceCollection services, Action<SpotifyOptions>? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        services.AddOptions<SpotifyOptions>().Configure(options);
        services.AddSpotifyServices();

        return services;
    }

    public static IServiceCollection AddSpotify(this IServiceCollection services, IConfigurationSection? configurationSection)
    {
        ArgumentNullException.ThrowIfNull(configurationSection);

        services.Configure<SpotifyOptions>(configurationSection);
        services.AddSpotifyServices();
        
        return services;
    }

    private static IServiceCollection AddSpotifyServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHttpClient<ISpotifyHttpProvider, SpotifyHttpProvider>();
        services.AddSingleton<ISpotifyJsonProvider, SpotifyJsonProvider>();
        services.AddSingleton<ISpotifyProvider, SpotifyProvider>();

        services.AddSingleton<IAlbumService, AlbumService>();
        services.AddSingleton<IArtistService, ArtistService>();
        services.AddSingleton<IAudiobookService, AudiobookService>();
        services.AddSingleton<IAuthorizationService, AuthorizationService>();
        services.AddSingleton<IChapterService, ChapterService>();
        services.AddSingleton<IEpisodeService, EpisodeService>();
        services.AddSingleton<ILibraryService, LibraryService>();
        services.AddSingleton<IPlayerService, PlayerService>();
        services.AddSingleton<IPlaylistService, PlaylistService>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<IShowService, ShowService>();
        services.AddSingleton<ITrackService, TrackService>();

        services.AddSingleton<ISpotifyClient, SpotifyClient>();

        return services;
    }
}
