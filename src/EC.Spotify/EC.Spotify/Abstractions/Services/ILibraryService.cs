using EC.Spotify.Models;
using EC.Spotify.Models.Library;

namespace EC.Spotify.Abstractions.Services;

public interface ILibraryService
{
    Task<SpotifyResult<List<bool>>> LibraryAddAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
    Task<SpotifyResult<bool>> LibraryAddAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);

    Task<SpotifyResult<List<bool>>> LibraryCheckAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
    Task<SpotifyResult<bool>> LibraryCheckAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);

    Task<SpotifyResult<List<bool>>> LibraryRemoveAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
    Task<SpotifyResult<bool>> LibraryRemoveAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);
}