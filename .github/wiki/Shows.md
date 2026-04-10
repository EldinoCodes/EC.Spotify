# IShowService

`IShowService` is part of the `EC.Spotify.Abstractions` namespace and provides access to Spotify podcast show and episode data. It is exposed via the `ISpotifyClient.Shows` property.

> **Required scope:** `user-read-playback-position`

```csharp
IShowService shows = spotifyClient.Shows;
```

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `ShowGetAsync` | `SpotifyResult<Show>` | Retrieves a single show by ID |
| `ShowEpisodeGetAllAsync` | `SpotifyResult<SpotifyPageResult<Episode>>` | Retrieves a paginated list of episodes for a show |
| `ShowGetRawAsync` | `string?` | Retrieves raw show JSON by ID |
| `ShowEpisodeGetAllRawAsync` | `string?` | Retrieves raw paginated episode JSON for a show |

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
Task<SpotifyResult<SpotifyPageResult<Episode>>> ShowEpisodeGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of episodes for the specified show asynchronously. Requires the `user-read-playback-position` scope. The result will contain an empty page if the show has no episodes — no exception is thrown.

- **Parameters:**
  - `id` — The Spotify show ID.
  - `limit` — Maximum number of episodes to return. Default is `20`.
  - `offset` — Index of the first episode to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<Episode>>` containing the page of episodes.

**Usage example:**

```csharp
var result = await spotifyClient.Shows.ShowEpisodeGetAllAsync("38bS44xjbVVZ3No3ByF1dJ", limit: 10, offset: 0, cancellationToken);

if (result.IsSuccess)
    foreach (var episode in result.Data?.Items ?? [])
        Console.WriteLine($"{episode.Name} — {episode.ReleaseDate}");
```

---

### `ShowGetRawAsync`

```csharp
Task<string?> ShowGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw show JSON from Spotify asynchronously by show identifier. Requires the `user-read-playback-position` scope.

- **Parameters:**
  - `id` — The Spotify show ID.
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

Retrieves raw paginated episode JSON for the specified show from Spotify asynchronously. Requires the `user-read-playback-position` scope.

- **Parameters:**
  - `id` — The Spotify show ID.
  - `limit` — Maximum number of episodes to return. Default is `20`.
  - `offset` — Index of the first episode to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Shows.ShowEpisodeGetAllRawAsync("38bS44xjbVVZ3No3ByF1dJ", limit: 5, cancellationToken: cancellationToken);
Console.WriteLine(json);
```
