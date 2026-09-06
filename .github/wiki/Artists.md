# IArtistService

`IArtistService` is part of the `EC.Spotify.Abstractions` namespace and provides access to Spotify artist data. It is exposed via the `ISpotifyClient.Artists` property.

```csharp
IArtistService artists = spotifyClient.Artists;
```

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `ArtistGetAsync` | `SpotifyResult<Artist>` | Retrieves a single artist by ID |
| `ArtistAlbumGetAllAsync` | `SpotifyResult<SpotifyPageResult<Album>>` | Retrieves a paginated, optionally filtered list of albums for an artist |

---

### `ArtistGetAsync`

```csharp
Task<SpotifyResult<Artist>> ArtistGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves detailed information about a Spotify artist asynchronously by their unique identifier. Does not throw for missing artists — check `IsSuccess` on the result.

- **Parameters:**
  - `id` — The Spotify artist ID. Can be null or empty to indicate an invalid request.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<Artist>` with the artist's details if found; otherwise, the result indicates failure.

**Usage example:**

```csharp
var result = await spotifyClient.Artists.ArtistGetAsync("0TnOYISbd1XYRBk9myaseg", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine(result.Data?.Name);
else
    Console.WriteLine($"Error: {result.Error}");
```

---

### `ArtistAlbumGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Album>>> ArtistAlbumGetAllAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paged list of albums for the specified artist from the Spotify catalog asynchronously. Supports filtering by `AlbumType` — a `[Flags]` enum allowing combinations.

- **Parameters:**
  - `id` — The Spotify artist ID.
  - `albumTypes` — Filter by album type(s). If not specified, all types are included.
  - `limit` — Maximum number of albums to return. Default is `5`.
  - `offset` — Index of the first album to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<Album>>` with the page of albums.

#### `AlbumType` enum (`[Flags]`)

| Value | Description |
|-------|-------------|
| `Album` | Full-length studio albums |
| `Single` | Singles and EPs |
| `AppearsOn` | Albums the artist appears on |
| `Compilation` | Compilation albums |

**Usage example:**

```csharp
// Retrieve only albums and singles
var result = await spotifyClient.Artists.ArtistAlbumGetAllAsync(
    "0TnOYISbd1XYRBk9myaseg",
    albumTypes: AlbumType.Album | AlbumType.Single,
    limit: 10,
    cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var album in result.Data?.Items ?? [])
        Console.WriteLine($"{album.Name} ({album.AlbumType})");
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `ArtistGetRawAsync` | `ArtistGetAsync` |
| `ArtistAlbumGetAllRawAsync` | `ArtistAlbumGetAllAsync` |

**Usage example:**

```csharp
// Get raw JSON response for an artist
var json = await spotifyClient.Artists.ArtistGetRawAsync(
    "0TnOYISbd1XYRBk9myaseg", 
    cancellationToken);

Console.WriteLine(json);
```

---

### `ArtistsGetAllAsync`

```csharp
Task<SpotifyResult<List<Artist>>> ArtistsGetAllAsync(string ids, CancellationToken cancellationToken = default);
```

Retrieves details for multiple artists asynchronously by their comma-separated IDs.

- **Parameters:**
  - `ids` — Comma-separated list of Spotify artist IDs (max 50).
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<List<Artist>>` containing the list of artists.

**Usage example:**

```csharp
var result = await spotifyClient.Artists.ArtistsGetAllAsync("0TnOYISbd1XYRBk9myaseg,1Xyo4u8uXC1ZmMpatF05PJ", cancellationToken);

if (result.IsSuccess)
    foreach (var artist in result.Data ?? [])
        Console.WriteLine(artist.Name);
```

---

### `ArtistsGetAllRawAsync`

```csharp
Task<string?> ArtistsGetAllRawAsync(string ids, CancellationToken cancellationToken = default);
```

Retrieves raw JSON for multiple artists from Spotify asynchronously.

- **Parameters:**
  - `ids` — Comma-separated list of Spotify artist IDs (max 50).
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Artists.ArtistsGetAllRawAsync("0TnOYISbd1XYRBk9myaseg,1Xyo4u8uXC1ZmMpatF05PJ", cancellationToken);
Console.WriteLine(json);
```

---

### `ArtistTopTracksGetAsync`

```csharp
Task<SpotifyResult<List<Track>>> ArtistTopTracksGetAsync(string id, string? market = null, CancellationToken cancellationToken = default);
```

Retrieves the top tracks for a specified artist from Spotify asynchronously.

- **Parameters:**
  - `id` — The Spotify artist ID. Cannot be null or empty.
  - `market` — An ISO 3166-1 alpha-2 country code to request market filtering. Optional.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<List<Track>>` containing the top tracks.

**Usage example:**

```csharp
var result = await spotifyClient.Artists.ArtistTopTracksGetAsync("0TnOYISbd1XYRBk9myaseg", market: "US", cancellationToken);

if (result.IsSuccess)
    foreach (var track in result.Data ?? [])
        Console.WriteLine(track.Name);
```

---

### `ArtistRelatedArtistsGetAsync`

```csharp
Task<SpotifyResult<List<Artist>>> ArtistRelatedArtistsGetAsync(string id, CancellationToken cancellationToken = default);
```

Retrieves related artists for a specified artist from Spotify asynchronously.

- **Parameters:**
  - `id` — The Spotify artist ID. Cannot be null or empty.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<List<Artist>>` containing the list of related artists.

**Usage example:**

```csharp
var result = await spotifyClient.Artists.ArtistRelatedArtistsGetAsync("0TnOYISbd1XYRBk9myaseg", cancellationToken);

if (result.IsSuccess)
    foreach (var artist in result.Data ?? [])
        Console.WriteLine(artist.Name);
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `ArtistGetRawAsync` | `ArtistGetAsync` |
| `ArtistAlbumGetAllRawAsync` | `ArtistAlbumGetAllAsync` |
| `ArtistsGetAllRawAsync` | `ArtistsGetAllAsync` |
| `MyArtistGetAllRawAsync` | `MyArtistGetAllAsync` (Obsolete) |

**Usage example:**

```csharp
// Get raw JSON response for an artist
var json = await spotifyClient.Artists.ArtistGetRawAsync(
    "0TnOYISbd1XYRBk9myaseg", 
    cancellationToken);

Console.WriteLine(json);
```

---

### `ArtistGetRawAsync`

```csharp
Task<string?> ArtistGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw artist JSON from Spotify asynchronously by artist identifier.

- **Parameters:**
  - `id` — The Spotify artist ID to retrieve. Can be null or empty to indicate an invalid request.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Artists.ArtistGetRawAsync("0TnOYISbd1XYRBk9myaseg", cancellationToken);
Console.WriteLine(json);
```

---

### `ArtistAlbumGetAllRawAsync`

```csharp
Task<string?> ArtistAlbumGetAllRawAsync(string? id, AlbumType? albumTypes = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated album JSON for the specified artist from Spotify asynchronously.

- **Parameters:**
  - `id` — The Spotify ID of the artist whose albums are to be retrieved. Can be null to indicate no artist.
  - `albumTypes` — A filter specifying which types of albums to include in the results.
  - `limit` — Maximum number of albums to return. Default is `5`.
  - `offset` — Index of the first album to return. Used for paging. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Artists.ArtistAlbumGetAllRawAsync("0TnOYISbd1XYRBk9myaseg", limit: 10, offset: 0, cancellationToken);
Console.WriteLine(json);
```

---

### `ArtistsGetAllRawAsync`

```csharp
Task<string?> ArtistsGetAllRawAsync(string ids, CancellationToken cancellationToken = default);
```

Retrieves raw JSON for multiple artists from Spotify asynchronously.

- **Parameters:**
  - `ids` — Comma-separated list of Spotify artist IDs (max 50).
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Artists.ArtistsGetAllRawAsync("0TnOYISbd1XYRBk9myaseg,1Xyo4u8uXC1ZmMpatF05PJ", cancellationToken);
Console.WriteLine(json);
```

---

### `MyArtistGetAllRawAsync`

```csharp
[Obsolete("This method is deprecated. Use IUserService.MyArtistGetAllRawAsync instead.")]
Task<string?> MyArtistGetAllRawAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated artist JSON for artists saved in the current user's library from Spotify asynchronously.

- **Parameters:**
  - `limit` — Maximum number of artists to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first artist to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Artists.MyArtistGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---
