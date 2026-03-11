# EC.Spotify

A comprehensive .NET client library for the Spotify Web API, providing a clean and intuitive interface for interacting with Spotify's music streaming platform.

## Overview

`EC.Spotify` is a modern .NET 10 library that wraps the Spotify Web API, offering strongly-typed access to albums, artists, tracks, playlists, playback control, and more. The library follows best practices with dependency injection support, async/await patterns, and comprehensive error handling.

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
        "user-read-playback-state",
        "user-modify-playback-state",
        "user-read-currently-playing",
        "playlist-read-private"
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
      "user-read-playback-state",
      "user-modify-playback-state",
      "user-read-currently-playing",
      "playlist-read-private"
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

### IAlbumService

Provides methods for retrieving album data from Spotify.

**Available Methods:**
- **`AlbumGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed album information by album ID, including track list, artists, release date, and more.

- **`AlbumTrackGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated tracks for a specific album with support for limit and offset parameters.

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
  Retrieves detailed artist information including name, genres, popularity, followers, and images.

- **`ArtistAlbumGetAllAsync(string? id, int? limit = 20, int? offset = 0, string? includeGroups = default, CancellationToken cancellationToken = default)`**  
  Retrieves paginated albums for an artist with filtering support via `includeGroups` (e.g., "album", "single", "appears_on", "compilation").

**Example:**
```csharp
// Get artist details
var artistResult = await _spotifyClient.Artists.ArtistGetAsync("0TnOYISbd1XYRBk9myaseg");
if (artistResult.IsSuccess)
{
    var artist = artistResult.Data;
    Console.WriteLine($"Artist: {artist.Name}");
    Console.WriteLine($"Followers: {artist.Followers?.Total:N0}");
    Console.WriteLine($"Popularity: {artist.Popularity}");
    Console.WriteLine($"Genres: {string.Join(", ", artist.Genres)}");
}

// Get artist albums (only albums and singles)
var albumsResult = await _spotifyClient.Artists.ArtistAlbumGetAllAsync(
    "0TnOYISbd1XYRBk9myaseg", 
    limit: 20, 
    offset: 0,
    includeGroups: "album,single"
);
```

### IAudiobookService

Provides methods for retrieving audiobook data and chapters.

**Available Methods:**
- **`AudiobookGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed audiobook information including title, author, narrator, description, and total chapters.

- **`AudiobookChapterGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated chapters for a specific audiobook.

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

### IAuthorizationService

Manages Spotify OAuth 2.0 authorization flow and token management. This service is critical for authenticating users and maintaining access tokens.

**Available Methods:**
- **`Validate(CancellationToken cancellationToken = default)`**  
  Validates the current authentication state and returns an authorization URL if user authorization is required.

- **`AuthorizationCodeUrl()`**  
  Generates the OAuth 2.0 authorization URL for initiating the authorization code flow.

- **`AuthorizationCodeAddAsync(string? authorizationCode, CancellationToken cancellationToken = default)`**  
  Stores an authorization code received from the OAuth callback.

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

// Handle OAuth callback
[HttpGet("callback")]
public async Task<IActionResult> SpotifyCallback(string code)
{
    var success = await _spotifyClient.Authorization.AuthorizationCodeAddAsync(code);
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
    Console.WriteLine($"Expires: {token.ExpiresAt}");
}

// Remove authorization
await _spotifyClient.Authorization.AuthorizationCodeRemoveAsync();
await _spotifyClient.Authorization.AuthorizationTokenReset();
```

### IChapterService

Provides methods for retrieving individual audiobook chapter data.

**Available Methods:**
- **`ChapterGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed chapter information including name, description, duration, and chapter number.

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
  Retrieves detailed episode information including name, description, duration, release date, and show information.

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

### IPlayerService

Controls Spotify playback and manages player state. This is the most feature-rich service interface, providing comprehensive control over Spotify's playback functionality.

**Available Methods:**
- **`QueueGetAsync(CancellationToken cancellationToken = default)`** - Retrieves the current playback queue
- **`QueueAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default)`** - Adds a track to the playback queue
- **`DeviceGetAllAsync(CancellationToken cancellationToken = default)`** - Retrieves all available playback devices
- **`TransferAsync(string? deviceId, bool play = false, CancellationToken cancellationToken = default)`** - Transfers playback to a specific device
- **`PlayerPlayAsync(string? deviceId, List<string>? trackUris, CancellationToken cancellationToken = default)`** - Starts playback
- **`PlayerPauseAsync(string? deviceId = null, CancellationToken cancellationToken = default)`** - Pauses playback
- **`PlayerNextAsync(string? deviceId = null, CancellationToken cancellationToken = default)`** - Skips to the next track
- **`PlayerPreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default)`** - Skips to the previous track
- **`PlayerSeekAsync(int positionMs, string? deviceId = null, CancellationToken cancellationToken = default)`** - Seeks to a position in the track
- **`PlayerRepeatAsync(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)`** - Sets repeat mode
- **`PlayerShuffleAsync(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off, string? deviceId = null, CancellationToken cancellationToken = default)`** - Enables or disables shuffle
- **`PlayerVolumeAsync(int volumePercent, string? deviceId = null, CancellationToken cancellationToken = default)`** - Sets the playback volume

**Example:**
```csharp
// Get available devices
var devicesResult = await _spotifyClient.Player.DeviceGetAllAsync();

// Start playback
var trackUris = new List<string> { "spotify:track:6rqhFgbbKwnb9MLmUQDhG6" };
await _spotifyClient.Player.PlayerPlayAsync(deviceId: null, trackUris);

// Control playback
await _spotifyClient.Player.PlayerPauseAsync();
await _spotifyClient.Player.PlayerNextAsync();
await _spotifyClient.Player.PlayerVolumeAsync(volumePercent: 75);

// Set playback modes
await _spotifyClient.Player.PlayerRepeatAsync(PlayerRepeatMode.Track);
await _spotifyClient.Player.PlayerShuffleAsync(PlayerShuffleMode.On);
```

### ISearchService

Performs search queries across Spotify's catalog, supporting multiple content types.

<p style="background-color: #856404; border-radius: .5rem; padding:.5rem; font-size:2rem;">
<span style="font-size:2rem; font-weight:bold;">Note:</span>&nbsp;Spotify JSON is polymorphic, to handle this without impacting consumer serialization, I fudge a '$type' property on the Spotify data where received to make System.Text.Serialization work.
</p>

**Available Methods:**
- **`SearchAsync(SearchQuery? searchQuery, CancellationToken cancellationToken = default)`**  
  Performs a search using specified criteria including query string, search types, limit, and offset.

**Example:**
```csharp
var searchQuery = new SearchQuery
{
    Query = "Bohemian Rhapsody",
    Types = new[] { "track", "artist" },
    Limit = 10
};

var searchResult = await _spotifyClient.Search.SearchAsync(searchQuery);
if (searchResult.IsSuccess)
{
    var tracks = searchResult.Data.Items?.Where(i => i.Type.Equals("track"))?.ToList() ?? [];
    var artists = searchResult.Data.Items?.Where(i => i.Type.Equals("artist"))?.ToList() ?? [];;
}
```

### IShowService

Provides methods for retrieving podcast show data and episodes.

**Available Methods:**
- **`ShowGetAsync(string? id, CancellationToken cancellationToken = default)`**  
  Retrieves detailed show information by show ID.

- **`ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default)`**  
  Retrieves paginated episodes for a specific show.

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
  Retrieves detailed track information including name, artists, album, duration, and popularity.

**Example:**
```csharp
var trackResult = await _spotifyClient.Tracks.TrackGetAsync("3n3Ppam7vgaVa1iaRUc9Lp");
if (trackResult.IsSuccess)
{
    var track = trackResult.Data;
    Console.WriteLine($"Track: {track.Name} by {track.Artists[0].Name}");
    Console.WriteLine($"Duration: {track.DurationMilliseconds}ms, Popularity: {track.Popularity}");
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

## Error Handling

All service methods return a `SpotifyResult<T>` which provides:
- `IsSuccess` - Indicates if the operation was successful
- `Data` - The result data (if successful)
- Error information (if unsuccessful)

```csharp
var result = await _spotifyClient.Albums.AlbumGetAsync(albumId);
if (result.IsSuccess)
{
    var album = result.Data;
    // Process album data
}
else
{
    // Handle error
    Console.WriteLine($"Error: {result.ErrorMessage}");
}
```

## Requirements

- .NET 10
- C# 14.0
- Valid Spotify Premium account for playback control features
- Valid Spotify Developer credentials ([Get them here](https://developer.spotify.com/dashboard))

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contributing

[Insert Contributing Guidelines]

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/EldinoCodes/EC.Spotify).

## Acknowledgments

Built with ?? for the Spotify developer community.
