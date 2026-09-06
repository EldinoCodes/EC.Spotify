---
name: spotify-example
description: 'Use when: creating or updating API example code and HTTP files; when asked to run /spotify examples; after library code changes; after running SpotifyUpdateSkill'
user-invocable: true
---

# spotify-example

## Purpose
Generate and maintain example code and HTTP files that demonstrate all available methods in the EC.Spotify library.

## When to Use
- User explicitly runs `/spotify examples`
- After running SpotifyUpdateSkill to reflect new/changed methods
- When example code is outdated compared to library implementation
- When new services or methods have been added
- To provide testable examples for API usage

## Input
- Service interfaces in `src/EC.Spotify/EC.Spotify/Abstractions/Services/`
- Current controller implementations in `src/EC.Spotify/EC.Spotify.ApiExample/Controllers/`
- Current HTTP files in `src/EC.Spotify/EC.Spotify.ApiExample/HttpFiles/`

## Output
- Updated controllers in `src/EC.Spotify/EC.Spotify.ApiExample/Controllers/`
- Updated HTTP files in `src/EC.Spotify/EC.Spotify.ApiExample/HttpFiles/`
- All service methods represented as controller actions
- All controller actions have corresponding HTTP request examples

## Execution Steps

### Step 1: Analyze Service Interfaces
1. Scan all service interfaces in `Abstractions/Services/`
2. Identify all public methods with signatures
3. Extract XML documentation comments
4. Note required parameters and return types
5. Group methods by service

### Step 2: Update or Create Controllers
For each service:
1. Check if corresponding controller exists in `Controllers/`
2. If exists, compare methods against interface
3. If missing, create new controller
4. For each method in interface:
   - Create controller action method
   - Add XML documentation from interface
   - Inject `ISpotifyClient` dependency
   - Call service method via client
   - Return `JsonResult`
   - Accept `CancellationToken` parameter

### Step 3: Update HTTP Files
For each controller:
1. Check if corresponding `.http` file exists in `HttpFiles/`
2. If exists, compare requests against controller actions
3. If missing, create new HTTP file
4. For each controller action:
   - Create HTTP request example
   - Add descriptive comment
   - Use variables for common values (IDs, URLs)
   - Include all required parameters
   - Specify expected response format

### Step 4: Define Variables
At the top of each HTTP file:
1. Define host address variable
2. Define common ID variables (album IDs, track IDs, etc.)
3. Define pagination variables (offset, limit)
4. Define authentication variables if needed
5. Add comments explaining each variable

### Step 5: Add Documentation
For each controller and HTTP file:
1. Add file-level description
2. Add section comments for different method groups
3. Include parameter descriptions in comments
4. Add usage examples in comments
5. Link to Wiki documentation

### Step 6: Validate Build
1. Run `dotnet build` on ApiExample project
2. Verify all controllers compile
3. Verify all actions have correct signatures
4. Ensure all dependencies are injected correctly
5. Check for any compilation errors

### Step 7: Test HTTP Files
1. Verify HTTP file syntax is correct
2. Check variable references are valid
3. Ensure all endpoints are accessible
4. Validate request formats match API expectations

## Critical Rules

### Rule 1: Match Service Interfaces Exactly
- **ALWAYS** implement all public methods from service interfaces
- **NEVER** omit methods even if they seem trivial
- **ALWAYS** use exact method signatures from interfaces
- **REASON**: Examples must demonstrate complete API

### Rule 2: Controller Structure
- **ONE** controller per service (e.g., `AlbumsController` for `IAlbumService`)
- **INJECT** `ISpotifyClient` as constructor parameter
- **CALL** methods via `_spotifyClient.{Service}.{Method}()`
- **RETURN** `JsonResult` for consistent formatting
- **ACCEPT** `CancellationToken` on all async methods

### Rule 3: HTTP File Format
- **USE** REST Client extension format
- **DEFINE** variables at top of file
- **COMMENT** each request with description
- **INCLUDE** all required parameters
- **SPECIFY** `Accept: application/json` header

### Rule 4: Documentation Requirements
- **INCLUDE** XML documentation on all controller actions
- **COMMENT** HTTP requests with purpose and usage
- **LINK** to Wiki documentation where applicable
- **EXPLAIN** complex parameters in comments

### Rule 5: Preserve User Customizations
- **DO NOT** overwrite user-written code in controllers
- **DO** preserve custom business logic
- **DO** keep user comments and documentation
- **ASK** before making breaking changes to user code

### Rule 6: Variable Usage
- **USE** variables for all IDs and common values
- **DOCUMENT** each variable with a comment
- **PROVIDE** example values in comments
- **MAKE** it easy for users to customize

## File Locations

### Input
- `src/EC.Spotify/EC.Spotify/Abstractions/Services/*.cs` - Service interfaces
- `src/EC.Spotify/EC.Spotify.ApiExample/Controllers/*.cs` - Existing controllers
- `src/EC.Spotify/EC.Spotify.ApiExample/HttpFiles/*.http` - Existing HTTP files

### Output
- `src/EC.Spotify/EC.Spotify.ApiExample/Controllers/{Service}Controller.cs` - Controller files
- `src/EC.Spotify/EC.Spotify.ApiExample/HttpFiles/{Service}.http` - HTTP request files

## Controller Template

```csharp
using EC.Spotify.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EC.Spotify.ApiExample.Controllers;

[Route("[controller]")]
[ApiController]
public class AlbumsController(ISpotifyClient spotifyClient) : ControllerBase
{
    private readonly ISpotifyClient _spotifyClient = spotifyClient;

    /// <summary>
    /// Get a Spotify album.
    /// </summary>
    /// <param name="albumId">The Spotify album ID.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The album details.</returns>
    [HttpGet("{albumId}")]
    public async Task<IActionResult> GetAsync(string albumId, CancellationToken cancellationToken = default)
    {
        var result = await _spotifyClient.Albums.GetAsync(albumId, cancellationToken);
        return new JsonResult(result);
    }

    /// <summary>
    /// Get multiple Spotify albums.
    /// </summary>
    /// <param name="albumIds">Comma-separated list of Spotify album IDs.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The album details.</returns>
    [HttpGet("batch")]
    public async Task<IActionResult> GetMultipleAsync([FromQuery] string albumIds, CancellationToken cancellationToken = default)
    {
        var ids = albumIds.Split(',', StringSplitOptions.TrimEntries);
        var result = await _spotifyClient.Albums.GetMultipleAsync(ids, cancellationToken);
        return new JsonResult(result);
    }
}
```

## HTTP File Template

```http
@EC.Spotify.HostAddress = https://localhost:5001
@EC.Spotify.AlbumId = 7a7arAXDE0BiaMgHLhdjGF
@EC.Spotify.AlbumIds = 7a7arAXDE0BiaMgHLhdjGF,4aawyAB9vmqN3uQ7FjRGTy,1A2B3C4D5E6F7G8H9I0J1K
@EC.Spotify.Offset = 0
@EC.Spotify.Limit = 20

### Get Album
# Retrieves a single Spotify album by ID
GET {{EC.Spotify.HostAddress}}/Albums/{{EC.Spotify.AlbumId}}
Accept: application/json

### Get Multiple Albums
# Retrieves multiple Spotify albums by comma-separated IDs
GET {{EC.Spotify.HostAddress}}/Albums/batch?albumIds={{EC.Spotify.AlbumIds}}
Accept: application/json

### Get Album Tracks
# Retrieves tracks from a Spotify album with pagination
GET {{EC.Spotify.HostAddress}}/Albums/{{EC.Spotify.AlbumId}}/tracks?offset={{EC.Spotify.Offset}}&limit={{EC.Spotify.Limit}}
Accept: application/json
```

## Validation Commands

```powershell
# Build ApiExample project
dotnet build src/EC.Spotify/EC.Spotify.ApiExample/EC.Spotify.ApiExample.csproj

# Count controllers
Get-ChildItem src/EC.Spotify/EC.Spotify.ApiExample/Controllers/*.cs | Measure-Object

# Count HTTP files
Get-ChildItem src/EC.Spotify/EC.Spotify.ApiExample/HttpFiles/*.http | Measure-Object

# Verify all services have controllers
$services = Get-ChildItem src/EC.Spotify/EC.Spotify/Abstractions/Services/*.cs | Select-Object -ExpandProperty BaseName -Unique
$controllers = Get-ChildItem src/EC.Spotify/EC.Spotify.ApiExample/Controllers/*.cs | Select-Object -ExpandProperty BaseName -Unique
$missing = $services | Where-Object { $_ -notlike "*Controller" -and $_ -notin $controllers }
$missing
```

## Example Execution

### Full Example Generation
```
Skill: spotify-example
Action: Generating example code and HTTP files
Input: All service interfaces
Changes:
  - Created 10 new controllers (one per service)
  - Created 10 new HTTP files (one per service)
  - Added 85 controller actions (all service methods)
  - Added 85 HTTP request examples
  - Defined common variables for IDs and pagination
Validation: dotnet build SUCCESS
Status: SUCCESS
```

### Incremental Update
```
Skill: spotify-example
Action: Updating examples for recent changes
Changes:
  - Updated 3 controllers (new methods added)
  - Updated 3 HTTP files (new requests)
  - Added 12 new controller actions
  - Added 12 new HTTP request examples
Validation: dotnet build SUCCESS
Status: SUCCESS
```

### No Changes Needed
```
Skill: spotify-example
Action: Checking examples against service interfaces
Changes: None (examples already up to date)
Validation: All methods have controller actions
Status: SKIPPED
```

## Dependencies
- **Runs after**: SpotifyUpdateSkill (must match current library implementation)
- **Runs before**: SpotifyTestSkill (tests can use examples as reference)
- **Requires**: dotnet CLI for build validation

## Troubleshooting

### Build Errors
**Error**: "dotnet build failed"  
**Fix**: Check for:
- Missing method implementations
- Incorrect method signatures
- Missing using statements
- Incorrect dependency injection

### Missing Controller
**Error**: "No controller for {ServiceName}"  
**Fix**: Create new controller following the template

### Missing HTTP File
**Error**: "No HTTP file for {ServiceName}"  
**Fix**: Create new HTTP file following the template

### Variable Errors
**Error**: "Undefined variable {{VariableName}}"  
**Fix**: Add variable definition at top of HTTP file

### Duplicate Actions
**Error**: "Multiple actions with same route"  
**Fix**: Ensure each method has unique route template or action name
