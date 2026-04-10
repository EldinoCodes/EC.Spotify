# IEpisodeService

`IEpisodeService` is part of the `EC.Spotify.Abstractions` namespace and provides access to Spotify podcast episode data. It is exposed via the `ISpotifyClient.Episodes` property.

> **Required scope:** `user-read-playback-position`

```csharp
IEpisodeService episodes = spotifyClient.Episodes;
```

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `EpisodeGetAsync` | `SpotifyResult<Episode>` | Retrieves a single episode by ID |
| `EpisodeGetRawAsync` | `string?` | Retrieves raw episode JSON by ID |

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

### `EpisodeGetRawAsync`

```csharp
Task<string?> EpisodeGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw episode JSON from Spotify asynchronously by episode identifier. Requires the `user-read-playback-position` scope.

- **Parameters:**
  - `id` — The Spotify episode ID. Can be null or invalid.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Episodes.EpisodeGetRawAsync("512ojhOuo1ktJprKbVcKyQ", cancellationToken);
Console.WriteLine(json);
```
