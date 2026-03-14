using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Abstractions;

public interface ISpotifyClient
{
    /// <summary>
    /// Gets the service to retrieve album data.
    /// </summary>
    IAlbumService Albums { get; }
    /// <summary>
    /// Gets the service to retrieve artist data.
    /// </summary>
    IArtistService Artists { get; }
    /// <summary>
    /// Gets the service to retrieve audiobook data.
    /// </summary>
    IAudiobookService Audiobooks { get; }
    /// <summary>
    /// Gets the service for managing Spotify Authorization.
    /// </summary>
    IAuthorizationService Authorization { get; }
    /// <summary>
    /// Gets the service to retrieve chapter data.
    /// </summary>
    IChapterService Chapters { get; }
    /// <summary>
    /// Gets the service to retrieve episode data.
    /// </summary>
    IEpisodeService Episodes { get; }
    /// <summary>
    /// Gets the service to manage your library.
    /// </summary>
    ILibraryService Library { get; }
    /// <summary>
    /// Gets the service used to control and query the media player.
    /// </summary>
    IPlayerService Player { get; }
    /// <summary>
    /// Gets the search service used to perform queries and retrieve results.
    /// </summary>
    ISearchService Search { get; }
    /// <summary>
    /// Gets the service to retrieve show data.
    /// </summary>
    IShowService Shows { get; }
    /// <summary>
    /// Gets the service to retrieve track data.
    /// </summary>
    ITrackService Tracks { get; }
}
