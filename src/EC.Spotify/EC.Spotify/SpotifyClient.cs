using EC.Spotify.Abstractions;
using EC.Spotify.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EC.Spotify;

public class SpotifyClient(IServiceProvider serviceProvider) : ISpotifyClient
{
    public IAlbumService Albums { get; } = serviceProvider.GetRequiredService<IAlbumService>();
    public IArtistService Artists { get; } = serviceProvider.GetRequiredService<IArtistService>();
    public IAudiobookService Audiobooks { get; } = serviceProvider.GetRequiredService<IAudiobookService>();
    public IAuthorizationService Authorization { get; } = serviceProvider.GetRequiredService<IAuthorizationService>();
    public IChapterService Chapters { get; } = serviceProvider.GetRequiredService<IChapterService>();
    public IEpisodeService Episodes { get; } = serviceProvider.GetRequiredService<IEpisodeService>();
    public ILibraryService Library { get; } = serviceProvider.GetRequiredService<ILibraryService>();
    public IPlayerService Player { get; } = serviceProvider.GetRequiredService<IPlayerService>();
    public IPlaylistService Playlists { get; } = serviceProvider.GetRequiredService<IPlaylistService>();
    public ISearchService Search { get; } = serviceProvider.GetRequiredService<ISearchService>();
    public IShowService Shows { get; } = serviceProvider.GetRequiredService<IShowService>();
    public ITrackService Tracks { get; } = serviceProvider.GetRequiredService<ITrackService>();
    public IUserService User { get; } = serviceProvider.GetRequiredService<IUserService>();
}
