# ITrackService

`ITrackService` is part of the `EC.Spotify.Abstractions` namespace and provides access to individual Spotify track data. It is exposed via the `ISpotifyClient.Tracks` property.

```csharp
ITrackService tracks = spotifyClient.Tracks;
```

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `TrackGetAsync` | `SpotifyResult<Track>` | Retrieves a single track by ID |
| `TrackGetRawAsync` | `string?` | Retrieves raw track JSON by ID |

---

### `TrackGetAsync`

```csharp
Task<SpotifyResult<Track>> TrackGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves the details of a Spotify track by its unique identifier asynchronously. Does not throw for missing tracks — check `IsSuccess` on the result.

- **Parameters:**
  - `id` — The Spotify track ID. Can be null or empty to indicate no track; in such cases, the result will not contain track data.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<Track>` with the track details if found; otherwise, the result indicates failure or not found.

**Usage example:**

```csharp
var result = await spotifyClient.Tracks.TrackGetAsync("1301WleyT98MSxVHPZCA6M", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"{result.Data?.Name} — {result.Data?.DurationMs}ms");
else
    Console.WriteLine($"Error: {result.Error}");
```

---

### `TrackGetRawAsync`

```csharp
Task<string?> TrackGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw track JSON from Spotify asynchronously by track identifier. Useful when you need the unprocessed API response.

- **Parameters:**
  - `id` — The Spotify track ID. Can be null or empty to indicate an invalid request.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Tracks.TrackGetRawAsync("1301WleyT98MSxVHPZCA6M", cancellationToken);
Console.WriteLine(json);
```
