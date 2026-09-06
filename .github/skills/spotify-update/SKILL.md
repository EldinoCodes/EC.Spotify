---
name: spotify-update
description: 'Use when: updating EC.Spotify library to match Spotify API documentation; when asked to run /spotify update; when API methods have changed; after running SpotifyDocsSkill'
user-invocable: true
---

# spotify-update

## Purpose
Update the EC.Spotify library code (interfaces, implementations, registrations) to match the Spotify API Method Summary Markdown file as the source of truth.

## When to Use
- User explicitly runs `/spotify update`
- After running SpotifyDocsSkill to ensure latest API methods are known
- When Spotify Web API has new methods or deprecated methods
- Before running SpotifyWikiSkill, SpotifyExampleSkill, or SpotifyTestSkill

## Input
- `docs/spotify-api-methods-summary.md` - Source of truth for API methods
- Current service interfaces in `src/EC.Spotify/EC.Spotify/Abstractions/Services/`
- Current service implementations in `src/EC.Spotify/EC.Spotify/Services/`

## Output
- Updated service interfaces with method signatures and summaries
- Updated service implementations
- Updated `SpotifyClient.cs` service property registrations
- Updated `appsettings.json` with new OAuth scopes
- Removed obsolete methods and registrations

## Execution Steps

### Step 1: Read API Summary
1. Load `docs/spotify-api-methods-summary.md`
2. Parse all API methods from the markdown table
3. Extract: HTTP method, endpoint, description, deprecated status, required scopes
4. Create a list of all current API methods

### Step 2: Compare with Current Implementation
1. Scan all service interfaces in `Abstractions/Services/`
2. Scan all service implementations in `Services/`
3. Compare API methods against implemented methods
4. Identify:
   - **Missing methods**: API methods not yet implemented
   - **Obsolete methods**: Implemented methods no longer in API
   - **Changed methods**: Methods with different signatures or scopes
   - **Deprecated methods**: Methods marked as deprecated in API

### Step 3: Update Interfaces
For each service interface:
1. Add method signatures for missing API methods
2. Remove method signatures for obsolete API methods
3. Update XML documentation comments with method summaries from API
4. Add `[Obsolete]` attribute **ONLY** if API shows `Deprecated: True`
5. Document required scopes in method comments

### Step 4: Update Implementations
For each service implementation:
1. Implement missing methods according to API specification
2. Remove obsolete method implementations
3. Update method signatures to match interface changes
4. Mark deprecated methods with `[Obsolete]` attribute
5. Ensure all HTTP calls match API endpoints

### Step 5: Update Registrations
1. Update `SpotifyClient.cs` service property registrations
2. Remove obsolete service properties
3. Add new service properties for missing services
4. Update `SpotifyRegistration.cs` DI registrations

### Step 6: Update Configuration
1. Update `appsettings.json` in ApiExample project
2. Add new OAuth scopes required by new methods
3. Document which methods require each scope
4. Remove obsolete scopes if no longer needed

### Step 7: Validate Build
Run `dotnet build` to ensure:
- All interfaces match implementations
- All registrations are correct
- No compilation errors
- Application can start

## Critical Rules

### Rule 1: API Summary is Source of Truth
- **ALWAYS** use `docs/spotify-api-methods-summary.md` as authoritative reference
- **NEVER** rely on memory or assumptions about API methods
- **ALWAYS** verify against the markdown table before making changes

### Rule 2: Deprecation Accuracy
- **ONLY** mark a method as `[Obsolete]` if the API documentation table explicitly shows `Deprecated: True`
- **NEVER** mark methods as obsolete based on:
  - Assumptions about Spotify's intentions
  - Methods that haven't been called recently
  - Methods that seem unused
- **ALWAYS** verify the Deprecated column in the markdown table

### Rule 3: Methods That Are NOT Deprecated (Common Examples)
These endpoints are **NOT deprecated** and should **NEVER** be marked as obsolete:
- `GET /me/albums` - Get User's Saved Albums (Deprecated: False)
- `GET /me/tracks` - Get User's Saved Tracks (Deprecated: False)
- `GET /me/episodes` - Get User's Saved Episodes (Deprecated: False)
- `GET /me/shows` - Get User's Saved Shows (Deprecated: False)
- `GET /me/audiobooks` - Get User's Saved Audiobooks (Deprecated: False)
- `GET /me/library` - Check User's Saved Items (Deprecated: False)
- `PUT /me/library` - Save Items to Library (Deprecated: False)
- `DELETE /me/library` - Remove Items from Library (Deprecated: False)

### Rule 4: Methods That ARE Deprecated (Common Examples)
These endpoints **ARE deprecated** and **SHOULD** be marked as obsolete:
- `GET /albums` - Get Several Albums (Deprecated: True)
- `GET /artists` - Get Several Artists (Deprecated: True)
- `GET /me/albums/contains` - Check User's Saved Albums (Deprecated: True)
- `PUT /me/albums` - Save Albums for Current User (Deprecated: True)
- `DELETE /me/albums` - Remove Users' Saved Albums (Deprecated: True)
- `GET /audio-features` - Get Several Tracks' Audio Features (Deprecated: True)
- `GET /audio-features/{id}` - Get Track's Audio Features (Deprecated: True)

### Rule 5: Verification Process
Before marking any method as `[Obsolete]`:
1. Find the exact API endpoint in the markdown table
2. Check the `Deprecated` column
3. Only proceed if it shows `True`
4. Document the specific table row as evidence

### Rule 6: Preserve User Code
- **DO NOT** modify user-written code in example projects
- **DO** preserve custom business logic
- **DO** keep user comments and documentation
- **ASK** before making breaking changes to user code

## File Locations

### Input
- `docs/spotify-api-methods-summary.md` - API method reference
- `src/EC.Spotify/EC.Spotify/Abstractions/Services/*.cs` - Service interfaces
- `src/EC.Spotify/EC.Spotify/Services/*.cs` - Service implementations
- `src/EC.Spotify/EC.Spotify/SpotifyClient.cs` - Client registrations

### Output
- Updated service interfaces
- Updated service implementations
- Updated `SpotifyClient.cs`
- Updated `SpotifyRegistration.cs`
- Updated `appsettings.json`

## Validation Commands

```powershell
# Build validation
dotnet build src/EC.Spotify/EC.Spotify.slnx

# Check for obsolete attributes
Get-ChildItem src/EC.Spotify/EC.Spotify -Recurse -Filter "*.cs" | 
    Select-String "\[Obsolete" | 
    Select-Object Path, LineNumber

# Verify service registrations
Get-Content src/EC.Spotify/EC.Spotify/SpotifyClient.cs | Select-String "public.*Service"
```

## Example Execution

### Normal Update
```
Skill: spotify-update
Action: Updating library to match API documentation
Input: docs/spotify-api-methods-summary.md
Changes:
  - Added 3 new methods (GetAlbumAsync, GetArtistAsync, GetShowAsync)
  - Removed 2 obsolete methods (GetAlbumsAsync, GetArtistsAsync)
  - Marked 5 methods as [Obsolete] (deprecated in API)
  - Updated 12 OAuth scopes in appsettings.json
Validation: dotnet build SUCCESS
Status: SUCCESS
```

### No Changes Needed
```
Skill: spotify-update
Action: Checking library against API documentation
Input: docs/spotify-api-methods-summary.md
Changes: None (library already up to date)
Validation: dotnet build SUCCESS
Status: SKIPPED
```

### Deprecation Update
```
Skill: spotify-update
Action: Updating deprecation status
Changes:
  - Marked 8 methods as [Obsolete] (newly deprecated in API)
  - Updated XML documentation with deprecation notices
  - Added migration guidance in comments
Validation: dotnet build SUCCESS
Status: SUCCESS
```

## Dependencies
- **Runs after**: SpotifyDocsSkill (must have latest API documentation)
- **Runs before**: SpotifyWikiSkill, SpotifyExampleSkill, SpotifyTestSkill
- **Requires**: dotnet CLI, ability to parse markdown and C# code

## Troubleshooting

### Build Errors After Update
**Error**: "dotnet build failed with X errors"  
**Fix**: Check for:
- Missing method implementations
- Incorrect method signatures
- Missing using statements
- Incorrect dependency injection

### Obsolete Attribute Missing
**Error**: "Deprecated methods not marked as [Obsolete]"  
**Fix**: Verify Deprecated column in API summary - only mark if True

### Extra Methods Added
**Error**: "Methods added that don't exist in API"  
**Fix**: Double-check API summary markdown - only implement methods listed there

### Scope Mismatch
**Error**: "OAuth scopes don't match method requirements"  
**Fix**: Update appsettings.json with scopes from API documentation table
