---
name: spotify-docs
description: 'Use when: generating or updating Spotify API documentation; when asked to run /spotify docs; when API documentation needs refreshing; when docs/spotify-api-methods-summary.md is outdated'
user-invocable: true
---

# spotify-docs

## Purpose
Generate and maintain the Spotify API Method Summary Markdown file by fetching documentation from the Spotify Web API.

## When to Use
- User explicitly runs `/spotify docs`
- `docs/spotify-api-methods-summary.md` is older than 7 days
- Spotify Web API has been updated and documentation needs refreshing
- Before running SpotifyUpdateSkill to ensure latest API methods are known

## Input
- Spotify Web API public documentation (fetched from web)

## Output
- `docs/spotify-api-methods-summary.md` - Complete API method reference table

## Execution Steps

### Step 1: Check Current State
1. Check if `docs/spotify-api-methods-summary.md` exists
2. If exists, check file timestamp
3. If file is less than 7 days old, skip regeneration unless explicitly forced
4. Log current state and age

### Step 2: Run Generation Script
Execute the PowerShell script to fetch and parse Spotify API documentation:

```powershell
cd .github/scripts
.\spotify-api-methods-summary.ps1
```

### Step 3: Verify Output
1. Confirm `docs/spotify-api-methods-summary.md` was created/updated
2. Check file timestamp is current
3. Verify markdown structure contains:
   - Table headers (Method, Description, Deprecated, Permissions, Http Method, API URL)
   - At least one API method row
   - Generation timestamp
4. Report any issues if file is missing or malformed

### Step 4: Report Results
Provide summary of:
- Number of API methods found
- Number of deprecated methods
- New methods added since last run
- Methods removed since last run
- File location and timestamp

## Critical Rules

### Rule 1: Age Check
- **NEVER** regenerate if file is less than 7 days old
- **EXCEPTION**: Only regenerate if user explicitly requests with `--force` flag
- **REASON**: Avoid unnecessary API calls and rate limiting

### Rule 2: Script as Source of Truth
- **ALWAYS** use the PowerShell script to generate documentation
- **NEVER** manually edit `docs/spotify-api-methods-summary.md`
- **REASON**: Script ensures consistent format and accurate data extraction

### Rule 3: Validate Structure
After generation, verify:
- Table has correct column headers
- All API methods are captured
- Links to official documentation are present
- Deprecated status is correctly identified

### Rule 4: Handle Errors Gracefully
If script fails:
1. Log the error with full details
2. Check network connectivity
3. Verify Spotify API documentation URL is accessible
4. Suggest retrying or manual intervention

## File Locations

### Input
- `scripts/spotify-api-methods-summary.ps1` - Generation script

### Output
- `docs/spotify-api-methods-summary.md` - Generated API documentation

## Validation Commands

```powershell
# Check file age
Get-Item docs/spotify-api-methods-summary.md | Select-Object LastWriteTime

# Verify markdown structure
Get-Content docs/spotify-api-methods-summary.md | Select-String "^|" | Measure-Object

# Count API methods
(Get-Content docs/spotify-api-methods-summary.md | Select-String "^|").Count
```

## Example Execution

### Normal Run (file > 7 days old)
```
Skill: spotify-docs
Action: Regenerating API documentation
Script: .\scripts\spotify-api-methods-summary.ps1
Output: docs/spotify-api-methods-summary.md
Status: SUCCESS
Methods found: 156
Deprecated: 12
Age: 8 days (regenerated)
```

### Skipped Run (file < 7 days old)
```
Skill: spotify-docs
Action: Skipping regeneration
Reason: File is only 3 days old
Output: No changes
Status: SKIPPED
```

### Force Run
```
Skill: spotify-docs
Action: Force regenerating API documentation
Flag: --force
Script: .\scripts\spotify-api-methods-summary.ps1 --force
Output: docs/spotify-api-methods-summary.md
Status: SUCCESS (forced)
```

## Dependencies
- **Runs before**: SpotifyUpdateSkill, SpotifyWikiSkill, SpotifyExampleSkill, SpotifyTestSkill
- **Runs after**: None (first step in workflow)
- **Requires**: PowerShell 7+, internet access to Spotify API docs

## Troubleshooting

### Script Not Found
**Error**: "Cannot find path '.\scripts\spotify-api-methods-summary.ps1'"  
**Fix**: Ensure script exists in `scripts/` folder

### Network Error
**Error**: "Failed to fetch Spotify API documentation"  
**Fix**: Check internet connection and Spotify API documentation URL accessibility

### Parse Error
**Error**: "Failed to parse HTML documentation"  
**Fix**: Spotify may have changed their documentation structure - update regex patterns in script

### Permission Error
**Error**: "Execution policy prevents running script"  
**Fix**: Run `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`
