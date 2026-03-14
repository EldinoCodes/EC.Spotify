using EC.Spotify.Abstractions;
using EC.Spotify.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EC.Spotify;

public class SpotifyClient(IServiceProvider serviceProvider) : ISpotifyClient
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IAlbumService Albums => _serviceProvider.GetRequiredService<IAlbumService>();
    public IArtistService Artists => _serviceProvider.GetRequiredService<IArtistService>();
    public IAudiobookService Audiobooks => _serviceProvider.GetRequiredService<IAudiobookService>();
    public IAuthorizationService Authorization => _serviceProvider.GetRequiredService<IAuthorizationService>();
    public IChapterService Chapters => _serviceProvider.GetRequiredService<IChapterService>();
    public IEpisodeService Episodes => _serviceProvider.GetRequiredService<IEpisodeService>();
    public ILibraryService Library => _serviceProvider.GetRequiredService<ILibraryService>();
    public IPlayerService Player => _serviceProvider.GetRequiredService<IPlayerService>();
    public ISearchService Search => _serviceProvider.GetRequiredService<ISearchService>();
    public IShowService Shows => _serviceProvider.GetRequiredService<IShowService>();
    public ITrackService Tracks => _serviceProvider.GetRequiredService<ITrackService>();
}
