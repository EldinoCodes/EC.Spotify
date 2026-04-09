using EC.Spotify.Abstractions;
using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Providers;
using EC.Spotify.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EC.Spotify.Tests")]
[assembly: InternalsVisibleTo("EC.Spotify.UnitTests")]

namespace EC.Spotify;

/// <summary>
/// Provides extension methods for registering EC.Spotify services with an <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
/// </summary>
public static class SpotifyRegistration
{
    /// <summary>
    /// Registers EC.Spotify services with the specified <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> using a configuration action.
    /// </summary>
    /// <param name="services">The service collection to add EC.Spotify services to.</param>
    /// <param name="options">An action to configure <see cref="SpotifyOptions"/>. Cannot be null.</param>
    /// <returns>The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
    public static IServiceCollection AddSpotify(this IServiceCollection services, Action<SpotifyOptions>? options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        services.AddOptions<SpotifyOptions>().Configure(options);
        services.AddSpotifyServices();

        return services;
    }

    /// <summary>
    /// Registers EC.Spotify services with the specified <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> using a configuration section.
    /// </summary>
    /// <param name="services">The service collection to add EC.Spotify services to.</param>
    /// <param name="configurationSection">The configuration section to bind <see cref="SpotifyOptions"/> from. Cannot be null.</param>
    /// <returns>The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationSection"/> is null.</exception>
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
        services.AddHttpClient(nameof(SpotifyHttpProvider));
        services.AddSingleton<ISpotifyHttpProvider, SpotifyHttpProvider>();
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

        services.AddSingleton<IUserService, UserService>();

        services.AddSingleton<ISpotifyClient, SpotifyClient>();

        return services;
    }
}
