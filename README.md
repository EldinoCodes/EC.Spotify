# EC.Spotify
[![Build And Test](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/BuildAndTest.yml/badge.svg)](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/BuildAndTest.yml)
[![Pack And Publish](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/PackAndPublish.yml/badge.svg)](https://github.com/EldinoCodes/EC.Spotify/actions/workflows/PackAndPublish.yml)
[![NuGet Version](https://img.shields.io/nuget/v/EC.Spotify.svg?style=flat)](https://www.nuget.org/packages/EC.Spotify) 
[![NuGet Downloads](https://img.shields.io/nuget/dt/EC.Spotify)](https://www.nuget.org/packages/EC.Spotify)


A comprehensive .NET client library for the Spotify Web API, providing a clean and intuitive interface for interacting with Spotify's music streaming platform.

## LATEST NEWS - 2026.04.09
### Version 1.1.1 - In Progress
Updated EC.Spotify to expose Raw methods (returning direct json results) with the intention that developers can handle the raw JSON responses themselves.  Added AI Generated Unit Tests to try to further validate methods.  Intend to try to refactor for stability and simplicity before next release.  Thanks for using EC.Spotify!

## Overview

offering strongly-typed access to albums, artists, tracks, playlists, playback control, library management, and more.

## Installation

### Using Package Manager Console
```powershell
Install-Package EC.Spotify
```

### Using .NET CLI
```bash
dotnet add package EC.Spotify
```

## Configuration

The library provides two methods for registering services in your dependency injection container through the `SpotifyRegistration` class:

### Method 1: Using Configuration Delegate

Register Spotify services by providing configuration options through an `Action<SpotifyOptions>` delegate:

```csharp
using EC.Spotify;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSpotify(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.RedirectUri = "https://localhost:5001/callback";
    options.Scopes = new List<string>
    {
      "user-read-currently-playing",
      "user-read-playback-state",
      "user-modify-playback-state",
      "user-library-read",
      "user-library-modify"
    };
});
```

### Method 2: Using IConfiguration Section

Register Spotify services by binding to a configuration section (e.g., from `appsettings.json`):

**appsettings.json:**
```json
{
  "Spotify": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "RedirectUri": "https://localhost:5001/callback",
    "Scopes": [
      "ugc-image-upload", // PlaylistService.PlaylistImageAddAsync
      "user-read-currently-playing", // PlayerService.QueueGetAsync
      "user-read-playback-state", // PlayerService.QueueGetAsync, DeviceGetAllAsync, StateGetAsync, CurrentlyPlayingGetAsync
      "user-modify-playback-state", // PlayerService.QueueAddAsync, TransferAsync, PlayAsync, PauseAsync, NextAsync, PreviousAsync, SeekAsync, RepeatAsync, ShuffleAsync, VolumeAsync
      "user-read-playback-position", // EpisodeService.EpisodeGetAsync | ShowService.ShowGetAsync, ShowEpisodeGetAllAsync | UserService.MyEpisodeGetAllAsync, MyShowGetAllAsync
      "user-library-read", // LibraryService.LibraryCheckAsync, LibraryCheckAllAsync | UserService.MyAlbumGetAllAsync, MyAudiobookGetAllAsync, MyEpisodeGetAllAsync, MyShowGetAllAsync, MyTrackGetAllAsync
      "user-library-modify", // LibraryService.LibraryAddAsync, LibraryAddAllAsync, LibraryRemoveAsync, LibraryRemoveAllAsync
      "user-top-read", // UserService.MyTopItemGetAllAsync
      "playlist-read-private", // PlaylistService.PlaylistItemGetAllAsync
      "playlist-modify-public", // PlaylistService.PlaylistDetailUpdateAsync, PlaylistItemAddAsync, PlaylistItemAddAllAsync, PlaylistItemRemoveAsync, PlaylistItemRemoveAllAsync, PlaylistImageAddAsync
      "playlist-modify-private" // PlaylistService.PlaylistDetailUpdateAsync, PlaylistItemAddAsync, PlaylistItemAddAllAsync, PlaylistItemRemoveAsync, PlaylistItemRemoveAllAsync, PlaylistImageAddAsync
    ]
  }
}
```

**Program.cs:**
```csharp
using EC.Spotify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Get the Spotify configuration section
var spotifySection = builder.Configuration.GetSection("Spotify");

// Register Spotify services
builder.Services.AddSpotify(spotifySection);

var app = builder.Build();
```

## Core Components

### ISpotifyClient

The `ISpotifyClient` interface is the primary entry point for interacting with the Spotify API. It provides access to all available service interfaces, acting as a facade for the entire library.

| Property | Service |
|----------|---------|
| `Albums` | `IAlbumService` |
| `Artists` | `IArtistService` |
| `Audiobooks` | `IAudiobookService` |
| `Authorization` | `IAuthorizationService` |
| `Chapters` | `IChapterService` |
| `Episodes` | `IEpisodeService` |
| `Library` | `ILibraryService` |
| `Player` | `IPlayerService` |
| `Playlists` | `IPlaylistService` |
| `Search` | `ISearchService` |
| `Shows` | `IShowService` |
| `Tracks` | `ITrackService` |
| `User` | `IUserService` |

**Usage Example:**
```csharp
public class MusicController : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient;

    public MusicController(ISpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
    }

    public async Task<IActionResult> GetAlbum(string albumId)
    {
        var result = await _spotifyClient.Albums.AlbumGetAsync(albumId);
        return result.IsSuccess ? Ok(result.Data) : NotFound();
    }
}
```

## Service Interfaces

### IAuthorizationService

Manages Spotify OAuth 2.0 authorization flow and token management. This service is critical for authenticating users and maintaining access tokens.

**Available Methods:**
- **`Validate(CancellationToken cancellationToken = default)`**  
  Validates the current authentication state and returns an authorization URL if user authorization is required.

- **`AuthorizationCodeUrl()`**  
  Generates the OAuth 2.0 authorization URL for initiating the authorization code flow.

- **`AuthorizationCodeAddAsync(string? authorizationCode, string? state = null, CancellationToken cancellationToken = default)`**  
  Stores an authorization code received from the OAuth callback. The optional `state` parameter is validated against the CSRF token generated by `AuthorizationCodeUrl()`.

- **`AuthorizationCodeGetAsync(CancellationToken cancellationToken = default)`**  
  Retrieves the currently stored authorization code.

- **`AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default)`**  
  Removes the stored authorization code from the underlying store.

- **`AuthorizationTokenGetAsync(CancellationToken cancellationToken = default)`**  
  Retrieves the current authentication token with access and refresh tokens.

- **`AuthorizationTokenReset()`**  
  Resets the authentication token, forcing re-authentication.

**Example:**
```csharp
// Check if authorization is needed
var authUrl = await _spotifyClient.Authorization.Validate();
if (authUrl != null)
{
    // Redirect user to Spotify authorization page
    return Redirect(authUrl);
}

// Alternative: Generate authorization URL manually
var manualAuthUrl = _spotifyClient.Authorization.AuthorizationCodeUrl();

// Handle OAuth callback — Spotify returns both 'code' and 'state' query parameters
[HttpGet("callback")]
public async Task<IActionResult> SpotifyCallback(string code, string? state)
{
    var success = await _spotifyClient.Authorization.AuthorizationCodeAddAsync(code, state);
    if (success)
    {
        return RedirectToAction("Index");
    }
    return BadRequest("Authorization failed");
}

// Get current token
var token = await _spotifyClient.Authorization.AuthorizationTokenGetAsync();
if (token != null)
{
    Console.WriteLine($"Access Token: {token.AccessToken}");
    Console.WriteLine($"Expires In: {token.ExpiresIn}s");
}

// Remove authorization
await _spotifyClient.Authorization.AuthorizationCodeRemoveAsync();
await _spotifyClient.Authorization.AuthorizationTokenReset();
```

### IAlbumService

Provides methods for retrieving album data from Spotify.

**Available Methods:**
- **`AlbumGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed album information by album ID, including track list, artists, release date, and more.

- **`AlbumTrackGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated tracks for a specific album with support for limit and offset parameters.

- **`AlbumGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for an album by album ID.

- **`AlbumTrackGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of tracks for a specific album.

**Example:**
```csharp
// Get album details
var albumResult = await _spotifyClient.Albums.AlbumGetAsync("4aawyAB9vmqN3uQ7FjRGTy");
if (albumResult.IsSuccess)
{
    var album = albumResult.Data;
    Console.WriteLine($"Album: {album.Name} by {album.Artists[0].Name}");
    Console.WriteLine($"Release Date: {album.ReleaseDate}");
    Console.WriteLine($"Total Tracks: {album.TotalTracks}");
}

// Get album tracks with pagination
var tracksResult = await _spotifyClient.Albums.AlbumTrackGetAllAsync(
    "4aawyAB9vmqN3uQ7FjRGTy", 
    limit: 50, 
    offset: 0
);
if (tracksResult.IsSuccess)
{
    foreach (var track in tracksResult.Data.Items)
    {
        Console.WriteLine($"Track: {track.Name} - {track.DurationMilliseconds}ms");
    }
}
```

### IArtistService

Provides methods for retrieving artist data and their albums.

**Available Methods:**
- **`ArtistGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed artist information including name, images, and external URLs.

- **`ArtistAlbumGetAllAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated albums for an artist, optionally filtered by `albumTypes` (e.g., `AlbumType.Album`, `AlbumType.Single`).

- **`ArtistGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for an artist by artist ID.

- **`ArtistAlbumGetAllRawAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of albums for a specific artist.

**Example:**
```csharp
// Get artist details
var artistResult = await _spotifyClient.Artists.ArtistGetAsync("0TnOYISbd1XYRBk9myaseg");
if (artistResult.IsSuccess)
{
    var artist = artistResult.Data;
    Console.WriteLine($"Artist: {artist.Name}");
    Console.WriteLine($"Id: {artist.Id}");
}

// Get artist albums (only albums and singles)
var albumsResult = await _spotifyClient.Artists.ArtistAlbumGetAllAsync(
    "0TnOYISbd1XYRBk9myaseg",
    albumTypes: AlbumType.Album | AlbumType.Single,
    limit: 10,
    offset: 0
);
```

### IAudiobookService

Provides methods for retrieving audiobook data and chapters.

**Available Methods:**
- **`AudiobookGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed audiobook information including title, author, narrator, description, and total chapters.

- **`AudiobookChapterGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated chapters for a specific audiobook.

- **`AudiobookGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for an audiobook by audiobook ID.

- **`AudiobookChapterGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of chapters for a specific audiobook.

**Example:**
```csharp
// Get audiobook details
var audiobookResult = await _spotifyClient.Audiobooks.AudiobookGetAsync("7iHfbu1YPACw6oZPAFJtqe");
if (audiobookResult.IsSuccess)
{
    var audiobook = audiobookResult.Data;
    Console.WriteLine($"Audiobook: {audiobook.Name}");
    Console.WriteLine($"Author: {string.Join(", ", audiobook.Authors.Select(a => a.Name))}");
    Console.WriteLine($"Total Chapters: {audiobook.TotalChapters}");
}

// Get audiobook chapters
var chaptersResult = await _spotifyClient.Audiobooks.AudiobookChapterGetAllAsync(
    "7iHfbu1YPACw6oZPAFJtqe",
    limit: 50
);
```

### IChapterService

Provides methods for retrieving individual audiobook chapter data.

**Available Methods:**
- **`ChapterGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed chapter information including name, description, duration, and chapter number.

- **`ChapterGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a chapter by chapter ID.

**Example:**
```csharp
var chapterResult = await _spotifyClient.Chapters.ChapterGetAsync("0D5wENdkdwbqlrHoaJ9g29");
if (chapterResult.IsSuccess)
{
    var chapter = chapterResult.Data;
    Console.WriteLine($"Chapter: {chapter.Name}");
    Console.WriteLine($"Chapter Number: {chapter.ChapterNumber}");
    Console.WriteLine($"Duration: {chapter.DurationMilliseconds}ms");
    Console.WriteLine($"Description: {chapter.Description}");
}
```

### IEpisodeService

Provides methods for retrieving podcast episode data.

**Available Methods:**
- **`EpisodeGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed episode information including name, description, duration, release date, and show information. Requires the `user-read-playback-position` scope.

- **`EpisodeGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for an episode by episode ID. Requires the `user-read-playback-position` scope.

**Example:**
```csharp
var episodeResult = await _spotifyClient.Episodes.EpisodeGetAsync("512ojhOuo1ktJprKbVcKyQ");
if (episodeResult.IsSuccess)
{
    var episode = episodeResult.Data;
    Console.WriteLine($"Episode: {episode.Name}");
    Console.WriteLine($"Show: {episode.Show?.Name}");
    Console.WriteLine($"Duration: {episode.DurationMilliseconds}ms");
    Console.WriteLine($"Release Date: {episode.ReleaseDate}");
    Console.WriteLine($"Description: {episode.Description}");
}
```

### ILibraryService

Provides methods for checking, adding, and removing items from the current user's Spotify library. Supports batching up to 40 items per request (a Spotify-imposed limit), automatically chunking larger lists across multiple requests.

**Available Methods:**
- **`LibraryCheckAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)`**  
  Checks whether a single item is saved in the current user's library. Requires the `user-library-read` scope.

- **`LibraryCheckAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)`**  
  Checks whether multiple items are saved in the current user's library. Returns a `List<bool>` in the same order as the input items. Requires the `user-library-read` scope.

- **`LibraryAddAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)`**  
  Saves a single item to the current user's library. Requires the `user-library-modify` scope.

- **`LibraryAddAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)`**  
  Saves multiple items to the current user's library. Returns a `List<bool>` indicating success for each item. Requires the `user-library-modify` scope.

- **`LibraryRemoveAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default)`**  
  Removes a single item from the current user's library. Requires the `user-library-modify` scope.

- **`LibraryRemoveAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)`**  
  Removes multiple items from the current user's library. Returns a `List<bool>` indicating success for each item. Requires the `user-library-modify` scope.

**`ReferenceItem` Model:**

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string?` | The Spotify ID of the item |
| `Type` | `ReferenceItemType` | The type of the item (`Album`, `Audiobook`, `Episode`, `Playlist`, `Show`, `Track`, `User`) |
| `Uri` | `string?` | Read-only. Computed Spotify URI in the format `spotify:{type}:{id}` |

**Example:**
```csharp
// Check if a single track is saved
var trackItem = new ReferenceItem { Id = "3n3Ppam7vgaVa1iaRUc9Lp", Type = ReferenceItemType.Track };
var checkResult = await _spotifyClient.Library.LibraryCheckAsync(trackItem);
if (checkResult.IsSuccess)
{
    Console.WriteLine($"Track is saved: {checkResult.Data}");
}

// Check multiple items at once
var items = new List<ReferenceItem>
{
    new() { Id = "3n3Ppam7vgaVa1iaRUc9Lp", Type = ReferenceItemType.Track },
    new() { Id = "4aawyAB9vmqN3uQ7FjRGTy", Type = ReferenceItemType.Album }
};
var checkAllResult = await _spotifyClient.Library.LibraryCheckAllAsync(items);
if (checkAllResult.IsSuccess)
{
    for (int i = 0; i < items.Count; i++)
        Console.WriteLine($"{items[i].Uri} saved: {checkAllResult.Data[i]}");
}

// Save a track to the library
var addResult = await _spotifyClient.Library.LibraryAddAsync(trackItem);

// Save multiple items
var addAllResult = await _spotifyClient.Library.LibraryAddAllAsync(items);

// Remove a track from the library
var removeResult = await _spotifyClient.Library.LibraryRemoveAsync(trackItem);

// Remove multiple items
var removeAllResult = await _spotifyClient.Library.LibraryRemoveAllAsync(items);
```

### IPlayerService

Controls Spotify playback and manages player state.

**Available Methods:**
- **`QueueGetAsync(CancellationToken cancellationToken = default)`** — Retrieves the current playback queue. Requires the `user-read-currently-playing` and `user-read-playback-state` scopes.
- **`QueueAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default)`** — Adds a track to the playback queue. Requires the `user-modify-playback-state` scope.
- **`DeviceGetAllAsync(CancellationToken cancellationToken = default)`** — Retrieves all available playback devices. Requires the `user-read-playback-state` scope.
- **`TransferAsync(string? deviceId, bool play = false, CancellationToken cancellationToken = default)`** — Transfers playback to a specific device. Requires the `user-modify-playback-state` scope.
- **`StateGetAsync(CancellationToken cancellationToken = default)`** — Retrieves the full current playback state including active device, track, and shuffle/repeat modes. Requires the `user-read-playback-state` scope.
- **`CurrentlyPlayingGetAsync(CancellationToken cancellationToken = default)`** — Retrieves the currently playing item and its playback context. Requires the `user-read-playback-state` scope.
- **`PlayAsync(string? deviceId, List<string>? trackUris, CancellationToken cancellationToken = default)`** — Starts playback. Requires the `user-modify-playback-state` scope.
- **`PauseAsync(string? deviceId = null, CancellationToken cancellationToken = default)`** — Pauses playback. Requires the `user-modify-playback-state` scope.
- **`NextAsync(string? deviceId = null, CancellationToken cancellationToken = default)`** — Skips to the next track. Requires the `user-modify-playback-state` scope.
- **`PreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default)`** — Skips to the previous track. Requires the `user-modify-playback-state` scope.
- **`SeekAsync(int positionMs, string? deviceId = null, CancellationToken cancellationToken = default)`** — Seeks to a position in the current track. Requires the `user-modify-playback-state` scope.
- **`RepeatAsync(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)`** — Sets the repeat mode. Requires the `user-modify-playback-state` scope.
- **`ShuffleAsync(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)`** — Enables or disables shuffle. Requires the `user-modify-playback-state` scope.
- **`VolumeAsync(int volumePercent, string? deviceId = null, CancellationToken cancellationToken = default)`** — Sets the playback volume. Requires the `user-modify-playback-state` scope.

**Example:**
```csharp
// Get available devices
var devicesResult = await _spotifyClient.Player.DeviceGetAllAsync();

// Get full playback state
var stateResult = await _spotifyClient.Player.StateGetAsync();
if (stateResult.IsSuccess)
{
    if (stateResult.Data.Item is Track track)
        Console.WriteLine($"Playing: {track.Name}");
    Console.WriteLine($"Device: {stateResult.Data.Device?.Name}");
}

// Get only the currently playing item
var nowPlaying = await _spotifyClient.Player.CurrentlyPlayingGetAsync();

// Start playback
var trackUris = new List<string> { "spotify:track:6rqhFgbbKwnb9MLmUQDhG6" };
await _spotifyClient.Player.PlayAsync(deviceId: null, trackUris);

// Control playback
await _spotifyClient.Player.PauseAsync();
await _spotifyClient.Player.NextAsync();
await _spotifyClient.Player.VolumeAsync(volumePercent: 75);

// Set playback modes
await _spotifyClient.Player.RepeatAsync(PlayerRepeatMode.Track);
await _spotifyClient.Player.ShuffleAsync(PlayerShuffleMode.On);
```

### IPlaylistService

Provides methods for retrieving, managing, and modifying playlists and their contents.

**Available Methods:**
- **`PlaylistGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed playlist information by playlist ID.

- **`PlaylistItemGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated items from a specific playlist. Requires the `playlist-read-private` scope.

- **`PlaylistDetailUpdateAsync(string? id, PlaylistDetail? playlistDetail, CancellationToken cancellationToken = default)`**  
  Updates the details of an existing playlist (name, description, public/collaborative status). Requires the `playlist-modify-public` and `playlist-modify-private` scopes.

- **`PlaylistItemAddAsync(string? id, ReferenceItem? libraryItem, int? position = null, CancellationToken cancellationToken = default)`**  
  Adds a single item to a playlist at an optional position. If position is null, the item is appended to the end. Requires the `playlist-modify-public` and `playlist-modify-private` scopes.

- **`PlaylistItemAddAllAsync(string? id, List<ReferenceItem> libraryItems, int? position = null, CancellationToken cancellationToken = default)`**  
  Adds multiple items to a playlist in batches of up to 100 per request. Returns a `List<bool>` indicating success for each item. Requires the `playlist-modify-public` and `playlist-modify-private` scopes.

- **`PlaylistItemRemoveAsync(string? id, ReferenceItem? libraryItem, CancellationToken cancellationToken = default)`**  
  Removes a single item from a playlist. Requires the `playlist-modify-public` and `playlist-modify-private` scopes.

- **`PlaylistItemRemoveAllAsync(string? id, List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default)`**  
  Removes multiple items from a playlist in batches of up to 100 per request. Returns a `List<bool>` indicating success for each item. Requires the `playlist-modify-public` and `playlist-modify-private` scopes.

- **`PlaylistImageAddAsync(string? id, byte[]? imageData, CancellationToken cancellationToken = default)`**  
  Adds or replaces the cover image of a playlist. The image must be in JPEG format. Requires the `ugc-image-upload`, `playlist-modify-public`, and `playlist-modify-private` scopes.

- **`PlaylistImageGetAllAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves all images associated with the specified playlist.

- **`PlaylistGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a playlist by playlist ID.

- **`PlaylistItemGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of items in a playlist. Requires the `playlist-read-private` scope.

- **`PlaylistImageGetAllRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for all images associated with a playlist.

**`PlaylistDetail` Model:**

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string?` | The Spotify ID of the playlist |
| `Name` | `string?` | The name of the playlist |
| `Public` | `bool?` | Whether the playlist is publicly visible |
| `Collaborative` | `bool` | Whether the playlist is collaborative |
| `Description` | `string?` | The description of the playlist |

**`ReferenceItem` Model:**

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string?` | The Spotify ID of the item |
| `Type` | `ReferenceItemType` | The type of the item (`Album`, `Audiobook`, `Episode`, `Playlist`, `Show`, `Track`, `User`) |
| `Uri` | `string?` | Read-only. Computed Spotify URI in the format `spotify:{type}:{id}` |

**Example:**
```csharp
// Get a specific playlist
var playlistResult = await _spotifyClient.Playlists.PlaylistGetAsync("37i9dQZF1DXcBWIGoYBM5M");
if (playlistResult.IsSuccess)
{
    Console.WriteLine($"Playlist: {playlistResult.Data.Name}");
}

// Get playlist items with pagination
var itemsResult = await _spotifyClient.Playlists.PlaylistItemGetAllAsync(
    "37i9dQZF1DXcBWIGoYBM5M",
    limit: 50,
    offset: 0
);

// Update playlist details
var detail = new PlaylistDetail
{
    Name = "My Updated Playlist",
    Description = "A freshly updated playlist",
    Public = true,
    Collaborative = false
};
await _spotifyClient.Playlists.PlaylistDetailUpdateAsync("37i9dQZF1DXcBWIGoYBM5M", detail);

// Add a single item to a playlist
var item = new ReferenceItem { Id = "3n3Ppam7vgaVa1iaRUc9Lp", Type = ReferenceItemType.Track };
await _spotifyClient.Playlists.PlaylistItemAddAsync("37i9dQZF1DXcBWIGoYBM5M", item, position: 0);

// Add multiple items to a playlist
var items = new List<ReferenceItem>
{
    new() { Id = "3n3Ppam7vgaVa1iaRUc9Lp", Type = ReferenceItemType.Track },
    new() { Id = "6rqhFgbbKwnb9MLmUQDhG6", Type = ReferenceItemType.Track }
};
var addAllResult = await _spotifyClient.Playlists.PlaylistItemAddAllAsync("37i9dQZF1DXcBWIGoYBM5M", items);

// Remove a single item from a playlist
await _spotifyClient.Playlists.PlaylistItemRemoveAsync("37i9dQZF1DXcBWIGoYBM5M", item);

// Remove multiple items from a playlist
var removeAllResult = await _spotifyClient.Playlists.PlaylistItemRemoveAllAsync("37i9dQZF1DXcBWIGoYBM5M", items);

// Update playlist cover image
byte[] imageData = await File.ReadAllBytesAsync("cover.jpg");
await _spotifyClient.Playlists.PlaylistImageAddAsync("37i9dQZF1DXcBWIGoYBM5M", imageData);

// Get playlist images
var imagesResult = await _spotifyClient.Playlists.PlaylistImageGetAllAsync("37i9dQZF1DXcBWIGoYBM5M");
```

### ISearchService

Performs search queries across Spotify's catalog, supporting multiple content types.

**Available Methods:**
- **`SearchAsync(string? query, SearchType? searchType = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Performs a search using the specified query string and search type. The `searchType` parameter is a bitwise enum that can be combined using the `|` operator to search multiple content types simultaneously (e.g., `SearchType.Track | SearchType.Artist`). The `limit` parameter controls the maximum number of results to return (default: 5), and `offset` allows for pagination (default: 0).

- **`SearchRawAsync(string? query, SearchType? searchType = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Performs a search against the Spotify catalog and returns the raw JSON response.

<p style=

**Example:**
```csharp
// Search for tracks and artists matching "Bohemian Rhapsody"
var searchResult = await _spotifyClient.Search.SearchAsync(
    query: "Bohemian Rhapsody",
    searchType: SearchType.Track | SearchType.Artist,
    limit: 10,
    offset: 0
);

if (searchResult.IsSuccess)
{
    var tracks = searchResult.Data.Items?.Where(i => i.GetType() == typeof(Track))?.ToList() ?? [];
    var artists = searchResult.Data.Items?.Where(i => i.GetType() == typeof(Artist))?.ToList() ?? [];
    
    Console.WriteLine($"Found {tracks.Count} tracks and {artists.Count} artists");
}

// Search for albums only
var albumSearch = await _spotifyClient.Search.SearchAsync(
    query: "Abbey Road",
    searchType: SearchType.Album,
    limit: 5
);

// Search multiple types with pagination
var multiSearch = await _spotifyClient.Search.SearchAsync(
    query: "The Beatles",
    searchType: SearchType.Album | SearchType.Track | SearchType.Artist,
    limit: 20,
    offset: 0
);
```

### IShowService

Provides methods for retrieving podcast show data and episodes.

**Available Methods:**
- **`ShowGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed show information by show ID. Requires the `user-read-playback-position` scope.

- **`ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated episodes for a specific show. Requires the `user-read-playback-position` scope.

- **`ShowGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a show by show ID. Requires the `user-read-playback-position` scope.

- **`ShowEpisodeGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of episodes for a specific show. Requires the `user-read-playback-position` scope.

**Example:**
```csharp
var showResult = await _spotifyClient.Shows.ShowGetAsync("38bS44xjbVVZ3No3ByF1dJ");
if (showResult.IsSuccess)
{
    Console.WriteLine($"Show: {showResult.Data.Name}");
}

var episodesResult = await _spotifyClient.Shows.ShowEpisodeGetAllAsync("38bS44xjbVVZ3No3ByF1dJ", limit: 20);
```

### ITrackService

Provides methods for retrieving individual track data.

**Available Methods:**
- **`TrackGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed track information including name, artists, album, and duration.

- **`TrackGetRawAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a track by track ID.

**Example:**
```csharp
var trackResult = await _spotifyClient.Tracks.TrackGetAsync("3n3Ppam7vgaVa1iaRUc9Lp");
if (trackResult.IsSuccess)
{
    var track = trackResult.Data;
    Console.WriteLine($"Track: {track.Name} by {track.Artists[0].Name}");
    Console.WriteLine($"Duration: {track.DurationMilliseconds}ms");
}
```

### IUserService

Provides methods for retrieving items from the current user's Spotify library and top listening history.

**Available Methods:**
- **`MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves a paginated list of albums saved in the current user's library. The `limit` value must be between 1 and 50. Requires the `user-library-read` scope.

- **`MyAudiobookGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves a paginated list of audiobooks saved in the current user's library. The `limit` value must be between 1 and 50. Requires the `user-library-read` scope.

- **`MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves a paginated list of episodes saved in the current user's library. The `limit` value must be between 1 and 50. Requires the `user-library-read` and `user-read-playback-position` scopes.

- **`MyPlaylistGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves a paginated list of playlists owned or followed by the current user. The `limit` value must be between 1 and 50. Requires the `playlist-read-private` scope.

- **`MyShowGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**
  Retrieves a paginated list of shows saved in the current user's library. The `limit` value must be between 1 and 50. Requires the `user-library-read` and `user-read-playback-position` scopes.

- **`MyTrackGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves a paginated list of tracks saved in the current user's library. The `limit` value must be between 1 and 50. Requires the `user-library-read` scope.

- **`MyTopItemGetAllAsync(UserTopType userTopType = UserTopType.Tracks, UserTopTimeRange userTopTimeRange = UserTopTimeRange.MediumTerm, int? limit = 20, int offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the current user's top artists or tracks based on calculated affinity over a given time range. The `limit` value must be between 1 and 50. Requires the `user-top-read` scope.

- **`MyAlbumGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of albums saved in the current user's library. Requires the `user-library-read` scope.

- **`MyAudiobookGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of audiobooks saved in the current user's library. Requires the `user-library-read` scope.

- **`MyEpisodeGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of episodes saved in the current user's library. Requires the `user-library-read` and `user-read-playback-position` scopes.

- **`MyPlaylistGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of the current user's playlists. Requires the `playlist-read-private` scope.

- **`MyShowGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of shows saved in the current user's library. Requires the `user-library-read` and `user-read-playback-position` scopes.

- **`MyTrackGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for a paginated list of tracks saved in the current user's library. Requires the `user-library-read` scope.

- **`MyTopItemGetAllRawAsync(UserTopType userTopType = UserTopType.Tracks, UserTopTimeRange userTopTimeRange = UserTopTimeRange.MediumTerm, int? limit = 20, int offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves the raw JSON response for the current user's top artists or tracks. Requires the `user-top-read` scope.

**`UserTopType` Enum:**

| Value | Description |
|-------|-------------|
| `Artists` | Retrieve the user's top artists |
| `Tracks` | Retrieve the user's top tracks |

**`UserTopTimeRange` Enum:**

| Value | Description |
|-------|-------------|
| `LongTerm` | Calculated from several years of data including all new data as it becomes available |
| `MediumTerm` | Approximately last 6 months |
| `ShortTerm` | Approximately last 4 weeks |

**Example:**
```csharp
// Get saved albums with pagination
var albumsResult = await _spotifyClient.User.MyAlbumGetAllAsync(limit: 20, offset: 0);
if (albumsResult.IsSuccess)
{
    foreach (var album in albumsResult.Data.Items)
    {
        Console.WriteLine($"Album: {album.Name}");
    }
    Console.WriteLine($"Total saved albums: {albumsResult.Data.Total}");
}

// Get saved audiobooks
var audiobooksResult = await _spotifyClient.User.MyAudiobookGetAllAsync(limit: 20);
if (audiobooksResult.IsSuccess)
{
    foreach (var audiobook in audiobooksResult.Data.Items)
    {
        Console.WriteLine($"Audiobook: {audiobook.Name}");
    }
}

// Get saved episodes
var episodesResult = await _spotifyClient.User.MyEpisodeGetAllAsync(limit: 50);
if (episodesResult.IsSuccess)
{
    foreach (var episode in episodesResult.Data.Items)
    {
        Console.WriteLine($"Episode: {episode.Name} — {episode.Show?.Name}");
    }
}

// Get saved playlists
var playlistsResult = await _spotifyClient.User.MyPlaylistGetAllAsync(limit: 20);
if (playlistsResult.IsSuccess)
{
    foreach (var playlist in playlistsResult.Data.Items)
    {
        Console.WriteLine($"Playlist: {playlist.Name}");
    }
}

// Get saved shows
var showsResult = await _spotifyClient.User.MyShowGetAllAsync(limit: 20);
if (showsResult.IsSuccess)
{
    foreach (var show in showsResult.Data.Items)
    {
        Console.WriteLine($"Show: {show.Name}");
    }
}

// Get saved tracks
var tracksResult = await _spotifyClient.User.MyTrackGetAllAsync(limit: 50);
if (tracksResult.IsSuccess)
{
    foreach (var track in tracksResult.Data.Items)
    {
        Console.WriteLine($"Track: {track.Name} by {track.Artists[0].Name}");
    }
}

// Get top artists over the last 6 months
var topArtistsResult = await _spotifyClient.User.MyTopItemGetAllAsync(
    limit: 20,
    userTopType: UserTopType.Artists,
    userTopTimeRange: UserTopTimeRange.MediumTerm);
if (topArtistsResult.IsSuccess)
{
    foreach (var item in topArtistsResult.Data.Items ?? [])
    {
        if (item is Artist artist)
            Console.WriteLine($"Top Artist: {artist.Name}");
    }
}

// Get top tracks over the last 4 weeks
var topTracksResult = await _spotifyClient.User.MyTopItemGetAllAsync(
    limit: 10,
    userTopType: UserTopType.Tracks,
    userTopTimeRange: UserTopTimeRange.ShortTerm);
if (topTracksResult.IsSuccess)
{
    foreach (var item in topTracksResult.Data.Items ?? [])
    {
        if (item is Track track)
            Console.WriteLine($"Top Track: {track.Name}");
    }
}
```

## SpotifyOptions Configuration

The `SpotifyOptions` class contains the following properties:

| Property | Type | Description |
|----------|------|-------------|
| `ClientId` | `string?` | Your Spotify application client ID |
| `ClientSecret` | `string?` | Your Spotify application client secret |
| `RedirectUri` | `string?` | The redirect URI configured in your Spotify app |
| `Scopes` | `List<string>` | List of Spotify API scopes your application requires |
| `VerboseLogging` | `bool` | When `true`, each service method emits additional debug-level log entries for each request |

## Error Handling

All service methods return a `SpotifyResult<T>` which provides:
- `IsSuccess` — `true` when `Error` is `null`; `false` otherwise
- `Data` — the result data (populated on success)
- `Error` — a `SpotifyError` with `Status` (HTTP status code), `Message` (human-readable description), and `Reason` (machine-readable code or exception type name)

```csharp
var result = await _spotifyClient.Albums.AlbumGetAsync(albumId);
if (result.IsSuccess)
{
    var album = result.Data;
    // Process album data
}
else
{
    Console.WriteLine($"Status: {result.Error?.Status}");
    Console.WriteLine($"Message: {result.Error?.Message}");
    Console.WriteLine($"Reason: {result.Error?.Reason}");
}
```

## Requirements

- .NET 10
- C# 14.0
- Valid Spotify Premium account for playback control features
- Valid Spotify Developer credentials ([Get them here](https://developer.spotify.com/dashboard))

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.

## Contributing

Bring issues to the table, or suggest features! Contributions are welcome and appreciated.

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/EldinoCodes/EC.Spotify).

## Acknowledgments

Built for people who enjoy their spotify premium account and want to interact with its API features.  Great appreciation for the Spotify development team!
