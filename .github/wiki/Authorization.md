# IAuthorizationService

`IAuthorizationService` is part of the `EC.Spotify.Abstractions` namespace and manages the OAuth 2.0 Authorization Code Flow for the Spotify API. It is exposed via the `ISpotifyClient.Authorization` property.

```csharp
IAuthorizationService authorization = spotifyClient.Authorization;
```

---

## Methods

### `Validate`

```csharp
Task<string?> ValidateAsync(CancellationToken cancellationToken = default);
```

Validates the current authentication state and determines whether user authorization is required.

- **Returns:** A URL string to redirect the user for authorization if authorization is required; otherwise `null` if the user is already authorized.

**Usage example:**

```csharp
var authUrl = await spotifyClient.Authorization.ValidateAsync(cancellationToken);

if (!string.IsNullOrEmpty(authUrl))
{
    // Redirect user to Spotify login
    return Redirect(authUrl);
}

// Already authorized
return Ok("Authorized");
```

---

### `AuthorizationCodeUrl`

```csharp
string? AuthorizationCodeUrl();
```

Generates the URL to initiate the OAuth 2.0 authorization code flow. The returned URL includes all required query parameters and should be used to redirect the user agent to the Spotify authorization server.

- **Returns:** A string containing the authorization endpoint URL.

**Usage example:**

```csharp
var url = spotifyClient.Authorization.AuthorizationCodeUrl();
Console.WriteLine($"Visit this URL to authorize: {url}");
```

---

### `AuthorizationCodeAddAsync`

```csharp
Task<bool> AuthorizationCodeAddAsync(string? authorizationCode, string? state = null, CancellationToken cancellationToken = default);
```

Asynchronously adds a new authorization code to the underlying store. This is typically called after the user is redirected back from the Spotify authorization server with a `code` query parameter.

- **Parameters:**
  - `authorizationCode` — The authorization code received from Spotify's callback.
  - `state` — Optional state value returned from the authorization server for CSRF validation.
  - `cancellationToken` — Token to cancel the operation.
- **Returns:** `true` if the authorization code was added successfully; otherwise `false`.

**Usage example:**

```csharp
// In a callback/response endpoint, e.g. /authorization/response?code=...&state=...
bool success = await spotifyClient.Authorization.AuthorizationCodeAddAsync(code, state, cancellationToken);
```

---

### `AuthorizationCodeGetAsync`

```csharp
Task<string?> AuthorizationCodeGetAsync(CancellationToken cancellationToken = default);
```

Asynchronously retrieves the current authorization code from the store, if one is available.

- **Returns:** The authorization code as a string, or `null` if no code is available.

**Usage example:**

```csharp
var code = await spotifyClient.Authorization.AuthorizationCodeGetAsync(cancellationToken);

if (code is not null)
{
    Console.WriteLine($"Current authorization code: {code}");
}
```

---

### `AuthorizationCodeRemoveAsync`

```csharp
Task<bool> AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default);
```

Asynchronously removes the current authorization code from the underlying store.

- **Returns:** `true` if the authorization code was successfully removed; otherwise `false`.

**Usage example:**

```csharp
bool removed = await spotifyClient.Authorization.AuthorizationCodeRemoveAsync(cancellationToken);

if (removed)
{
    Console.WriteLine("Authorization code cleared.");
}
```

---

### `AuthorizationTokenGetAsync`

```csharp
Task<AuthToken?> AuthorizationTokenGetAsync(CancellationToken cancellationToken = default);
```

Asynchronously retrieves the current authentication token, if available.

- **Returns:** An [`AuthToken`](#authtoken-model) instance if a token exists; otherwise `null`.

**Usage example:**

```csharp
var token = await spotifyClient.Authorization.AuthorizationTokenGetAsync(cancellationToken);

if (token is not null)
{
    Console.WriteLine($"Access token: {token.AccessToken}");
    Console.WriteLine($"Expires in:   {token.ExpiresIn}s");
    Console.WriteLine($"Scopes:       {token.Scope}");
}
```

---

### `AuthorizationTokenReset`

```csharp
Task<bool> AuthorizationTokenReset();
```

Attempts to reset (invalidate/clear) the current authentication token asynchronously.

- **Returns:** `true` if the token was successfully reset; otherwise `false`.

**Usage example:**

```csharp
bool reset = await spotifyClient.Authorization.AuthorizationTokenReset();

if (reset)
{
    Console.WriteLine("Token has been reset. Re-authorization will be required.");
}
```

---

## AuthToken Model

Returned by `AuthorizationTokenGetAsync`. Represents an OAuth 2.0 access token response from Spotify.

| Property       | JSON Key        | Type     | Description                                            |
|----------------|-----------------|----------|--------------------------------------------------------|
| `AccessToken`  | `access_token`  | `string?`| The bearer token used to authenticate API requests.    |
| `TokenType`    | `token_type`    | `string?`| The type of token (typically `"Bearer"`).              |
| `ExpiresIn`    | `expires_in`    | `double` | Lifetime of the access token in seconds.               |
| `RefreshToken` | `refresh_token` | `string?`| Token used to obtain a new access token when expired.  |
| `Scope`        | `scope`         | `string?`| Space-separated list of scopes granted by the user.    |

---

## Typical Authorization Flow

The following sequence illustrates the full OAuth 2.0 authorization code flow using `IAuthorizationService`:

```
1. GET /authorization/validate
   → Calls ValidateAsync() → returns redirect URL → browser navigates to Spotify login

2. User logs in and grants access on Spotify's authorization server

3. Spotify redirects to GET /authorization/response?code=<code>&state=<state>
   → Calls AuthorizationCodeAddAsync(code, state) → stores the code
   → Redirects back to /authorization/validate

4. GET /authorization/validate (again)
   → Calls ValidateAsync() detects a valid token → returns null → responds with "Authorized"

5. GET /authorization/token
   → Calls AuthorizationTokenGetAsync() → returns the active AuthToken

6. POST /authorization/token/reset  (optional)
   → Calls AuthorizationTokenReset() → clears the stored token
```
