# ISearchService

`ISearchService` is part of the `EC.Spotify.Abstractions` namespace and provides full-catalog search across Spotify content types. It is exposed via the `ISpotifyClient.Search` property.

```csharp
ISearchService search = spotifyClient.Search;
```

Results are polymorphic — a single call can return a mix of albums, artists, tracks, playlists, shows, episodes, and audiobooks depending on the `SearchType` flags supplied.

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `SearchAsync` | `SpotifyResult<SpotifyPageResult<IPolymorphicItem>>` | Searches the Spotify catalog and returns typed results |

---

## `SearchType` enum (`[Flags]`)

`SearchType` is a flags enum, so types can be combined with the bitwise OR operator.

| Value | Description |
|-------|-------------|
| `Album` | Search for albums |
| `Artist` | Search for artists |
| `Track` | Search for tracks |
| `Playlist` | Search for playlists |
| `Show` | Search for podcasts/shows |
| `Episode` | Search for podcast episodes |
| `Audiobook` | Search for audiobooks |

---

### `SearchAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<IPolymorphicItem>>> SearchAsync(string? query, SearchType? searchType = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default);
```

Performs an asynchronous search against the Spotify catalog using the specified query and search type. Returns a polymorphic page of results matching the query and search type.

- **Parameters:**
  - `query` — The search query string. Can be null or empty to return no results.
  - `searchType` — The type(s) of item to search for. If null, a default search type may be used.
  - `limit` — Maximum number of items to return. Default is `5`.
  - `offset` — Index of the first item to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<IPolymorphicItem>>` with paged results.

**Usage example — single type:**

```csharp
var result = await spotifyClient.Search.SearchAsync("Radiohead", SearchType.Artist, limit: 5, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var item in result.Data?.Items ?? [])
        Console.WriteLine(item.Name);
```

**Usage example — multiple types:**

```csharp
// Search for both tracks and albums
var result = await spotifyClient.Search.SearchAsync(
    "OK Computer",
    SearchType.Track | SearchType.Album,
    limit: 10,
    cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var item in result.Data?.Items ?? [])
        Console.WriteLine($"[{item.GetType().Name}] {item.Name}");
```

---

## Raw Methods

The `SearchAsync` method has a corresponding raw counterpart that returns the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `SearchRawAsync` | `SearchAsync` |

**Usage example:**

```csharp
// Get raw JSON response for a search
var json = await spotifyClient.Search.SearchRawAsync(
    "Daft Punk",
    SearchType.Track | SearchType.Album,
    limit: 3,
    cancellationToken: cancellationToken);

Console.WriteLine(json);
```

---

### `SearchRawAsync`

```csharp
Task<string?> SearchRawAsync(string? query, SearchType? searchType = default, int? limit = 5, int? offset = 0, CancellationToken cancellationToken = default);
```

Performs an asynchronous search against the Spotify catalog and returns the raw JSON response. Results are polymorphic — a single call can return a mix of albums, artists, tracks, playlists, shows, episodes, and audiobooks depending on the `SearchType` flags supplied.

- **Parameters:**
  - `query` — The search query string. Can be null or empty to return no results.
  - `searchType` — The type(s) of item to search for. If null, a default search type may be used.
  - `limit` — Maximum number of items to return. Default is `5`.
  - `offset` — Index of the first item to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example — single type:**

```csharp
var json = await spotifyClient.Search.SearchRawAsync("Radiohead", SearchType.Artist, limit: 5, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

**Usage example — multiple types:**

```csharp
// Search for both tracks and albums
var json = await spotifyClient.Search.SearchRawAsync(
    "OK Computer",
    SearchType.Track | SearchType.Album,
    limit: 10,
    cancellationToken: cancellationToken);

Console.WriteLine(json);
```

---
Console.WriteLine(json);
```
