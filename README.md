# EC.Spotify
[![Build And Test](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/BuildAndTest.yml/badge.svg)](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/BuildAndTest.yml)
[![Pack And Publish](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/PackAndPublish.yml/badge.svg)](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/PackAndPublish.yml)
[![NuGet Version](https://img.shields.io/nuget/v/EC.Spotify.svg?style=flat)](https://www.nuget.org/packages/EC.Spotify) 
[![NuGet Downloads](https://img.shields.io/nuget/dt/EC.Spotify)](https://www.nuget.org/packages/EC.Spotify)

A comprehensive .NET client library for the Spotify Web API, providing strongly-typed access to albums, artists, tracks, playlists, playback control, library management, and more.

> **Full documentation is available in the [Wiki](https://github.com/EldinoCodes/EC.Spotify/wiki/GettingStarted).**

## Latest News - 2026.09.06
### Version 1.1.2 - Documentation Enhancement
Comprehensive documentation updates covering all available Spotify Web API methods. Added raw methods documentation across all service wiki pages, enabling developers to access unprocessed JSON responses directly. All 30+ raw methods now fully documented with usage examples and parameter details.

### Raw Methods

EC.Spotify provides raw method counterparts for all typed service methods, returning unprocessed JSON responses as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response, useful for custom parsing or debugging.

**Example:**

```csharp
var json = await spotifyClient.Tracks.TrackGetRawAsync("1301WleyT98MSxVHPZCA6M", cancellationToken);
Console.WriteLine(json);
```

See the [Albums](https://github.com/EldinoCodes/EC.Spotify/wiki/Albums), [Artists](https://github.com/EldinoCodes/EC.Spotify/wiki/Artists), and other service wiki pages for complete raw method documentation.

## Installation

### Package Manager Console
```powershell
Install-Package EC.Spotify
```

### .NET CLI
```bash
dotnet add package EC.Spotify
```

## Configuration

Register EC.Spotify in your `IServiceCollection` using either a configuration delegate or an `IConfiguration` section. See the [Getting Started](https://github.com/EldinoCodes/EC.Spotify/wiki/GettingStarted) wiki page for full setup instructions.

```csharp
builder.Services.AddSpotify(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.RedirectUri = "https://localhost:5001/callback";
    options.Scopes = ["user-read-playback-state", "user-library-read"];
});
```

## ISpotifyClient

Inject `ISpotifyClient` to access all services:

| Property | Service | Wiki |
|----------|---------|------|
| `Albums` | `IAlbumService` | [Albums](https://github.com/EldinoCodes/EC.Spotify/wiki/Albums) |
| `Artists` | `IArtistService` | [Artists](https://github.com/EldinoCodes/EC.Spotify/wiki/Artists) |
| `Audiobooks` | `IAudiobookService` | [Audiobooks](https://github.com/EldinoCodes/EC.Spotify/wiki/Audiobooks) |
| `Authorization` | `IAuthorizationService` | [Authorization](https://github.com/EldinoCodes/EC.Spotify/wiki/Authorization) |
| `Chapters` | `IChapterService` | [Chapters](https://github.com/EldinoCodes/EC.Spotify/wiki/Chapters) |
| `Episodes` | `IEpisodeService` | [Episodes](https://github.com/EldinoCodes/EC.Spotify/wiki/Episodes) |
| `Library` | `ILibraryService` | [Library](https://github.com/EldinoCodes/EC.Spotify/wiki/Library) |
| `Player` | `IPlayerService` | [Player](https://github.com/EldinoCodes/EC.Spotify/wiki/Player) |
| `Playlists` | `IPlaylistService` | [Playlists](https://github.com/EldinoCodes/EC.Spotify/wiki/Playlists) |
| `Search` | `ISearchService` | [Search](https://github.com/EldinoCodes/EC.Spotify/wiki/Search) |
| `Shows` | `IShowService` | [Shows](https://github.com/EldinoCodes/EC.Spotify/wiki/Shows) |
| `Tracks` | `ITrackService` | [Tracks](https://github.com/EldinoCodes/EC.Spotify/wiki/Tracks) |
| `User` | `IUserService` | [User](https://github.com/EldinoCodes/EC.Spotify/wiki/User) |

## Error Handling

All service methods return `SpotifyResult<T>`:

- `IsSuccess` — `true` when the request succeeded
- `Data` — the result data (populated on success)
- `Error` — a `SpotifyError` with `Status`, `Message`, and `Reason` (populated on failure)

```csharp
var result = await _spotifyClient.Albums.AlbumGetAsync(albumId);
if (result.IsSuccess)
    Console.WriteLine(result.Data.Name);
else
    Console.WriteLine($"{result.Error?.Status}: {result.Error?.Message}");
```

## Requirements

- .NET 10
- Valid Spotify Developer credentials ([Get them here](https://developer.spotify.com/dashboard))
- Valid Spotify Premium account for playback control features

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.

## Contributing

Bring issues to the table, or suggest features! Contributions are welcome and appreciated.

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/EldinoCodes/EC.Spotify).

## Acknowledgments

Built for people who enjoy their Spotify Premium account and want to interact with its API features. Great appreciation for the Spotify development team!
