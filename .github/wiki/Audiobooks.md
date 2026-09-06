# IAudiobookService

`IAudiobookService` is part of the `EC.Spotify.Abstractions` namespace and provides access to Spotify audiobook data. It is exposed via the `ISpotifyClient.Audiobooks` property.

```csharp
IAudiobookService audiobooks = spotifyClient.Audiobooks;
```

---

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `AudiobookGetAsync` | `SpotifyResult<Audiobook>` | Retrieves a single audiobook by ID |
| `AudiobookChapterGetAllAsync` | `SpotifyResult<SpotifyPageResult<Chapter>>` | Retrieves a paginated list of chapters for an audiobook |

---

### `AudiobookGetAsync`

```csharp
Task<SpotifyResult<Audiobook>> AudiobookGetAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves the details of an audiobook by its Spotify identifier asynchronously. Does not throw for missing audiobooks — check `IsSuccess` on the result.

- **Parameters:**
  - `id` — The Spotify audiobook ID. Can be null to indicate no audiobook is specified.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<Audiobook>` with the audiobook details if found; otherwise, the result indicates an error or missing item.

**Usage example:**

```csharp
var result = await spotifyClient.Audiobooks.AudiobookGetAsync("7iHfbu1YPACw6oZPAFJtqe", cancellationToken);

if (result.IsSuccess)
    Console.WriteLine(result.Data?.Name);
else
    Console.WriteLine($"Error: {result.Error}");
```

---

### `AudiobookChapterGetAllAsync`

```csharp
Task<SpotifyResult<SpotifyPageResult<Chapter>>> AudiobookChapterGetAllAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves a paginated list of chapters for the specified audiobook asynchronously.

- **Parameters:**
  - `id` — The Spotify audiobook ID.
  - `limit` — Maximum number of chapters to return. Default is `20`.
  - `offset` — Index of the first chapter to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<SpotifyPageResult<Chapter>>` containing the page of chapters.

**Usage example:**

```csharp
var result = await spotifyClient.Audiobooks.AudiobookChapterGetAllAsync(
    "7iHfbu1YPACw6oZPAFJtqe",
    limit: 10,
    offset: 0,
    cancellationToken: cancellationToken);

if (result.IsSuccess)
    foreach (var chapter in result.Data?.Items ?? [])
        Console.WriteLine($"Chapter {chapter.ChapterNumber}: {chapter.Name}");
```

---

## Raw Methods

The typed methods in this service have corresponding raw counterparts that return the unprocessed JSON response as `string?`. Raw methods share the same parameter signatures but provide direct access to the raw API response.

### Available Raw Methods

| Raw Method | Typed Equivalent |
|------------|------------------|
| `AudiobookGetRawAsync` | `AudiobookGetAsync` |
| `AudiobookChapterGetAllRawAsync` | `AudiobookChapterGetAllAsync` |

**Usage example:**

```csharp
// Get raw JSON response for an audiobook
var json = await spotifyClient.Audiobooks.AudiobookGetRawAsync(
    "7iHfbu1YPACw6oZPAFJtqe", 
    cancellationToken);

Console.WriteLine(json);
```

---

### `AudiobookGetRawAsync`

```csharp
Task<string?> AudiobookGetRawAsync(string? id, CancellationToken cancellationToken = default);
```

Retrieves raw audiobook JSON from Spotify asynchronously by audiobook identifier.

- **Parameters:**
  - `id` — The Spotify audiobook ID. Can be null to indicate no audiobook is specified.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Audiobooks.AudiobookGetRawAsync("7iHfbu1YPACw6oZPAFJtqe", cancellationToken);
Console.WriteLine(json);
```

---

### `AudiobookChapterGetAllRawAsync`

```csharp
Task<string?> AudiobookChapterGetAllRawAsync(string? id, int? limit = 20, int? offset = 0, CancellationToken cancellationToken = default);
```

Retrieves raw paginated chapter JSON for the specified audiobook from Spotify asynchronously.

- **Parameters:**
  - `id` — The Spotify audiobook ID.
  - `limit` — Maximum number of chapters to return. Default is `20`.
  - `offset` — Index of the first chapter to return. Default is `0`.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Audiobooks.AudiobookChapterGetAllRawAsync(
    "7iHfbu1YPACw6oZPAFJtqe",
    limit: 5,
    cancellationToken: cancellationToken);
Console.WriteLine(json);
```

---

### `AudiobooksGetAllRawAsync`

```csharp
Task<string?> AudiobooksGetAllRawAsync(string ids, CancellationToken cancellationToken = default);
```

Retrieves raw JSON for multiple audiobooks from Spotify asynchronously.

- **Parameters:**
  - `ids` — A comma-separated list of Spotify audiobook IDs.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Audiobooks.AudiobooksGetAllRawAsync(
    "7iHfbu1YPACw6oZPAFJtqe,587ehiVcGuXjxKMECQneQ6",
    cancellationToken);
Console.WriteLine(json);
```

---

---

### `AudiobooksGetAllRawAsync`

```csharp
Task<string?> AudiobooksGetAllRawAsync(string ids, CancellationToken cancellationToken = default);
```

Retrieves raw JSON for multiple audiobooks from Spotify.

- **Parameters:**
  - `ids` — A comma-separated list of Spotify audiobook IDs.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** Raw JSON string, or `null` if no content was returned.

**Usage example:**

```csharp
var json = await spotifyClient.Audiobooks.AudiobooksGetAllRawAsync(
    "7iHfbu1YPACw6oZPAFJtqe,587ehiVcGuXjxKMECQneQ6",
    cancellationToken);
Console.WriteLine(json);
```

---

### `MyAudiobookSaveAsync`

```csharp
Task<SpotifyResult<List<bool>>> MyAudiobookSaveAsync(List<string> audiobookIds, CancellationToken cancellationToken = default);
```

Saves audiobooks to the current user's library. Requires the `user-library-modify` scope.

- **Parameters:**
  - `audiobookIds` — List of Spotify audiobook IDs to save.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<List<bool>>` with a boolean for each audiobook indicating success.

**Usage example:**

```csharp
var result = await spotifyClient.Audiobooks.MyAudiobookSaveAsync(
    new List<string> { "7iHfbu1YPACw6oZPAFJtqe" },
    cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"Saved {result.Data?.Count} audiobook(s)");
```

---

### `MyAudiobookRemoveAsync`

```csharp
Task<SpotifyResult<List<bool>>> MyAudiobookRemoveAsync(List<string> audiobookIds, CancellationToken cancellationToken = default);
```

Removes audiobooks from the current user's library. Requires the `user-library-modify` scope.

- **Parameters:**
  - `audiobookIds` — List of Spotify audiobook IDs to remove.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<List<bool>>` with a boolean for each audiobook indicating success.

**Usage example:**

```csharp
var result = await spotifyClient.Audiobooks.MyAudiobookRemoveAsync(
    new List<string> { "7iHfbu1YPACw6oZPAFJtqe" },
    cancellationToken);

if (result.IsSuccess)
    Console.WriteLine($"Removed {result.Data?.Count} audiobook(s)");
```

---

### `MyAudiobookContainsAsync`

```csharp
Task<SpotifyResult<List<bool>>> MyAudiobookContainsAsync(List<string> audiobookIds, CancellationToken cancellationToken = default);
```

Checks whether the specified audiobooks are saved in the current user's library. Requires the `user-library-read` scope.

- **Parameters:**
  - `audiobookIds` — List of Spotify audiobook IDs to check.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `SpotifyResult<List<bool>>` with a boolean for each audiobook indicating if it is saved.

**Usage example:**

```csharp
var result = await spotifyClient.Audiobooks.MyAudiobookContainsAsync(
    new List<string> { "7iHfbu1YPACw6oZPAFJtqe", "587ehiVcGuXjxKMECQneQ6" },
    cancellationToken);

if (result.IsSuccess)
    for (int i = 0; i < result.Data?.Count; i++)
        Console.WriteLine($"Audiobook {i + 1} saved: {result.Data[i]}");
```
