# IPlaylistService

`IPlaylistService` is part of the `EC.Spotify.Abstractions` namespace and provides full playlist management — reading, updating, managing items and images. It is exposed via the `ISpotifyClient.Playlists` property.

```csharp
IPlaylistService playlists = spotifyClient.Playlists;
```

---

## Methods

| Method | Scope Required | Returns | Description |
|--------|---------------|---------|-------------|
| `PlaylistGetAsync` | — | `SpotifyResult<Playlist>` | Retrieves a playlist by ID |
| `PlaylistItemGetAllAsync` | `playlist-read-private` | `SpotifyResult<PlaylistPageResult>` | Retrieves a paginated list of items from a playlist |
| `PlaylistDetailUpdateAsync` | `playlist-modify-public`, `playlist-modify-private` | `SpotifyResult<bool>` | Updates a playlist's name, description, or visibility |
| `PlaylistItemAddAsync` | `playlist-modify-public`, `playlist-modify-private` | `SpotifyResult<PlaylistSnapshot>` | Adds a single item to a playlist |
| `PlaylistItemAddAllAsync` | `playlist-modify-public`, `playlist-modify-private` | `SpotifyResult<List<PlaylistSnapshot>>` | Adds multiple items to a playlist |
| `PlaylistItemRemoveAsync` | `playlist-modify-public`, `playlist-modify-private` | `SpotifyResult<PlaylistSnapshot>` | Removes a single item from a playlist |
| `PlaylistItemRemoveAllAsync` | `playlist-modify-public`, `playlist-modify-private` | `SpotifyResult<List<PlaylistSnapshot>>` | Removes multiple items from a playlist |
| `PlaylistImageAddAsync` | `ugc-image-upload`, `playlist-modify-public`, `playlist-modify-private` | `SpotifyResult<bool>` | Adds or replaces the playlist cover image |

---

### `PlaylistGetAsync`

```csharp
Task<SpotifyResult<Playlist>> PlaylistGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves the details of a Spotify playlist by its unique identifier asynchronously.

**Usage example:**

```csharp
var result = await spotifyClient.Playlists.PlaylistGetAsync("37i9dQZF1DXcBWIGoYBM5M", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"{result.Data?.Name} — {result.Data?.Description}");
```

---

### `PlaylistItemGetAllAsync`

```csharp
Task<SpotifyResult<PlaylistPageResult>> PlaylistItemGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paged list of items from the specified Spotify playlist asynchronously. Requires the `playlist-read-private` scope.

- **Parameters:**
  - `id` — The Spotify playlist ID.
  - `limit` — Maximum number of items to return. Default is `20`.
  - `offset` — Index of the first item to return. Default is `0`.

**Usage example:**

```csharp
var result = await spotifyClient.Playlists.PlaylistItemGetAllAsync("37i9dQZF1DXcBWIGoYBM5M", limit: 10, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var track in result.Data?.Items ?? [])
        Console.WriteLine(track.Track?.Name);
```

---

### `PlaylistDetailUpdateAsync`

```csharp
Task<SpotifyResult<bool>> PlaylistDetailUpdateAsync(string? id, PlaylistDetail? playlistDetail, CancellationToken cancellationToken = default);
```

Asynchronously updates the details of an existing playlist. Requires `playlist-modify-public` and `playlist-modify-private` scopes.

- **Parameters:**
  - `id` — The Spotify playlist ID.
  - `playlistDetail` — Object containing the new name, description, and/or public status to apply.

**Usage example:**

```csharp
var detail = new PlaylistDetail { Name = "Updated Name", Description = "New description", Public = false };
var result = await spotifyClient.Playlists.PlaylistDetailUpdateAsync("37i9dQZF1DXcBWIGoYBM5M", detail, cancellationToken);

Console.WriteLine(result.Data ? "Updated" : "Failed");
```

---

### `PlaylistItemAddAsync`

```csharp
Task<SpotifyResult<PlaylistSnapshot>> PlaylistItemAddAsync(string? id, ReferenceItem? libraryItem, int? position = null, CancellationToken cancellationToken = default);
```

Adds an item to a playlist asynchronously at the specified position. If `position` is null, the item is appended to the end. Returns a `PlaylistSnapshot` reflecting the state of the playlist after the item is added. Requires `playlist-modify-public` and `playlist-modify-private` scopes.

**Usage example:**

```csharp
var item = new ReferenceItem { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track };
var result = await spotifyClient.Playlists.PlaylistItemAddAsync("37i9dQZF1DXcBWIGoYBM5M", item, position: 0, cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"Added at position 0. Snapshot: {result.Data?.SnapshotId}");
```

---

### `PlaylistItemAddAllAsync`

```csharp
Task<SpotifyResult<List<PlaylistSnapshot>>> PlaylistItemAddAllAsync(string? id, List<ReferenceItem> libraryItems, int? position = null, CancellationToken cancellationToken = default);
```

Adds all specified items to a playlist asynchronously. If `position` is null, items are appended to the end. Returns a list of `PlaylistSnapshot` objects reflecting the state of the playlist after each batch is added. Requires `playlist-modify-public` and `playlist-modify-private` scopes.

**Usage example:**

```csharp
var items = new List<ReferenceItem>
{
    new() { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track },
    new() { Id = "4iV5W9uYEdYUVa79Axb7Rh", Type = ReferenceItemType.Track }
};

var result = await spotifyClient.Playlists.PlaylistItemAddAllAsync("37i9dQZF1DXcBWIGoYBM5M", items, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var snapshot in result.Data!)
        Console.WriteLine($"Snapshot: {snapshot.SnapshotId}");
```

---

### `PlaylistItemRemoveAsync`

```csharp
Task<SpotifyResult<PlaylistSnapshot>> PlaylistItemRemoveAsync(string? id, ReferenceItem? libraryItem, CancellationToken cancellationToken = default);
```

Removes an item from a playlist asynchronously. Returns a `PlaylistSnapshot` reflecting the updated state of the playlist after the item is removed. Requires `playlist-modify-public` and `playlist-modify-private` scopes.

**Usage example:**

```csharp
var item = new ReferenceItem { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track };
var result = await spotifyClient.Playlists.PlaylistItemRemoveAsync("37i9dQZF1DXcBWIGoYBM5M", item, cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"Removed. Snapshot: {result.Data?.SnapshotId}");
```

---

### `PlaylistItemRemoveAllAsync`

```csharp
Task<SpotifyResult<List<PlaylistSnapshot>>> PlaylistItemRemoveAllAsync(string? id, List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
```

Removes all specified items from the playlist asynchronously. Returns a list of `PlaylistSnapshot` objects reflecting the state of the playlist after each batch is removed. Requires `playlist-modify-public` and `playlist-modify-private` scopes.

**Usage example:**

```csharp
var items = new List<ReferenceItem>
{
    new() { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track },
    new() { Id = "4iV5W9uYEdYUVa79Axb7Rh", Type = ReferenceItemType.Track }
};

var result = await spotifyClient.Playlists.PlaylistItemRemoveAllAsync("37i9dQZF1DXcBWIGoYBM5M", items, cancellationToken);

if (result.IsSuccess)
    foreach (var snapshot in result.Data!)
        Console.WriteLine($"Snapshot: {snapshot.SnapshotId}");
```

---

### `PlaylistImageAddAsync`

```csharp
Task<SpotifyResult<bool>> PlaylistImageAddAsync(string? id, byte[]? imageData, CancellationToken cancellationToken = default);
```

Adds or replaces the image for the specified playlist asynchronously. The image must be a valid JPEG and meet Spotify's size requirements. Requires `ugc-image-upload`, `playlist-modify-public`, and `playlist-modify-private` scopes.

**Usage example:**

```csharp
var imageBytes = await File.ReadAllBytesAsync("cover.jpg", cancellationToken);
var result = await spotifyClient.Playlists.PlaylistImageAddAsync("37i9dQZF1DXcBWIGoYBM5M", imageBytes, cancellationToken);

Console.WriteLine(result.Data ? "Image updated" : "Failed");
```

---

### `PlaylistImageGetAllAsync`

```csharp
Task<SpotifyResult<List<Image>>> PlaylistImageGetAllAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves all images associated with the specified playlist asynchronously. The list will be empty if the playlist has no images.

**Usage example:**

```csharp
var result = await spotifyClient.Playlists.PlaylistImageGetAllAsync("37i9dQZF1DXcBWIGoYBM5M", cancellationToken);

if (result.IsSuccess)
    foreach (var image in result.Data ?? [])
        Console.WriteLine($"{image.Url} ({image.Width}x{image.Height})");
```

---

### `PlaylistGetRawAsync`

```csharp
Task<string?> PlaylistGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw playlist JSON from Spotify asynchronously by playlist identifier.

**Usage example:**

```csharp
var json = await spotifyClient.Playlists.PlaylistGetRawAsync("37i9dQZF1DXcBWIGoYBM5M", cancellationToken);
Console.WriteLine(json);
```

---

### `PlaylistItemGetAllRawAsync`

```csharp
Task<string?> PlaylistItemGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated playlist item JSON from Spotify asynchronously. Requires the `playlist-read-private` scope.

**Usage example:**

```csharp
var json = await spotifyClient.Playlists.PlaylistItemGetAllRawAsync("37i9dQZF1DXcBWIGoYBM5M", limit: 5, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `PlaylistImageGetAllRawAsync`

```csharp
Task<string?> PlaylistImageGetAllRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw playlist image JSON from Spotify asynchronously by playlist identifier.

- **Parameters:**
  - `id` — The Spotify playlist identifier.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Playlists.PlaylistImageGetAllRawAsync("37i9dQZF1DXcBWIGoYBM5M", cancellationToken);
Console.WriteLine(json);
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `PlaylistGetRawAsync` | `PlaylistGetAsync` |
| `PlaylistImageGetAllRawAsync` | `PlaylistImageGetAllAsync` |
| `PlaylistItemGetAllRawAsync` | `PlaylistItemGetAllAsync` |

**Usage example:**

```csharp
// Get raw JSON response for a playlist
var json = await spotifyClient.Playlists.PlaylistGetRawAsync(
    "37i9dQZF1DXcBWIGoYBM5M", 
    cancellationToken);

Console.WriteLine(json);
```

---

### `PlaylistGetRawAsync`

```csharp
Task<string?> PlaylistGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw playlist JSON from Spotify asynchronously by playlist identifier.

- **Parameters:**
  - `id` — The Spotify playlist identifier.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Playlists.PlaylistGetRawAsync("37i9dQZF1DXcBWIGoYBM5M", cancellationToken);
Console.WriteLine(json);
```

---

### `PlaylistItemGetAllRawAsync`

```csharp
Task<string?> PlaylistItemGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated playlist item JSON from Spotify asynchronously. Requires the `playlist-read-private` scope.

- **Parameters:**
  - `id` — The Spotify playlist identifier.
  - `limit` — Maximum number of items to return. Default is `20`.
  - `offset` — Index of the first item to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Playlists.PlaylistItemGetAllRawAsync("37i9dQZF1DXcBWIGoYBM5M", limit: 5, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `PlaylistImageGetAllRawAsync`

```csharp
Task<string?> PlaylistImageGetAllRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw playlist image JSON from Spotify asynchronously by playlist identifier.

- **Parameters:**
  - `id` — The Spotify playlist identifier.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Playlists.PlaylistImageGetAllRawAsync("37i9dQZF1DXcBWIGoYBM5M", cancellationToken);
Console.WriteLine(json);
```

---
