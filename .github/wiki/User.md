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
| `MyEpisodeGetAllAsync` | `user-library-read`, `user-read-playback-position` | `SpotifyResult<SpotifyPageResult<Episode>>` | Retrieves the user's saved episodes |
| `MyPlaylistGetAllAsync` | `playlist-read-private` | `SpotifyResult<SpotifyPageResult<Playlist>>` | Retrieves playlists owned or followed by the user |
| `MyShowGetAllAsync` | `user-library-read`, `user-read-playback-position` | `SpotifyResult<SpotifyPageResult<Show>>` | Retrieves the user's saved shows |
| `MyTrackGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Track>>` | Retrieves the user's saved tracks |
| `MyTopItemGetAllAsync` | `user-top-read` | `SpotifyResult<SpotifyPageResult<IPolymorphicItem>>` | Retrieves the user's top artists or tracks |
| `MyAlbumGetAllRawAsync` | `user-library-read` | `string?` | Raw JSON for saved albums |
| `MyAudiobookGetAllRawAsync` | `user-library-read` | `string?` | Raw JSON for saved audiobooks |
| `MyEpisodeGetAllRawAsync` | `user-library-read`, `user-read-playback-position` | `string?` | Raw JSON for saved episodes |
| `MyPlaylistGetAllRawAsync` | `playlist-read-private` | `string?` | Raw JSON for user's playlists |
| `MyShowGetAllRawAsync` | `user-library-read`, `user-read-playback-position` | `string?` | Raw JSON for saved shows |
| `MyTrackGetAllRawAsync` | `user-library-read` | `string?` | Raw JSON for saved tracks |
| `MyTopItemGetAllRawAsync` | `user-top-read` | `string?` | Raw JSON for user's top items |

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

### Raw Methods

Each typed method has a corresponding raw counterpart returning `string?` with the unprocessed JSON response. They share the same parameter signatures.

| Raw Method | Typed Equivalent |
|------------|-----------------|
| `MyAlbumGetAllRawAsync` | `MyAlbumGetAllAsync` |
| `MyAudiobookGetAllRawAsync` | `MyAudiobookGetAllAsync` |
| `MyEpisodeGetAllRawAsync` | `MyEpisodeGetAllAsync` |
| `MyPlaylistGetAllRawAsync` | `MyPlaylistGetAllAsync` |
| `MyShowGetAllRawAsync` | `MyShowGetAllAsync` |
| `MyTrackGetAllRawAsync` | `MyTrackGetAllAsync` |
| `MyTopItemGetAllRawAsync` | `MyTopItemGetAllAsync` |

**Usage example:**

```csharp
var json = await spotifyClient.User.MyTrackGetAllRawAsync(limit: 20, cancellationToken: cancellationToken);
Console.WriteLine(json);
```
