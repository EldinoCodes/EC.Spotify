using EC.Spotify.Models;
using EC.Spotify.Models.Library;

namespace EC.Spotify.Abstractions.Services;

public interface ILibraryService
{
    Task<SpotifyResult<List<bool>>> LibraryAddAllAsync(List<LibraryItem> libraryItems, CancellationToken cancellationToken = default);
    Task<SpotifyResult<bool>> LibraryAddAsync(LibraryItem? libraryItem, CancellationToken cancellationToken = default);

    Task<SpotifyResult<List<bool>>> LibraryCheckAllAsync(List<LibraryItem> libraryItems, CancellationToken cancellationToken = default);
    Task<SpotifyResult<bool>> LibraryCheckAsync(LibraryItem? libraryItem, CancellationToken cancellationToken = default);

    Task<SpotifyResult<List<bool>>> LibraryRemoveAllAsync(List<LibraryItem> libraryItems, CancellationToken cancellationToken = default);
    Task<SpotifyResult<bool>> LibraryRemoveAsync(LibraryItem? libraryItem, CancellationToken cancellationToken = default);
}