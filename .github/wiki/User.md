# IUserService

`IUserService` is part of the `EC.Spotify.Abstractions` namespace and provides access to the current authenticated user's saved library content and personalization data. It is exposed via the `ISpotifyClient.User` property.

```csharp
IUserService user = spotifyClient.User;
```

---

## Methods

| Method | Scope Required | Returns | Description |
|--------|---------------|---------|-------------|
| `MyAlbumGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Album>>` | Retrieves the user's saved albums |
| `MyAudiobookGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Audiobook>>` | Retrieves the user's saved audiobooks |
| `MyEpisodeGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Episode>>` | Retrieves the user's saved episodes |
| `MyPlaylistGetAllAsync` | `playlist-read-private` | `SpotifyResult<SpotifyPageResult<Playlist>>` | Retrieves playlists owned or followed by the user |
| `MyShowGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Show>>` | Retrieves the user's saved shows |
| `MyTrackGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Track>>` | Retrieves the user's saved tracks |
| `MyTopItemGetAllAsync` | `user-top-read` | `SpotifyResult<SpotifyPageResult<IPolymorphicItem>>` | Retrieves the user's top artists or tracks |
| `CurrentProfileGetAsync` | `user-read-private`, `user-read-email` | `SpotifyResult<User>` | Retrieves the current user's profile |
| `GetFollowingAsync` | `user-follow-read` | `SpotifyResult<SpotifyPageResult<Artist>>` | Retrieves the user's followed artists |

> **Note:** `limit` must be between 1 and 50 for all paginated methods.

---

### `MyAlbumGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Album>>> MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of albums saved in the current user's Spotify library asynchronously. Requires the `user-library-read` scope.

**Usage example:**

```csharp
var result = await spotifyClient.User.MyAlbumGetAllAsync(limit: 20, offset: 0, cancellationToken);

if (result.IsSuccess)
    foreach (var album in result.Data?.Items ?? [])
        Console.WriteLine(album.Name);
```

---

### `MyAudiobookGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Audiobook>>> MyAudiobookGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of audiobooks saved in the current user's Spotify library asynchronously. Requires the `user-library-read` scope.

**Usage example:**

```csharp
var result = await spotifyClient.User.MyAudiobookGetAllAsync(limit: 10, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var audiobook in result.Data?.Items ?? [])
        Console.WriteLine(audiobook.Name);
```

---

### `MyEpisodeGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Episode>>> MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of episodes saved in the current user's Spotify library asynchronously. Requires `user-library-read` and `user-read-playback-position` scopes.

**Usage example:**

```csharp
var result = await spotifyClient.User.MyEpisodeGetAllAsync(limit: 10, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var episode in result.Data?.Items ?? [])
        Console.WriteLine($"{episode.Name} — {episode.ReleaseDate}");
```

---

### `MyPlaylistGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Playlist>>> MyPlaylistGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves all playlists owned or followed by the current user asynchronously. Requires the `playlist-read-private` scope. The result may be empty if the user has no playlists.

**Usage example:**

```csharp
var result = await spotifyClient.User.MyPlaylistGetAllAsync(limit: 50, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var playlist in result.Data?.Items ?? [])
        Console.WriteLine($"{playlist.Name} ({playlist.Tracks?.Total} tracks)");
```

---

### `MyShowGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Show>>> MyShowGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of shows saved in the current user's Spotify library asynchronously. Requires `user-library-read` and `user-read-playback-position` scopes.

**Usage example:**

```csharp
var result = await spotifyClient.User.MyShowGetAllAsync(limit: 20, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var show in result.Data?.Items ?? [])
        Console.WriteLine($"{show.Name} — {show.Publisher}");
```

---

### `MyTrackGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Track>>> MyTrackGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of tracks saved in the current user's Spotify library asynchronously. Requires the `user-library-read` scope.

**Usage example:**

```csharp
var result = await spotifyClient.User.MyTrackGetAllAsync(limit: 50, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var track in result.Data?.Items ?? [])
        Console.WriteLine($"{track.Name} — {track.Artists?.FirstOrDefault()?.Name}");
```

---

### `MyTopItemGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<IPolymorphicItem>>> MyTopItemGetAllAsync(UserTopType userTopType = UserTopType.Tracks, UserTopTimeRange userTopTimeRange = UserTopTimeRange.MediumTerm, int? limit = 20, int offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of the current user's top artists or tracks asynchronously. Requires the `user-top-read` scope.

- **Parameters:**
  - `userTopType` — The type of top items to retrieve.
  - `userTopTimeRange` — The time range over which affinity is computed.
  - `limit` — Maximum number of items to return. Default is `20`.
  - `offset` — Index of the first item to return. Default is `0`.

#### `UserTopType` enum

| Value | Description |
|-------|-------------|
| `Artists` | Retrieve the user's top artists |
| `Tracks` | Retrieve the user's top tracks |

#### `UserTopTimeRange` enum

| Value | Description |
|-------|-------------|
| `LongTerm` | Computed over several years including all historical data |
| `MediumTerm` | Approximately the last 6 months |
| `ShortTerm` | Approximately the last 4 weeks |

**Usage example:**

```csharp
// Top 10 artists over the last 6 months
var result = await spotifyClient.User.MyTopItemGetAllAsync(
    UserTopType.Artists,
    UserTopTimeRange.MediumTerm,
    limit: 10,
    cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var item in result.Data?.Items ?? [])
        Console.WriteLine(item.Name);
```

---

### `GetFollowingAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Artist>>> GetFollowingAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a list of the current user's followed artists asynchronously. Requires the `user-follow-read` scope. This method returns the artists that the current user is following on Spotify.

**Parameters:**

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `limit` | `int?` | `20` | The maximum number of artists to return. Must be between 1 and 50. |
| `offset` | `int?` | `0` | The zero-based index of the first artist to return. Used for pagination. |
| `cancellationToken` | `CancellationToken` | `default` | A cancellation token that can be used to cancel the operation. |

**Returns:** A task that represents the asynchronous operation. The task result contains a `SpotifyResult<SpotifyPageResult<Artist>>` with the user's followed artists.

**Usage example:**

```csharp
var result = await spotifyClient.User.GetFollowingAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var artist in result.Data?.Items ?? [])
        Console.WriteLine($"{artist.Name} ({artist.Followers?.Total} followers)");
```

---

## Raw Methods

Each typed method has a corresponding raw counterpart that returns the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures as their typed equivalents but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `MyAlbumGetAllRawAsync` | `MyAlbumGetAllAsync` |
| `MyAudiobookGetAllRawAsync` | `MyAudiobookGetAllAsync` |
| `MyEpisodeGetAllRawAsync` | `MyEpisodeGetAllAsync` |
| `MyPlaylistGetAllRawAsync` | `MyPlaylistGetAllAsync` |
| `MyShowGetAllRawAsync` | `MyShowGetAllAsync` |
| `MyTrackGetAllRawAsync` | `MyTrackGetAllAsync` |
| `MyTopItemGetAllRawAsync` | `MyTopItemGetAllAsync` |
| `GetFollowingRawAsync` | `GetFollowingAsync` |

**Usage example:**

```csharp
// Get raw JSON response for user's top tracks
var json = await spotifyClient.User.MyTrackGetAllRawAsync(
    limit: 20, 
    offset: 0, 
    cancellationToken: cancellationToken);

Console.WriteLine(json);
```

---

### `MyAlbumGetAllRawAsync`

```csharp
Task<string?> MyAlbumGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated album JSON for albums saved in the current user's library from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of albums to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first album to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.MyAlbumGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `MyAudiobookGetAllRawAsync`

```csharp
Task<string?> MyAudiobookGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated audiobook JSON for audiobooks saved in the current user's library from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of audiobooks to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first audiobook to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.MyAudiobookGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `MyEpisodeGetAllRawAsync`

```csharp
Task<string?> MyEpisodeGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated episode JSON for episodes saved in the current user's library from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of episodes to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first episode to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.MyEpisodeGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `MyPlaylistGetAllRawAsync`

```csharp
Task<string?> MyPlaylistGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated playlist JSON for playlists owned by the current user from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of playlists to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first playlist to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.MyPlaylistGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `MyShowGetAllRawAsync`

```csharp
Task<string?> MyShowGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated show JSON for shows saved in the current user's library from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of shows to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first show to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.MyShowGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `MyTrackGetAllRawAsync`

```csharp
Task<string?> MyTrackGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated track JSON for tracks saved in the current user's library from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of tracks to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first track to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.MyTrackGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `MyTopItemGetAllRawAsync`

```csharp
Task<string?> MyTopItemGetAllRawAsync(UserTopType? type = default, string? timeRange = default, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated top item JSON for the current user from Spotify asynchronously. Top items are computed from a selected period of time (all time, recent, or custom range).

- **Parameters:**
  - `type` — The type of item to retrieve top items for. Default is `UserTopType.Track`.
  - `timeRange` — The time range over which to compute top items. Options: `long_term`, `medium_term`, `short_term`. Default is `null`.
  - `limit` — Maximum number of items to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first item to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.MyTopItemGetAllRawAsync(UserTopType.Track, "medium_term", limit: 10, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `GetFollowingRawAsync`

```csharp
Task<string?> GetFollowingRawAsync(string? type = default, string? after = default, int? limit = 20, CancellationToken cancellationToken = default);
```

Retrieves raw paginated followed artist JSON for the current user from Spotify asynchronously. Requires the `user-follow-read` scope.

- **Parameters:**
  - `type` — The type of item to retrieve followed items for. Must be `artist`. Default is `null`.
  - `after` — The cursor position in the list of followed items. Used for pagination.
  - `limit` — Maximum number of items to return. Must be between 1 and 50. Default is `20`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.User.GetFollowingRawAsync("artist", limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---
