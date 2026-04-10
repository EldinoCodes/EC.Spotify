# IChapterService

`IChapterService` is part of the `EC.Spotify.Abstractions` namespace and provides access to individual Spotify audiobook chapter data. It is exposed via the `ISpotifyClient.Chapters` property.

```csharp
IChapterService chapters = spotifyClient.Chapters;
```

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `ChapterGetAsync` | `SpotifyResult<Chapter>` | Retrieves a single audiobook chapter by ID |
| `ChapterGetRawAsync` | `string?` | Retrieves raw chapter JSON by ID |

---

### `ChapterGetAsync`

```csharp
Task<SpotifyResult<Chapter>> ChapterGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves the details of a Spotify audiobook chapter by its unique identifier asynchronously. Does not block the calling thread. If the chapter ID does not exist or is invalid, the result indicates an error or not found.

- **Parameters:**
  - `id` — The Spotify chapter ID. Can be null or empty to indicate an invalid request.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<Chapter>` with the chapter details if found; otherwise, the result indicates an error or not found.

**Usage example:**

```csharp
var result = await spotifyClient.Chapters.ChapterGetAsync("0D5wENdkdwbqlrHoaJ9g29", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"Chapter {result.Data?.ChapterNumber}: {result.Data?.Name}");
else
    Console.WriteLine($"Error: {result.Error}");
```

---

### `ChapterGetRawAsync`

```csharp
Task<string?> ChapterGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw chapter JSON from Spotify asynchronously by chapter identifier. Useful when you need the unprocessed API response.

- **Parameters:**
  - `id` — The Spotify chapter ID. Can be null or empty to indicate an invalid request.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Chapters.ChapterGetRawAsync("0D5wENdkdwbqlrHoaJ9g29", cancellationToken);
Console.WriteLine(json);
```
