# Getting Started with EC.Spotify

EC.Spotify is a .NET client library for the Spotify Web API. It provides strongly-typed access to albums, artists, tracks, playlists, playback control, library management, and more — all wired up through a single `ISpotifyClient` interface and standard .NET dependency injection.

---

## Installation

Install the package from NuGet:

### Package Manager Console
```powershell
Install-Package EC.Spotify
```

### .NET CLI
```bash
dotnet add package EC.Spotify
```

---

## Configuration

EC.Spotify is configured through `SpotifyOptions`. All properties map directly to your Spotify application credentials, which you can obtain from the [Spotify Developer Dashboard](https://developer.spotify.com/dashboard).

| Property | Type | Description |
|----------|------|-------------|
| `ClientId` | `string?` | Your Spotify application client ID |
| `ClientSecret` | `string?` | Your Spotify application client secret |
| `RedirectUri` | `string?` | The URI Spotify redirects to after user authorization |
| `Scopes` | `List<string>` | The OAuth scopes your application requires |
| `VerboseLogging` | `bool` | Enables debug-level logging for each request |

---

## Registration

Call `AddSpotify` on your `IServiceCollection` during application startup. There are two approaches:

### Option 1 — Configuration Delegate

```csharp
using EC.Spotify;

builder.Services.AddSpotify(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.RedirectUri = "https://localhost:5001/authorization/response";
    options.Scopes =
    [
        "user-read-playback-state",
        "user-modify-playback-state",
        "user-library-read",
        "user-library-modify"
    ];
});
```

### Option 2 — `appsettings.json` Configuration Section

**`appsettings.json`:**
```json
{
  "Spotify": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "RedirectUri": "https://localhost:5001/authorization/response",
    "Scopes": [
      "user-read-playback-state",
      "user-modify-playback-state",
      "user-library-read",
      "user-library-modify"
    ]
  }
}
```

**`Program.cs`:**
```csharp
using EC.Spotify;

builder.Services.AddSpotify(builder.Configuration.GetSection("Spotify"));
```

Both approaches register all EC.Spotify services as singletons, including `ISpotifyClient`, which is the primary entry point for the entire library.

---

## Scopes

Spotify uses OAuth 2.0 scopes to control which API endpoints a user grants your application access to. You must declare all required scopes upfront during registration — a missing scope will result in a `403 Forbidden` error at runtime.

Common scopes used across EC.Spotify services:

| Scope | Used By |
|-------|---------|
| `user-read-playback-state` | `Player.StateGetAsync`, `Player.DeviceGetAllAsync`, `Player.QueueGetAsync` |
| `user-modify-playback-state` | `Player.PlayAsync`, `Player.PauseAsync`, `Player.NextAsync`, `Player.VolumeAsync`, etc. |
| `user-read-currently-playing` | `Player.QueueGetAsync`, `Player.CurrentlyPlayingGetAsync` |
| `user-library-read` | `Library.LibraryCheckAsync`, `User.MyTrackGetAllAsync`, etc. |
| `user-library-modify` | `Library.LibraryAddAsync`, `Library.LibraryRemoveAsync`, etc. |
| `user-top-read` | `User.MyTopItemGetAllAsync` |
| `playlist-read-private` | `Playlists.PlaylistItemGetAllAsync` |
| `playlist-modify-public` | `Playlists.PlaylistItemAddAsync`, `Playlists.PlaylistDetailUpdateAsync`, etc. |
| `playlist-modify-private` | `Playlists.PlaylistItemAddAsync`, `Playlists.PlaylistDetailUpdateAsync`, etc. |
| `ugc-image-upload` | `Playlists.PlaylistImageAddAsync` |

---

## Authorization

EC.Spotify uses the **OAuth 2.0 Authorization Code Flow**. This requires redirecting the user to Spotify to grant consent, then receiving an authorization code back via a callback URL. The library manages the code, token exchange, and token refresh automatically.

The entry point for this entire flow is a single method: **`ValidateAsync`**.

---

### `ValidateAsync`

```csharp
Task<string?> ValidateAsync(CancellationToken cancellationToken = default);
```

`ValidateAsync` is the recommended way to check and enforce authorization state before making any API calls. Call it at the start of a request — or from a dedicated authorization endpoint — and branch on its return value.

**Return values:**

| Return Value | Meaning |
|---|---|
| `string` (a URL) | Authorization is required. Redirect the user to this URL. |
| `null` | The user is fully authorized. API calls can proceed. |

**What `ValidateAsync` does internally:**

1. Generates a Spotify authorization URL (with a CSRF `state` parameter stored in cache).
2. Checks whether an authorization code is already stored. If not, returns the authorization URL.
3. If a code exists, attempts to retrieve or exchange it for an access token. If that fails, returns the authorization URL.
4. If a valid token is present, returns `null` — authorization is complete.

Token refresh is handled automatically. You do not need to manage token expiry manually.

**Usage example:**

```csharp
[HttpGet("validate")]
public async Task<IActionResult> ValidateAsync(CancellationToken cancellationToken = default)
{
    var authUrl = await _spotifyClient.Authorization.ValidateAsync(cancellationToken);

    // Not yet authorized — redirect the user to Spotify
    if (!string.IsNullOrEmpty(authUrl))
        return Redirect(authUrl);

    // Authorized — continue with API calls
    return Ok("Authorized");
}
```

---

### Authorization Flow — Step by Step

#### Step 1 — User hits your validate endpoint

Your application calls `ValidateAsync`. Since no authorization code exists yet, it returns the Spotify authorization URL. You redirect the user there.

```csharp
var authUrl = await _spotifyClient.Authorization.ValidateAsync(cancellationToken);
if (!string.IsNullOrEmpty(authUrl))
    return Redirect(authUrl);
```

#### Step 2 — User grants consent on Spotify

Spotify prompts the user to log in and approve the requested scopes. On approval, Spotify redirects the user back to your configured `RedirectUri` with `code` and `state` query parameters appended.

```
https://localhost:5001/authorization/response?code=AQD...&state=abc123
```

#### Step 3 — Handle the callback

Your callback endpoint receives the `code` and `state` from Spotify. Pass both to `AuthorizationCodeAddAsync`. The `state` parameter is validated against the CSRF token generated in Step 1 — a mismatch is rejected automatically.

```csharp
[HttpGet("response")]
public async Task<IActionResult> Callback(
    [FromQuery] string? code,
    [FromQuery] string? state,
    CancellationToken cancellationToken = default)
{
    await _spotifyClient.Authorization.AuthorizationCodeAddAsync(code, state, cancellationToken);

    // Redirect back to validate to complete the flow
    return RedirectToAction("Validate");
}
```

#### Step 4 — `Validate` confirms authorization

On the second call to `Validate`, the authorization code is now stored. The library exchanges it for an access token and caches the token internally. `Validate` returns `null`, confirming the user is authorized.

```csharp
var authUrl = await _spotifyClient.Authorization.Validate(cancellationToken);
// authUrl is now null — the user is authorized
return Ok("Authorized");
```

---

### Complete Authorization Controller Example

```csharp
using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthorizationController(ISpotifyClient spotifyClient) : ControllerBase
{
    [HttpGet("validate", Name = "authorizationValidate")]
    public async Task<IActionResult> Validate(CancellationToken cancellationToken = default)
    {
        var authUrl = await spotifyClient.Authorization.Validate(cancellationToken);

        return !string.IsNullOrEmpty(authUrl)
            ? Redirect(authUrl)
            : Ok("Authorized");
    }

    [HttpGet("response")]
    public async Task<IActionResult> Callback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        CancellationToken cancellationToken = default)
    {
        await spotifyClient.Authorization.AuthorizationCodeAddAsync(code, state, cancellationToken);
        return RedirectToRoute("authorizationValidate");
    }
}
```

---

### Authorization Flow Diagram

```
Your App                    EC.Spotify                  Spotify
   |                            |                           |
   |-- GET /validate ---------->|                           |
   |                            |-- Validate() ------------>|
   |                            |   (no code stored)        |
   |<-- Redirect(authUrl) ------|                           |
   |                            |                           |
   |-- GET authUrl (browser) -------------------------------->|
   |                            |                           |
   |<-- Redirect to /response?code=...&state=... -----------|
   |                            |                           |
   |-- GET /response ---------->|                           |
   |                            |-- AuthorizationCodeAddAsync|
   |<-- Redirect /validate -----|   (code + state stored)   |
   |                            |                           |
   |-- GET /validate ---------->|                           |
   |                            |-- Validate() ------------>|
   |                            |   (code found)            |
   |                            |-- Exchange code for token >|
   |                            |<-- AccessToken + Refresh --|
   |<-- 200 OK "Authorized" ----|                           |
```

---

### Additional Authorization Methods

These are available via `ISpotifyClient.Authorization` but are typically not needed in a standard flow:

| Method | Description |
|--------|-------------|
| `AuthorizationCodeUrl()` | Returns the Spotify authorization URL without checking state. Useful for building custom flows. |
| `AuthorizationCodeGetAsync()` | Returns the currently cached authorization code, or `null` if not present. |
| `AuthorizationCodeRemoveAsync()` | Clears the stored authorization code from cache. |
| `AuthorizationTokenGetAsync()` | Returns the current `AuthToken` (access token, refresh token, expiry, scopes), or `null` if not authorized. |
| `AuthorizationTokenReset()` | Clears the cached token, forcing re-authorization on the next `Validate` call. |

---

## Making API Calls

Once `Validate` returns `null`, all service properties on `ISpotifyClient` are available. Inject `ISpotifyClient` wherever you need it and call the appropriate service method.

Every method returns a `SpotifyResult<T>`, which wraps either a successful data payload or an error — never throws for API-level failures.

### `SpotifyResult<T>`

| Property | Type | Description |
|----------|------|-------------|
| `IsSuccess` | `bool` | `true` if no error occurred |
| `Data` | `T?` | The response payload; `null` on failure |
| `Error` | `SpotifyError?` | Error details; `null` on success |

### `SpotifyError`

| Property | Type | Description |
|----------|------|-------------|
| `Status` | `int?` | HTTP status code (`500` for internal errors) |
| `Message` | `string?` | Human-readable error description |
| `Reason` | `string?` | Machine-readable reason or exception type name |

### Usage Pattern

```csharp
var result = await _spotifyClient.Player.StateGetAsync(cancellationToken);

if (result.IsSuccess)
{
    var state = result.Data!;
    Console.WriteLine($"Playing: {state.IsPlaying}");
}
else
{
    Console.WriteLine($"Error {result.Error?.Status}: {result.Error?.Message}");
}
```

### Available Services

All services are accessed through `ISpotifyClient`:

| Property | Interface | Description |
|----------|-----------|-------------|
| `Albums` | `IAlbumService` | Retrieve album data |
| `Artists` | `IArtistService` | Retrieve artist data |
| `Audiobooks` | `IAudiobookService` | Retrieve audiobook data |
| `Authorization` | `IAuthorizationService` | Manage OAuth authorization |
| `Chapters` | `IChapterService` | Retrieve chapter data |
| `Episodes` | `IEpisodeService` | Retrieve episode data |
| `Library` | `ILibraryService` | Save and remove library items |
| `Player` | `IPlayerService` | Control and query playback |
| `Playlists` | `IPlaylistService` | Access and manage playlists |
| `Search` | `ISearchService` | Search the Spotify catalog |
| `Shows` | `IShowService` | Retrieve show data |
| `Tracks` | `ITrackService` | Retrieve track data |
| `User` | `IUserService` | Access current user data |

> Calling a method that requires a scope not included in your `SpotifyOptions.Scopes` will result in a `403 Forbidden` response from Spotify, surfaced as `SpotifyResult.IsSuccess == false`.
