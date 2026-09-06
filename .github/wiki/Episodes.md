# IEpisodeService

`IEpisodeService` is part of the `EC.Spotify.Abstractions` namespace and provides access to Spotify podcast episode data. It is exposed via the `ISpotifyClient.Episodes` property.

> **Required scope:** `user-read-playback-position`

```csharp
IEpisodeService episodes = spotifyClient.Episodes;
```

---

## Methods

| Method | Scope Required | Returns | Description |
|--------|---------------|---------|-------------|
| `EpisodeGetAsync` | — | `SpotifyResult<Episode>` | Retrieves a single episode by ID |
| `MyEpisodeGetAllAsync` | `user-library-read` | `SpotifyResult<SpotifyPageResult<Episode>>` | Retrieves user's saved episodes |

---

### `EpisodeGetAsync`

```csharp
Task<SpotifyResult<Episode>> EpisodeGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves the details of a Spotify episode by its unique identifier asynchronously. Requires the `user-read-playback-position` scope. The operation may fail if the episode does not exist or if the ID is invalid.

- **Parameters:**
  - `id` — The Spotify episode ID. Can be null or invalid; in such cases, the result will indicate an error.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<Episode>` with the episode details if found; otherwise, an error result.

**Usage example:**

```csharp
var result = await spotifyClient.Episodes.EpisodeGetAsync("512ojhOuo1ktJprKbVcKyQ", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"{result.Data?.Name} — {result.Data?.DurationMs}ms");
else
    Console.WriteLine($"Error: {result.Error}");
```

---

### `MyEpisodeGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Episode>>> MyEpisodeGetAllAsync(int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of episodes saved in the current user's library asynchronously. Requires the `user-library-read` scope.

- **Parameters:**
  - `limit` — Maximum number of episodes to return. Must be between 1 and 50. Default is `20`.
  - `offset` — Zero-based index of the first episode to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<Episode>>` containing the user's saved episodes.

**Usage example:**

```csharp
var result = await spotifyClient.Episodes.MyEpisodeGetAllAsync(limit: 10, offset: 0, cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var episode in result.Data?.Items ?? [])
        Console.WriteLine($"{episode.Name} — {episode.ReleaseDate}");
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `EpisodeGetRawAsync` | `EpisodeGetAsync` |
| `MyEpisodeGetAllRawAsync` | `MyEpisodeGetAllAsync` |

**Usage example:**

```csharp
// Get raw JSON response for an episode
var json = await spotifyClient.Episodes.EpisodeGetRawAsync(
    "512ojhOuo1ktJprKbVcKyQ", 
    cancellationToken);

Console.WriteLine(json);
```

---

### `EpisodeGetRawAsync`

```csharp
Task<string?> EpisodeGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw episode JSON from Spotify asynchronously by episode identifier.

- **Parameters:**
  - `id` — The Spotify episode ID. Can be null or invalid; in such cases, the result will indicate an error.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Episodes.EpisodeGetRawAsync("512ojhOuo1ktJprKbVcKyQ", cancellationToken);
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
var json = await spotifyClient.Episodes.MyEpisodeGetAllRawAsync(limit: 20, offset: 0, cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---
