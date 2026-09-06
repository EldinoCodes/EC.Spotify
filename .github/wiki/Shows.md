# IShowService

`IShowService` is part of the `EC.Spotify.Abstractions` namespace and provides access to Spotify podcast show and episode data. It is exposed via the `ISpotifyClient.Shows` property.

> **Required scope:** `user-read-playback-position`

```csharp
IShowService shows = spotifyClient.Shows;
```

---

## Methods

| Method | Scope Required | Returns | Description |
|--------|---------------|---------|-------------|
| `ShowGetAsync` | — | `SpotifyResult<Show>` | Retrieves a single show by ID |
| `ShowEpisodeGetAllAsync` | — | `SpotifyResult<SpotifyPageResult<Episode>>` | Retrieves a paginated list of episodes for a show |
| `MyShowGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Show>>` | Retrieves user's saved shows |

---

### `ShowGetAsync`

```csharp
Task<SpotifyResult<Show>> ShowGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves a Spotify show by its unique identifier asynchronously. Requires the `user-read-playback-position` scope. The result indicates failure if the show does not exist or the ID is invalid.

- **Parameters:**
  - `id` — The Spotify show ID. Can be null or empty to indicate an invalid request.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<Show>` with the show details if found; otherwise, the result indicates failure.

**Usage example:**

```csharp
var result = await spotifyClient.Shows.ShowGetAsync("38bS44xjbVVZ3No3ByF1dJ", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"{result.Data?.Name} — {result.Data?.Publisher}");
else
    Console.WriteLine($"Error: {result.Error}");
```

---

### `ShowEpisodeGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Show>>> MyShowGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of shows saved in the current user's Spotify library asynchronously. Requires the `user-library-read` scope.

- **Parameters:**
  - `limit` — Maximum number of shows to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first show to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<Show>>` containing the user's saved shows.

**Usage example:**

```csharp
var result = await spotifyClient.Shows.MyShowGetAllAsync(limit: 10, offset: 0, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var show in result.Data?.Items ?? [])
        Console.WriteLine($"{show.Name} — {show.Publisher}");
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `ShowGetRawAsync` | `ShowGetAsync` |
| `ShowEpisodeGetAllRawAsync` | `ShowEpisodeGetAllAsync` |
| `MyShowGetAllRawAsync` | `MyShowGetAllAsync` |

**Usage example:**

```csharp
// Get raw JSON response for a show
var json = await spotifyClient.Shows.ShowGetRawAsync(
    "38bS44xjbVVZ3No3ByF1dJ", 
    cancellationToken);

Console.WriteLine(json);
```

---

### `ShowGetRawAsync`

```csharp
Task<string?> ShowGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw show JSON from Spotify asynchronously by show identifier.

- **Parameters:**
  - `id` — The Spotify show ID. Can be null or empty to indicate an invalid request.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Shows.ShowGetRawAsync("38bS44xjbVVZ3No3ByF1dJ", cancellationToken);
Console.WriteLine(json);
```

---

### `ShowEpisodeGetAllRawAsync`

```csharp
Task<string?> ShowEpisodeGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated episode JSON for a show from Spotify asynchronously.

- **Parameters:**
  - `id` — The Spotify show ID.
  - `limit` — Maximum number of episodes to return. Default is `20`.
  - `offset` — Index of the first episode to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Shows.ShowEpisodeGetAllRawAsync("38bS44xjbVVZ3No3ByF1dJ", limit: 10, offset: 0, cancellationToken: cancellationToken);
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
var json = await spotifyClient.Shows.MyShowGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---
