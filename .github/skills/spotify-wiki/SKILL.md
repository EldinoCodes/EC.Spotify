---
name: spotify-wiki
description: 'Use when: generating or updating EC.Spotify Wiki documentation; when asked to run /spotify wiki; after library code changes; after running SpotifyUpdateSkill'
user-invocable: true
---

# spotify-wiki

## Purpose
Generate and maintain comprehensive Wiki documentation for the EC.Spotify library by analyzing source code, interfaces, implementations, and documentation comments.

## When to Use
- User explicitly runs `/spotify wiki`
- After running SpotifyUpdateSkill to reflect library changes
- When Wiki documentation is outdated compared to source code
- When new services or methods have been added
- Before running SpotifyExampleSkill or SpotifyTestSkill

## Input
- Service interfaces in `src/EC.Spotify/EC.Spotify/Abstractions/Services/`
- Service implementations in `src/EC.Spotify/EC.Spotify/Services/`
- XML documentation comments from interfaces and classes
- Model classes in `src/EC.Spotify/EC.Spotify/Models/`
- Enum definitions in `src/EC.Spotify/EC.Spotify/Enums/`

## Output
- `.github/wiki/README.md` - Wiki home page
- `.github/wiki/Services/{ServiceName}.md` - Service reference pages
- `.github/wiki/Models/{ModelName}.md` - Model reference pages
- `.github/wiki/Enums/{EnumName}.md` - Enum reference pages
- `.github/wiki/Configuration.md` - Configuration guide
- `.github/wiki/Examples.md` - Usage examples

## Execution Steps

### Step 1: Analyze Library Structure
1. Scan all service interfaces in `Abstractions/Services/`
2. Identify all public methods, parameters, return types
3. Extract XML documentation comments
4. Map service dependencies and provider relationships
5. List all models and enums used by services

### Step 2: Generate Service Reference Pages
For each service interface:
1. Create/update `.github/wiki/Services/{ServiceName}.md`
2. Include:
   - Service overview and purpose
   - Interface definition with all methods
   - Method documentation (from XML comments)
   - Parameters and return types
   - Required OAuth scopes
   - Example usage code
3. Format consistently with other service pages

### Step 3: Generate Model Reference Pages
For each model class:
1. Create/update `.github/wiki/Models/{ModelName}.md`
2. Include:
   - Model description and purpose
   - Property list with types and descriptions
   - JSON serialization examples
   - Usage context (which services use this model)
3. Group related models together if appropriate

### Step 4: Generate Enum Reference Pages
For each enum:
1. Create/update `.github/wiki/Enums/{EnumName}.md`
2. Include:
   - Enum description and purpose
   - All enum values with descriptions
   - Default values and usage examples
   - Which methods use this enum

### Step 5: Generate Configuration Guide
Create/update `.github/wiki/Configuration.md`:
1. SpotifyOptions properties and descriptions
2. Service registration process
3. OAuth scope requirements
4. Authentication flow
5. Error handling configuration

### Step 6: Generate Examples Page
Create/update `.github/wiki/Examples.md`:
1. Basic usage examples for each service
2. Advanced usage patterns
3. Error handling examples
4. Common scenarios and solutions
5. Link to ApiExample project for complete code

### Step 7: Update Wiki Home Page
Update `.github/wiki/README.md`:
1. Quick start guide
2. Links to all service reference pages
3. Links to model and enum references
4. Configuration overview
5. Example code links
6. Last updated timestamp

### Step 8: Validate Wiki Structure
1. Verify all service pages exist
2. Check that all methods are documented
3. Ensure links between pages work
4. Confirm consistent formatting
5. Report any missing documentation

## Critical Rules

### Rule 1: Source Code is Source of Truth
- **ALWAYS** generate Wiki from current source code
- **NEVER** manually edit Wiki pages to add methods
- **ALWAYS** verify against actual interface definitions
- **REASON**: Ensures Wiki stays synchronized with code

### Rule 2: Extract XML Documentation
- **ALWAYS** use XML documentation comments from source code
- **NEVER** write documentation from memory or assumptions
- **ALWAYS** preserve original documentation intent
- **DO** enhance with examples if documentation is sparse

### Rule 3: Consistent Formatting
- **USE** consistent markdown structure across all pages
- **FOLLOW** the template format for each page type
- **INCLUDE** code blocks for all C# examples
- **USE** proper table formatting for properties and parameters

### Rule 4: Document All Public API
- **INCLUDE** all public methods in service pages
- **INCLUDE** all public properties in model pages
- **INCLUDE** all enum values in enum pages
- **DO NOT** omit methods even if they seem trivial

### Rule 5: Preserve Deprecation Notices
- **INCLUDE** `[Obsolete]` attributes in documentation
- **EXPLAIN** why a method is deprecated
- **PROVIDE** migration path to alternative methods
- **HIGHLIGHT** deprecated methods visually

## File Locations

### Input
- `src/EC.Spotify/EC.Spotify/Abstractions/Services/*.cs` - Service interfaces
- `src/EC.Spotify/EC.Spotify/Services/*.cs` - Service implementations
- `src/EC.Spotify/EC.Spotify/Models/**/*.cs` - Model classes
- `src/EC.Spotify/EC.Spotify/Enums/*.cs` - Enum definitions
- `src/EC.Spotify/EC.Spotify/SpotifyOptions.cs` - Configuration

### Output
- `.github/wiki/README.md` - Wiki home page
- `.github/wiki/Services/*.md` - Service reference pages
- `.github/wiki/Models/*.md` - Model reference pages
- `.github/wiki/Enums/*.md` - Enum reference pages
- `.github/wiki/Configuration.md` - Configuration guide
- `.github/wiki/Examples.md` - Usage examples

## Wiki Page Template

```markdown
# {ServiceName} Reference

## Overview
{Brief description of the service and its purpose}

## Interface
```csharp
public interface I{ServiceName}Service
{
    // Methods...
}
```

## Methods

### {MethodName}
{XML documentation summary}

**Parameters:**
- {paramName}: {description} ({type})

**Returns:**
- {returnType}: {description}

**Required Scopes:**
- {scope1}
- {scope2}

**Example:**
```csharp
var result = await spotifyClient.{ServiceName}.{MethodName}(
    parameter1,
    parameter2,
    cancellationToken
);
```

## Related Models
- {ModelName1}
- {ModelName2}
```

## Validation Commands

```powershell
# Count service pages
Get-ChildItem .github/wiki/Services/*.md | Measure-Object

# Verify all services documented
$services = Get-ChildItem src/EC.Spotify/EC.Spotify/Abstractions/Services/*.cs | Select-Object -ExpandProperty BaseName
$wikiServices = Get-ChildItem .github/wiki/Services/*.md | Select-Object -ExpandProperty BaseName
$missing = $services | Where-Object { $_ -notin $wikiServices }
$missing

# Check for broken links
Get-ChildItem .github/wiki -Recurse -Filter "*.md" | 
    Select-String "\[.*\]\(.*\)" | 
    Select-Object -Unique
```

## Example Execution

### Full Wiki Generation
```
Skill: spotify-wiki
Action: Generating Wiki documentation from source code
Input: All service interfaces, implementations, models, enums
Changes:
  - Updated 10 service reference pages
  - Added 1 new service page (AudiobookService)
  - Updated 25 model reference pages
  - Updated 6 enum reference pages
  - Refreshed examples and configuration guide
Validation: All pages exist and are properly formatted
Status: SUCCESS
```

### Incremental Update
```
Skill: spotify-wiki
Action: Updating Wiki for recent changes
Changes:
  - Updated 3 service pages (new methods added)
  - Updated 5 model pages (new properties)
  - Updated examples page (new code samples)
Validation: Changes verified against source
Status: SUCCESS
```

### No Changes Needed
```
Skill: spotify-wiki
Action: Checking Wiki against source code
Changes: None (Wiki already up to date)
Validation: All pages match source code
Status: SKIPPED
```

## Dependencies
- **Runs after**: SpotifyUpdateSkill (must reflect current library state)
- **Runs before**: SpotifyExampleSkill, SpotifyTestSkill
- **Requires**: Ability to parse C# code and extract XML documentation

## Troubleshooting

### Missing Service Pages
**Error**: "Service page not found for {ServiceName}"  
**Fix**: Ensure interface exists in `Abstractions/Services/` and has XML documentation

### Incomplete Method Documentation
**Error**: "Method missing parameters or return type"  
**Fix**: Verify XML documentation comments exist in source code

### Broken Links
**Error**: "Link to model page not found"  
**Fix**: Ensure model page exists and link path is correct

### Formatting Inconsistencies
**Error**: "Wiki pages have different formatting"  
**Fix**: Apply consistent template to all pages

### Missing OAuth Scopes
**Error**: "Required scopes not documented"  
**Fix**: Extract scopes from method comments or appsettings.json
