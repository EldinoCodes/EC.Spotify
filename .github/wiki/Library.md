# ILibraryService

`ILibraryService` is part of the `EC.Spotify.Abstractions` namespace and manages the current user's Spotify library — saving, checking, and removing items. It is exposed via the `ISpotifyClient.Library` property.

```csharp
ILibraryService library = spotifyClient.Library;
```

Items are identified using `ReferenceItem`, which wraps a Spotify ID and a `ReferenceItemType` to build the appropriate Spotify URI automatically.

---

## `ReferenceItem`

```csharp
var item = new ReferenceItem { Id = "4aawyAB9vmqN3uQ7FjRGTy", Type = ReferenceItemType.Album };
// item.Uri => "spotify:album:4aawyAB9vmqN3uQ7FjRGTy"
```

### `ReferenceItemType` enum

| Value | URI segment |
|-------|-------------|
| `Album` | `album` |
| `Audiobook` | `audiobook` |
| `Episode` | `episode` |
| `Playlist` | `playlist` |
| `Show` | `show` |
| `Track` | `track` |
| `User` | `user` |

---

## Methods

| Method | Scope Required | Returns | Description |
|--------|---------------|---------|-------------|
| `LibraryAddAsync` | `user-library-modify` | `SpotifyResult<bool>` | Saves a single item to the library |
| `LibraryAddAllAsync` | `user-library-modify` | `SpotifyResult<List<bool>>` | Saves multiple items in batches of up to 40 |
| `LibraryCheckAsync` | `user-library-read` | `SpotifyResult<bool>` | Checks if a single item is saved |
| `LibraryCheckAllAsync` | `user-library-read` | `SpotifyResult<List<bool>>` | Checks multiple items in batches of up to 40 |
| `LibraryRemoveAsync` | `user-library-modify` | `SpotifyResult<bool>` | Removes a single item from the library |
| `LibraryRemoveAllAsync` | `user-library-modify` | `SpotifyResult<List<bool>>` | Removes multiple items in batches of up to 40 |

---

### `LibraryAddAsync`

```csharp
Task<SpotifyResult<bool>> LibraryAddAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);
```

Saves the specified item to the current user's Spotify library asynchronously. If `libraryItem` is `null`, no action is taken. Requires the `user-library-modify` scope.

**Usage example:**

```csharp
var item = new ReferenceItem { Id = "4aawyAB9vmqN3uQ7FjRGTy", Type = ReferenceItemType.Album };
var result = await spotifyClient.Library.LibraryAddAsync(item, cancellationToken);

Console.WriteLine(result.IsSuccess && result.Data ? "Saved" : "Failed");
```

---

### `LibraryAddAllAsync`

```csharp
Task<SpotifyResult<List<bool>>> LibraryAddAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
```

Saves the specified items to the current user's Spotify library asynchronously. Items are sent in batches of up to 40 per request. Requires the `user-library-modify` scope.

**Usage example:**

```csharp
var items = new List<ReferenceItem>
{
    new() { Id = "4aawyAB9vmqN3uQ7FjRGTy", Type = ReferenceItemType.Album },
    new() { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track }
};

var result = await spotifyClient.Library.LibraryAddAllAsync(items, cancellationToken);

if (result.IsSuccess)
    for (int i = 0; i < result.Data!.Count; i++)
        Console.WriteLine($"Item {i}: {(result.Data[i] ? "saved" : "failed")}");
```

---

### `LibraryCheckAsync`

```csharp
Task<SpotifyResult<bool>> LibraryCheckAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);
```

Checks whether the specified item is saved in the current user's Spotify library asynchronously. Returns `false` if `libraryItem` is `null`. Requires the `user-library-read` scope.

**Usage example:**

```csharp
var item = new ReferenceItem { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track };
var result = await spotifyClient.Library.LibraryCheckAsync(item, cancellationToken);

Console.WriteLine(result.Data ? "In library" : "Not in library");
```

---

### `LibraryCheckAllAsync`

```csharp
Task<SpotifyResult<List<bool>>> LibraryCheckAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
```

Checks whether the specified items are saved in the current user's Spotify library asynchronously. Items are checked in batches of up to 40 per request. Requires the `user-library-read` scope.

**Usage example:**

```csharp
var items = new List<ReferenceItem>
{
    new() { Id = "4aawyAB9vmqN3uQ7FjRGTy", Type = ReferenceItemType.Album },
    new() { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track }
};

var result = await spotifyClient.Library.LibraryCheckAllAsync(items, cancellationToken);

if (result.IsSuccess)
    for (int i = 0; i < result.Data!.Count; i++)
        Console.WriteLine($"Item {i}: {(result.Data[i] ? "saved" : "not saved")}");
```

---

### `LibraryRemoveAsync`

```csharp
Task<SpotifyResult<bool>> LibraryRemoveAsync(ReferenceItem? libraryItem, CancellationToken cancellationToken = default);
```

Removes the specified item from the current user's Spotify library asynchronously. If `libraryItem` is `null`, no action is taken. Requires the `user-library-modify` scope.

**Usage example:**

```csharp
var item = new ReferenceItem { Id = "4aawyAB9vmqN3uQ7FjRGTy", Type = ReferenceItemType.Album };
var result = await spotifyClient.Library.LibraryRemoveAsync(item, cancellationToken);

Console.WriteLine(result.IsSuccess && result.Data ? "Removed" : "Failed");
```

---

### `LibraryRemoveAllAsync`

```csharp
Task<SpotifyResult<List<bool>>> LibraryRemoveAllAsync(List<ReferenceItem> libraryItems, CancellationToken cancellationToken = default);
```

Removes the specified items from the current user's Spotify library asynchronously. Items are sent in batches of up to 40 per request. Requires the `user-library-modify` scope.

**Usage example:**

```csharp
var items = new List<ReferenceItem>
{
    new() { Id = "4aawyAB9vmqN3uQ7FjRGTy", Type = ReferenceItemType.Album },
    new() { Id = "1301WleyT98MSxVHPZCA6M", Type = ReferenceItemType.Track }
};

var result = await spotifyClient.Library.LibraryRemoveAllAsync(items, cancellationToken);

if (result.IsSuccess)
    for (int i = 0; i < result.Data!.Count; i++)
        Console.WriteLine($"Item {i}: {(result.Data[i] ? "removed" : "failed")}");
```
