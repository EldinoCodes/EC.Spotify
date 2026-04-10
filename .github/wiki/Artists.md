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
| `ArtistGetRawAsync` | `string?` | Retrieves raw artist JSON by ID |
| `ArtistAlbumGetAllRawAsync` | `string?` | Retrieves raw paginated album JSON for an artist |

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

### `ArtistGetRawAsync`

```csharp
Task<string?> ArtistGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw artist JSON from Spotify asynchronously by artist identifier.

- **Parameters:**
  - `id` — The Spotify artist ID.
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
  - `id` — The Spotify artist ID.
  - `albumTypes` — Filter by album type(s).
  - `limit` — Maximum number of albums to return. Default is `5`.
  - `offset` — Index of the first album to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Artists.ArtistAlbumGetAllRawAsync(
    "0TnOYISbd1XYRBk9myaseg",
    albumTypes: AlbumType.Compilation,
    cancellationToken: cancellationToken);
Console.WriteLine(json);
```
