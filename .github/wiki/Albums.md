# IAlbumService

`IAlbumService` is part of the `EC.Spotify.Abstractions` namespace and provides access to Spotify album data. It is exposed via the `ISpotifyClient.Albums` property.

```csharp
IAlbumService albums = spotifyClient.Albums;
```

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `AlbumGetAsync` | `SpotifyResult<Album>` | Retrieves a single album by ID |
| `AlbumTrackGetAllAsync` | `SpotifyResult<SpotifyPageResult<Track>>` | Retrieves a paginated list of tracks for an album |
| `MyAlbumGetAllAsync` | `SpotifyResult<SpotifyPageResult<Album>>` | Retrieves user's saved albums |

---

### `AlbumGetAsync`

```csharp
Task<SpotifyResult<Album>> AlbumGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves album details from Spotify asynchronously by album identifier. Does not throw for missing albums — check `IsSuccess` on the result.

- **Parameters:**
  - `id` — The Spotify album ID. If null or empty, the result will indicate failure.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<Album>` with album details if found; otherwise, the result indicates failure.

**Usage example:**

```csharp
var result = await spotifyClient.Albums.AlbumGetAsync("4aawyAB9vmqN3uQ7FjRGTy", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine(result.Data?.Name);
else
    Console.WriteLine($"Error: {result.Error}");
```

---

### `AlbumTrackGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Track>>> AlbumTrackGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of tracks for the specified album from Spotify asynchronously.

- **Parameters:**
  - `id` — The Spotify album ID.
  - `limit` — Maximum number of tracks to return. Default is `20`.
  - `offset` — Index of the first track to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<Track>>` containing the page of tracks.

**Usage example:**

```csharp
var result = await spotifyClient.Albums.AlbumTrackGetAllAsync("4aawyAB9vmqN3uQ7FjRGTy", limit: 10, offset: 0, cancellationToken);

if (result.IsSuccess)
    foreach (var track in result.Data?.Items ?? [])
        Console.WriteLine(track.Name);
```

---

### `MyAlbumGetAllAsync`

```csharp
Task<string?> MyAlbumGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated album JSON for albums saved in the current user's library from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of albums to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first album to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<Album>>` containing the page of albums.

**Usage example:**

```csharp
var result = await spotifyClient.Albums.MyAlbumGetAllAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var album in result.Data?.Items ?? [])
        Console.WriteLine(album.Name);
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `AlbumGetRawAsync` | `AlbumGetAsync` |
| `AlbumTrackGetAllRawAsync` | `AlbumTrackGetAllAsync` |
| `MyAlbumGetAllRawAsync` | `MyAlbumGetAllAsync` |

**Usage example:**

```csharp
// Get raw JSON response for an album
var json = await spotifyClient.Albums.AlbumGetRawAsync(
    "4aawyAB9vmqN3uQ7FjRGTy", 
    cancellationToken);

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
var json = await spotifyClient.Albums.MyAlbumGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `AlbumGetRawAsync` | `AlbumGetAsync` |
| `AlbumTrackGetAllRawAsync` | `AlbumTrackGetAllAsync` |
| `MyAlbumGetAllRawAsync` | `MyAlbumGetAllAsync` (Obsolete) |
| `NewReleasesGetRawAsync` | N/A |

**Usage example:**

```csharp
// Get raw JSON response for an album
var json = await spotifyClient.Albums.AlbumGetRawAsync(
    "4aawyAB9vmqN3uQ7FjRGTy", 
    cancellationToken);

Console.WriteLine(json);
```

---

### `AlbumGetRawAsync`

```csharp
Task<string?> AlbumGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw album JSON from Spotify asynchronously by album identifier.

- **Parameters:**
  - `id` — The Spotify album identifier. If null or empty, the request will not be sent.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Albums.AlbumGetRawAsync("4aawyAB9vmqN3uQ7FjRGTy", cancellationToken);
Console.WriteLine(json);
```

---

### `AlbumTrackGetAllRawAsync`

```csharp
Task<string?> AlbumTrackGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated track JSON for the specified album from Spotify asynchronously.

- **Parameters:**
  - `id` — The Spotify album identifier. If null, no tracks will be returned.
  - `limit` — Maximum number of tracks to return. Must be a positive integer. Default is `20`.
  - `offset` — Index of the first track to return. Used for pagination. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Albums.AlbumTrackGetAllRawAsync("4aawyAB9vmqN3uQ7FjRGTy", limit: 10, offset: 0, cancellationToken);
Console.WriteLine(json);
```

---

### `MyAlbumGetAllRawAsync`

```csharp
[Obsolete("This method is deprecated. Use IUserService.MyAlbumGetAllRawAsync instead.")]
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
var json = await spotifyClient.Albums.MyAlbumGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `NewReleasesGetRawAsync`

```csharp
Task<string?> NewReleasesGetRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw JSON for new album releases from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of items to return. Default is `20`.
  - `offset` — Index of the first item to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Albums.NewReleasesGetRawAsync(limit: 20, offset: 0, cancellationToken);
Console.WriteLine(json);
```

---
