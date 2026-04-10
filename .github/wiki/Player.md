# IPlayerService

`IPlayerService` is part of the `EC.Spotify.Abstractions` namespace and provides full control over Spotify playback — state, devices, queue, transport, and settings. It is exposed via the `ISpotifyClient.Player` property.

```csharp
IPlayerService player = spotifyClient.Player;
```

---

## Methods

| Method | Scope Required | Returns | Description |
|--------|---------------|---------|-------------|
| `QueueGetAsync` | `user-read-playback-state`, `user-read-currently-playing` | `SpotifyResult<PlayerQueue>` | Retrieves the current playback queue |
| `QueueAddAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Adds a track to the queue |
| `DeviceGetAllAsync` | `user-read-playback-state` | `SpotifyResult<List<Device>>` | Retrieves all available devices |
| `TransferAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Transfers playback to a device |
| `StateGetAsync` | `user-read-playback-state` | `SpotifyResult<PlayerState>` | Retrieves the current playback state |
| `CurrentlyPlayingGetAsync` | `user-read-playback-state` | `SpotifyResult<PlayerState>` | Retrieves the currently playing item |
| `PlayAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Starts playback of specified tracks |
| `PauseAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Pauses playback |
| `NextAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Skips to the next track |
| `PreviousAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Skips to the previous track |
| `SeekAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Seeks to a position in the current track |
| `RepeatAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Sets the repeat mode |
| `ShuffleAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Enables or disables shuffle |
| `VolumeAsync` | `user-modify-playback-state` | `SpotifyResult<bool>` | Sets the playback volume |

---

### `QueueGetAsync`

```csharp
Task<SpotifyResult<PlayerQueue>> QueueGetAsync(CancellationToken cancellationToken = default);
```

Retrieves the current playback queue for the user asynchronously. Requires `user-read-playback-state` and `user-read-currently-playing` scopes.

**Usage example:**

```csharp
var result = await spotifyClient.Player.QueueGetAsync(cancellationToken);

if (result.IsSuccess)
{
    Console.WriteLine($"Now playing: {result.Data?.CurrentlyPlaying?.Name}");
    foreach (var item in result.Data?.Queue ?? [])
        Console.WriteLine($"  Up next: {item.Name}");
}
```

---

### `QueueAddAsync`

```csharp
Task<SpotifyResult<bool>> QueueAddAsync(string? trackId, string? deviceId = null, CancellationToken cancellationToken = default);
```

Adds the specified track to the playback queue on the user's Spotify account asynchronously. Does not start playback. Requires the `user-modify-playback-state` scope.

- **Parameters:**
  - `trackId` — The Spotify track ID to add to the queue.
  - `deviceId` — Target device ID. If null, the user's currently active device is used.

**Usage example:**

```csharp
var result = await spotifyClient.Player.QueueAddAsync("1301WleyT98MSxVHPZCA6M", cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Queued" : "Failed to queue");
```

---

### `DeviceGetAllAsync`

```csharp
Task<SpotifyResult<List<Device>>> DeviceGetAllAsync(CancellationToken cancellationToken = default);
```

Retrieves a list of all available devices associated with the user's Spotify account asynchronously. Requires the `user-read-playback-state` scope.

**Usage example:**

```csharp
var result = await spotifyClient.Player.DeviceGetAllAsync(cancellationToken);

if (result.IsSuccess)
    foreach (var device in result.Data ?? [])
        Console.WriteLine($"{device.Name} ({device.Type}) — Active: {device.IsActive}");
```

---

### `TransferAsync`

```csharp
Task<SpotifyResult<bool>> TransferAsync(string? deviceId, bool play = false, CancellationToken cancellationToken = default);
```

Transfers playback to the specified device asynchronously, optionally starting playback immediately. Requires the `user-modify-playback-state` scope.

- **Parameters:**
  - `deviceId` — Target device ID. If null, the currently active device is used.
  - `play` — Set to `true` to start playback immediately on the target device.

**Usage example:**

```csharp
var result = await spotifyClient.Player.TransferAsync("abc123deviceId", play: true, cancellationToken);
Console.WriteLine(result.Data ? "Transferred" : "Transfer failed");
```

---

### `StateGetAsync`

```csharp
Task<SpotifyResult<PlayerState>> StateGetAsync(CancellationToken cancellationToken = default);
```

Retrieves the current playback state for the user's Spotify account asynchronously, including the active device, repeat state, shuffle state, and the currently playing item. Requires the `user-read-playback-state` scope.

**Usage example:**

```csharp
var result = await spotifyClient.Player.StateGetAsync(cancellationToken);

if (result.IsSuccess)
{
    var state = result.Data!;
    Console.WriteLine($"Playing: {state.IsPlaying}");
    Console.WriteLine($"Shuffle: {state.ShuffleState}");
    Console.WriteLine($"Repeat: {state.RepeatState}");
}
```

---

### `CurrentlyPlayingGetAsync`

```csharp
Task<SpotifyResult<PlayerState>> CurrentlyPlayingGetAsync(CancellationToken cancellationToken = default);
```

Retrieves the item currently playing on the user's Spotify account asynchronously. Returns an empty result if nothing is currently playing. Requires the `user-read-playback-state` scope.

**Usage example:**

```csharp
var result = await spotifyClient.Player.CurrentlyPlayingGetAsync(cancellationToken);

if (result.IsSuccess && result.Data?.IsPlaying == true)
    Console.WriteLine($"Currently playing: {result.Data.Item?.Name}");
```

---

### `PlayAsync`

```csharp
Task<SpotifyResult<bool>> PlayAsync(string? deviceId, List<string>? trackUris, CancellationToken cancellationToken = default);
```

Starts playback of the specified tracks on the given Spotify device asynchronously. If `trackUris` is null or empty, playback resumes the user's current queue. Requires the `user-modify-playback-state` scope.

- **Parameters:**
  - `deviceId` — Target device ID. If null, the currently active device is used.
  - `trackUris` — List of Spotify track URIs to play (e.g. `"spotify:track:1301WleyT98MSxVHPZCA6M"`).

**Usage example:**

```csharp
var uris = new List<string> { "spotify:track:1301WleyT98MSxVHPZCA6M", "spotify:track:4iV5W9uYEdYUVa79Axb7Rh" };
var result = await spotifyClient.Player.PlayAsync(deviceId: null, trackUris: uris, cancellationToken);
Console.WriteLine(result.Data ? "Playback started" : "Failed");
```

---

### `PauseAsync`

```csharp
Task<SpotifyResult<bool>> PauseAsync(string? deviceId = null, CancellationToken cancellationToken = default);
```

Pauses playback on the user's Spotify account. Requires the `user-modify-playback-state` scope.

**Usage example:**

```csharp
var result = await spotifyClient.Player.PauseAsync(cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Paused" : "Failed to pause");
```

---

### `NextAsync`

```csharp
Task<SpotifyResult<bool>> NextAsync(string? deviceId = null, CancellationToken cancellationToken = default);
```

Skips to the next track in the user's currently active Spotify player. Requires the `user-modify-playback-state` scope and an active playback session.

**Usage example:**

```csharp
var result = await spotifyClient.Player.NextAsync(cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Skipped to next" : "Failed");
```

---

### `PreviousAsync`

```csharp
Task<SpotifyResult<bool>> PreviousAsync(string? deviceId = null, CancellationToken cancellationToken = default);
```

Skips to the previous track in the user's currently active Spotify player. Requires the `user-modify-playback-state` scope and an active playback session.

**Usage example:**

```csharp
var result = await spotifyClient.Player.PreviousAsync(cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Skipped to previous" : "Failed");
```

---

### `SeekAsync`

```csharp
Task<SpotifyResult<bool>> SeekAsync(int positionMs, string? deviceId = null, CancellationToken cancellationToken = default);
```

Seeks to the specified position in the currently playing track. Does not start playback if the player is paused. Requires the `user-modify-playback-state` scope.

- **Parameters:**
  - `positionMs` — The position in milliseconds to seek to. Must be between 0 and the track's duration.
  - `deviceId` — Target device ID. If null, the currently active device is used.

**Usage example:**

```csharp
// Seek to 1 minute 30 seconds
var result = await spotifyClient.Player.SeekAsync(90_000, cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Seeked" : "Failed");
```

---

### `RepeatAsync`

```csharp
Task<SpotifyResult<bool>> RepeatAsync(PlayerRepeatMode playerRepeatMode = PlayerRepeatMode.Off, string? deviceId = null, CancellationToken cancellationToken = default);
```

Sets the repeat mode for the current Spotify playback session asynchronously. Does not change playback state. Requires the `user-modify-playback-state` scope.

#### `PlayerRepeatMode` enum

| Value | Description |
|-------|-------------|
| `Off` | No repeat |
| `Track` | Repeat the current track |
| `Context` | Repeat the current context (album, playlist, etc.) |

**Usage example:**

```csharp
var result = await spotifyClient.Player.RepeatAsync(PlayerRepeatMode.Track, cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Repeat set" : "Failed");
```

---

### `ShuffleAsync`

```csharp
Task<SpotifyResult<bool>> ShuffleAsync(PlayerShuffleMode playerShuffleMode = PlayerShuffleMode.Off, string? deviceId = null, CancellationToken cancellationToken = default);
```

Enables or disables shuffle mode for the user's playback. Applied to the currently active device if `deviceId` is null. Requires the `user-modify-playback-state` scope.

#### `PlayerShuffleMode` enum

| Value | Description |
|-------|-------------|
| `Off` | Shuffle disabled |
| `On` | Shuffle enabled |

**Usage example:**

```csharp
var result = await spotifyClient.Player.ShuffleAsync(PlayerShuffleMode.On, cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Shuffle on" : "Failed");
```

---

### `VolumeAsync`

```csharp
Task<SpotifyResult<bool>> VolumeAsync(int volumePercent, string? deviceId = null, CancellationToken cancellationToken = default);
```

Sets the playback volume for the specified Spotify device asynchronously. Does not change playback state. Requires the `user-modify-playback-state` scope.

- **Parameters:**
  - `volumePercent` — Desired volume level as a percentage (0–100).
  - `deviceId` — Target device ID. If null, the currently active device is used.

**Usage example:**

```csharp
// Set volume to 50%
var result = await spotifyClient.Player.VolumeAsync(50, cancellationToken: cancellationToken);
Console.WriteLine(result.Data ? "Volume set" : "Failed");
```
