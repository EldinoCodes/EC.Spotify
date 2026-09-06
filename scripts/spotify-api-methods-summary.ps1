[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$OutputPath = "docs\spotify-api-methods-summary.md",

    [Parameter(Mandatory = $false)]
    [string]$SourceUrl = "https://developer.spotify.com/documentation/web-api",

    [Parameter(Mandatory = $false)]
    [string]$TestFilePath = $null
)

<#
.SYNOPSIS
    Methods for pulling specific chunks of HTML out of HtmlContent.html.
#>

$OutputPath = (Resolve-Path $OutputPath).Path
if ($TestFilePath) {
    $TestFilePath = (Resolve-Path $TestFilePath).Path
}

function Invoke-WebRequestSafely {
    <#
    .SYNOPSIS
        Fetches a URL with automatic retry logic for failed requests.
    .DESCRIPTION
        Attempts to retrieve content from a specified URL with built-in retry
        functionality. Retries up to MaxRetries times with exponential backoff
        between attempts, providing robustness against network instability.
    .PARAMETER Url
        The URL to fetch.
    .PARAMETER MaxRetries
        Maximum number of retry attempts. Defaults to 3.
    .EXAMPLE
        Invoke-WebRequestSafely -Url "https://example.com"
    #>
    param([string]$Url, [int]$MaxRetries = 3)

    for ($i = 1; $i -le $MaxRetries; $i++) {
        try {
            Write-host "Fetching $Url"

            $response = Invoke-WebRequest -Uri $Url -Headers $Headers -UseBasicParsing -ErrorAction Stop
            return $response
        }
        catch {
            Write-Warning "Attempt $i/$MaxRetries failed for $($_.Exception.Message) for $Url"
            if ($i -lt $MaxRetries) {
                Start-Sleep -Seconds (2 * $i)
            }
            else {
                throw $_
            }
        }
    }
}

function Get-AllMethodUrls {
    <#
    .SYNOPSIS
        Extracts all method documentation URLs from the navigation menu in the HTML file.
    .DESCRIPTION
        Parses the sidebar navigation to find all links to method documentation pages.
    .PARAMETER Html
        The HTML content to parse.
    #>
    param(
        [string]$Html
    )

    if (-not $Html) { return $null }
    
    # Match all navigation links to method documentation
    $pattern = 'href="(/documentation/web-api/reference/[^"]+)"'
    $tagMatches = [regex]::Matches($Html, $pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    
    $urls = @()
    foreach ($tagMatch in $tagMatches) {
        $url = $tagMatch.Groups[1].Value
        # Avoid duplicates and the base reference page
        if ($url -notmatch '/\[' -and $url -notmatch '/reference$' -and $url -notin $urls) {
            $urls += "https://developer.spotify.com$url"
        }
    }

    return $urls | Select-Object -Unique
}

function Get-MainHtml {
    <#
    .SYNOPSIS
        Returns the inner HTML of the first <main> tag found in the given HTML file.
    .PARAMETER Html
        The HTML content to parse.
    #>
    param(
        [string]$Html
    )

    if (-not $Html) { return $null }

    $pattern = '<main[^>]*>(.*?)</main>'
    $tagMatch = [regex]::Match($Html, $pattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)

    if ($tagMatch.Success) {
        return $tagMatch.Groups[1].Value
    }

    Write-Warning "No <main> tag found in the provided HTML content"
    return $null
}

function Get-TopLevelDivs {
    <#
    .SYNOPSIS
        Splits an HTML fragment into its direct-child <div> elements (including nested markup).
    .PARAMETER Html
        The HTML fragment to scan, e.g. the inner content of <main>.
    #>
    param(
        [string]$Html
    )

    if (-not $Html) { return $null }

    # Track open/close tag depth so nested divs don't break up a top-level element.
    $tagPattern = '<div[^>]*>|</div>'
    $tagMatches = [regex]::Matches($Html, $tagPattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)

    $divs = @()
    $depth = 0
    $startIndex = -1

    foreach ($tag in $tagMatches) {
        if ($tag.Value -match '^<div') {
            if ($depth -eq 0) { $startIndex = $tag.Index }
            $depth++
        }
        else {
            $depth--
            if ($depth -eq 0) {
                $endIndex = $tag.Index + $tag.Length
                $divs += $Html.Substring($startIndex, $endIndex - $startIndex)
            }
        }
    }

    return $divs
}

function Get-MainSections {
    <#
    .SYNOPSIS
        Returns the two top-level <div> children of <main>: method documentation and response example.
    .PARAMETER Path
        Path to the HTML file to parse. Defaults to HtmlContent.html next to this script.
    #>
    param(
        [string]$Html
    )

    if (-not $Html) { return $null }

    $divs = Get-TopLevelDivs -Html $Html
    if ($divs.Count -lt 2) {
        Write-Warning "Expected 2 top-level <div> children in <main>, found $($divs.Count)"
    }

    return [PSCustomObject]@{
        MethodDocumentation = $divs[0]
        ResponseSample = $divs[1]
    }
}

function Get-MethodDocumentationParts {
    <#
    .SYNOPSIS
        Splits the method-documentation div into Overview, Request, and Response sections.
    .DESCRIPTION
        The only reliable, consistent landmarks in this markup are the <h1> method name
        and the <h2>Request</h2> / <h2>Response</h2> section headings, so those are used
        as the split points instead of relying on div classes (which change per method).
    .PARAMETER Html
        The method-documentation HTML fragment, e.g. from Get-MainSections.MethodDocumentation.
    #>
    param(
        [string]$Html
    )

    if (-not $Html) {
        Write-Warning 'Request HTML is null or empty.'
        return [PSCustomObject]@{
            Overview = $null
            Request  = $null
            Response = $null
        }
    }

    $requestMatch = [regex]::Match($Html, '<h2[^>]*>\s*Request\s*</h2>', [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    $responseMatch = [regex]::Match($Html, '<h2[^>]*>\s*Response\s*</h2>', [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)

    if (-not $requestMatch.Success -or -not $responseMatch.Success) {
        Write-Warning 'Could not locate the Request and/or Response section headings.'
        return [PSCustomObject]@{
            Overview = $Html
            Request  = $null
            Response = $null
        }
    }

    return [PSCustomObject]@{
        Overview = $Html.Substring(0, $requestMatch.Index)
        Request  = $Html.Substring($requestMatch.Index, $responseMatch.Index - $requestMatch.Index)
        Response = $Html.Substring($responseMatch.Index)
    }
}

function Get-HeadingMethodDetails {
    <#
    .SYNOPSIS
        Extracts the method name from the heading structure in the HTML.
    .DESCRIPTION
        Parses the heading structure containing SVG icon, "Web API" label,
        and navigation breadcrumbs to extract the topic/method name.
    .PARAMETER Html
        The HTML fragment containing the heading structure.
    .EXAMPLE
        Get-HeadingMethodDetails -Html $htmlContent
    #>
    param(
        [string]$Html
    )

    # Pattern to match the heading structure: svg, span with "Web API", span with "References / Topic / Method"
    # More flexible pattern to handle extra content between elements
    $pattern = '<svg[^>]*>.*?</svg>\s*<span[^>]*>.*?Web Api.*?</span>\s*<span[^>]*>.*?(?:References|Reference)\s*/\s*([^/]+)\s*/\s*[^<]+'
    
    $match = [regex]::Match($Html, $pattern, [System.Text.RegularExpressions.RegexOptions]::Singleline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    
    if ($match.Success) {
        return $match.Groups[1].Value.Trim()
    }
    
    Write-Warning "Could not parse heading structure from HTML content"
    return $null
}

function Get-OverviewMethodDetails {
    <#
    .SYNOPSIS
        Extracts the method name, description, deprecated status, and authorization scopes from the overview section.
    .PARAMETER Html
        The overview section HTML fragment, e.g. from Get-MethodDocumentationParts.Overview.
    #>
    param(
        [string]$Html
    )

    # Extract the method name from the <h1> tag
    $h1Match = [regex]::Match($Html, '<h1[^>]*>([^<]+)</h1>', [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    
    if (-not $h1Match.Success) {
        Write-Warning 'Could not locate the method name in the <h1> tag.'
        return [PSCustomObject]@{
            MethodName = $null
            Description = $null
            IsDeprecated = $false
            AuthorizationScopes = @()
        }
    }

    $methodName = $h1Match.Groups[1].Value.Trim()

    # Extract the description that follows the <h1> tag.
    $afterH1 = $Html.Substring($h1Match.Index + $h1Match.Length)
    
    # Match text content in <p>, <span>, or text nodes, excluding tags and script/style.
    $descriptionPattern = '(?:>([^<]+)<)|(?:</h1>([^<]+)<)'
    $descMatches = [regex]::Matches($afterH1, $descriptionPattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    
    $description = $null
    foreach ($match in $descMatches) {
        $text = if ($match.Groups[1].Success) { $match.Groups[1].Value } else { $match.Groups[2].Value }
        $text = $text.Trim()
        
        if ($text -and $text.Length -gt 20) {
            $description = $text
            break
        }
    }

    # Check for deprecated marker - looks for <span>Deprecated</span> in a tag element
    $deprecatedPattern = '<span[^>]*>\s*Deprecated\s*</span>'
    $deprecatedMatch = [regex]::Match($Html, $deprecatedPattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    $isDeprecated = $deprecatedMatch.Success

    # Extract authorization scopes from the <ul> list
    # Each scope is in a <span> tag inside a <button> within a <li> inside the <ul>
    $scopes = @()
    if ($Html -match 'Authorization scopes') {
        # Find the scopes section
        $scopesSectionStart = $Html.IndexOf('Authorization scopes')
        if ($scopesSectionStart -ge 0) {
            $scopesSection = $Html.Substring($scopesSectionStart)
            
            # Match <button> tags and extract the first <span> text inside them
            # This avoids matching the "Read more" link that appears after the button
            $buttonMatches = [regex]::Matches($scopesSection, '<button[^>]*>(.*?)</button>', [System.Text.RegularExpressions.RegexOptions]::Singleline)
            foreach ($match in $buttonMatches) {
                $buttonContent = $match.Groups[1].Value
                
                # Extract the first <span> text from the button content
                $spanMatch = [regex]::Match($buttonContent, '<span[^>]*>([^<]+)</span>')
                if ($spanMatch.Success) {
                    $scopeName = $spanMatch.Groups[1].Value.Trim()
                    # Filter out non-scope buttons (like the OAuth 2.0 button)
                    if ($scopeName -match '^[a-zA-Z0-9_-]+$' -and $scopeName -notmatch 'OAuth') {
                        $scopes += $scopeName
                    }
                }
            }
        }
    }

    return [PSCustomObject]@{
        MethodName = $methodName
        Description = $description
        IsDeprecated = $isDeprecated
        AuthorizationScopes = $scopes
    }
}

function Get-RequestMethodDetails {
    <#
    .SYNOPSIS
        Extracts the HTTP method verb (GET, POST, PUT, DELETE, etc.) and URL path from the Request section.
    .DESCRIPTION
        Parses the Request section to find the HTTP verb and API endpoint URL.
        Handles two different HTML structures:
        Type 1: <button> with <span> elements for HTTP method and URL
        Type 2: <div> with <span> elements for HTTP method and URL
    .PARAMETER Html
        The Request section HTML fragment, e.g. from Get-MethodDocumentationParts.Request.
    #>
    param(
        [string]$Html
    )

    if (-not $Html) {
        Write-Warning 'Request HTML is null or empty.'
        return [PSCustomObject]@{
            HttpMethod = $null
            ApiUrl = $null
        }
    }

    # Try Type 1: Look for <button> tags containing HTTP verb and URL spans
    $buttonPattern = '<button[^>]*>(.*?)</button>'
    $buttonMatches = [regex]::Matches($Html, $buttonPattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)
    
    foreach ($match in $buttonMatches) {
        $buttonContent = $match.Groups[1].Value
        
        # Look for span tags that contain the HTTP verb (uppercase letters)
        $spanMatches = [regex]::Matches($buttonContent, '<span[^>]*>([^<]+)</span>')
        
        if ($spanMatches.Count -ge 2) {
            $firstSpan = $spanMatches[0].Groups[1].Value.Trim()
            $secondSpan = $spanMatches[1].Groups[1].Value.Trim()
            
            # First span should be the HTTP verb (all uppercase)
            if ($firstSpan -match '^(GET|POST|PUT|DELETE|PATCH|HEAD|OPTIONS)$') {
                return [PSCustomObject]@{
                    HttpMethod = $firstSpan
                    ApiUrl = $secondSpan
                }
            }
        }
    }

    # Try Type 2: Look for <div> tags containing HTTP method and URL spans
    # This handles newer pages that don't use <button> wrapper
    $divPattern = '<div[^>]*>(.*?)</div>'
    $divMatches = [regex]::Matches($Html, $divPattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)
    
    foreach ($match in $divMatches) {
        $divContent = $match.Groups[1].Value
        
        # Look for span tags that contain the HTTP verb (uppercase letters)
        $spanMatches = [regex]::Matches($divContent, '<span[^>]*>([^<]+)</span>')
        
        if ($spanMatches.Count -ge 2) {
            $firstSpan = $spanMatches[0].Groups[1].Value.Trim()
            $secondSpan = $spanMatches[1].Groups[1].Value.Trim()
            
            # First span should be the HTTP verb (all uppercase)
            if ($firstSpan -match '^(GET|POST|PUT|DELETE|PATCH|HEAD|OPTIONS)$') {
                return [PSCustomObject]@{
                    HttpMethod = $firstSpan -replace "`n", ""
                    ApiUrl = $secondSpan -replace "`n", ""
                }
            }
        }
    }

    Write-Warning 'Could not locate HTTP method and URL in the Request section.'
    return [PSCustomObject]@{
        HttpMethod = $null
        ApiUrl = $null
    }
}

function Get-MethodUrlFromPath {
    <#
    .SYNOPSIS
        Extracts the method name and URL from a navigation link in the HTML.
    .PARAMETER Html
        The HTML fragment containing the navigation link.
    #>
    param(
        [string]$Html
    )

    $pattern = 'href="(/documentation/web-api/reference/[^"]+)"[^>]*>\s*<span[^>]*>([^<]+)</span>'
    $tagMatches = [regex]::Matches($Html, $pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    
    $methods = @()
    foreach ($tagMatch in $tagMatches) {
        $methods += [PSCustomObject]@{
            Url = $tagMatch.Groups[1].Value -replace "`n", ""
            Name = $tagMatch.Groups[2].Value.Trim() -replace "`n", ""
        }
    }

    return $methods
}

function Get-SpotifyApiMethodDetailsAll {
    <#
    .SYNOPSIS
        Retrieves all Spotify Web API method details by iterating through all method URLs.
    .DESCRIPTION
        Fetches the main Spotify Web API documentation page, extracts all individual
        method URLs, and processes each one to gather comprehensive method information
        including names, descriptions, HTTP methods, and authorization scopes.
    .PARAMETER SpotifyWebApiUri
        The URI of the Spotify Web API documentation page to parse.
    .EXAMPLE
        Get-SpotifyApiMethodDetailsAll -SpotifyWebApiUri "https://developer.spotify.com/documentation/web-api"
    #>
    param(
        [string]$SpotifyWebApiUri
    )

    $response = Invoke-WebRequestSafely -Url $SpotifyWebApiUri
    if (-not $response) { return }

    $spotifyApiMethodDetails = Select-SpotifyApiMethodDetailsAll -Html $response.Content

    return $spotifyApiMethodDetails
}

function Select-SpotifyApiMethodDetailsAll {
    <#
    .SYNOPSIS
        Retrieves all Spotify Web API method details from HTML content.
    .DESCRIPTION
        Iterates through all method URLs extracted from the main documentation page,
        fetches each method's HTML content, and collects comprehensive method information
        including names, descriptions, HTTP methods, and authorization scopes.
    .PARAMETER Html
        The HTML content of the main Spotify Web API documentation page.
    .EXAMPLE
        Select-SpotifyApiMethodDetailsAll -Html $htmlContent
    #>
    param(
        [string]$Html
    )

    if (-not $Html) { return $null }

    $spotifyApiMethodDetails = @()

    # When run directly, generate the markdown documentation
    Get-AllMethodUrls -Html $Html | ForEach-Object {
        $methodUrl = $_
        Write-Host "Processing method URL: $methodUrl"
        
        # Fetch the HTML content for the method documentation page
        $response = Invoke-WebRequestSafely -Url $methodUrl
        if ($response) {
            $htmlContent = $response.Content
            $spotifyApiMethodDetail = Select-SpotifyApiMethodDetails -Html $htmlContent
            if ($spotifyApiMethodDetail) {
                Add-Member -InputObject $spotifyApiMethodDetail -MemberType NoteProperty -Name "DocUrl" -Value $methodUrl
                $spotifyApiMethodDetails += $spotifyApiMethodDetail
            }
        }
        Start-Sleep -Seconds 1
    }

    return $spotifyApiMethodDetails
}

function Select-SpotifyApiMethodDetails {
    <#
    .SYNOPSIS
        Extracts detailed information from a single Spotify Web API method documentation page.
    .DESCRIPTION
        Parses the HTML content of an individual method documentation page to extract
        the method name, description, deprecated status, authorization scopes, HTTP method,
        and API URL. Returns a PSCustomObject containing all extracted details.
    .PARAMETER Html
        The HTML content of a single method documentation page.
    .EXAMPLE
        Select-SpotifyApiMethodDetails -Html $htmlContent
    #>
    param(
        [string]$Html
    )
    if (-not $Html) { return $null }

    $main = Get-MainHtml -Html $Html
    $sections = Get-MainSections -Html $main
    if (-not $sections) { return $null }

    $parts = Get-MethodDocumentationParts -Html $sections.MethodDocumentation
    if (-not $parts) { return $null }

    $heading = Get-HeadingMethodDetails -Html $main
    $overviewMethodDetails = Get-OverviewMethodDetails -Html $parts.Overview
    $requestMethodDetails = Get-RequestMethodDetails -Html $parts.Request
    $spotifyApiMethodDetail = [PSCustomObject]@{
        Topic = $heading -replace "`r`n", ""
        MethodName = $overviewMethodDetails.MethodName -replace "`n", ""
        Description = $overviewMethodDetails.Description -replace "`n", ""
        IsDeprecated = $overviewMethodDetails.IsDeprecated -replace "`n", ""
        AuthorizationScopes = @($overviewMethodDetails.AuthorizationScopes)
        HttpMethod = $requestMethodDetails.HttpMethod -replace "`n", ""
        ApiUrl = $requestMethodDetails.ApiUrl -replace "`n", ""
    }
    return $spotifyApiMethodDetail    
}

function Get-SpotifyApiMethodDetailsTest {
    <#
    .SYNOPSIS
        Tests the Spotify Web API method extraction process using a local HTML file.
    .DESCRIPTION
        Reads the local HtmlContent.html file, extracts all method URLs, and processes
        a single method to verify the extraction pipeline works correctly. Returns
        an array of method details for testing purposes.
    .EXAMPLE
        Get-SpotifyApiMethodDetailsTest
    #>
    param(
        [string]$TestFilePath
    )
    if (-not $TestFilePath) { return $null }
    
    $Html = Get-content -Path $TestFilePath -Raw

    $spotifyApiMethodDetails = @()

    if (-not $Html) { return $spotifyApiMethodDetails }

    $spotifyApiMethodDetail = Select-SpotifyApiMethodDetails -Html $Html
    if ($spotifyApiMethodDetail) { 
        $spotifyApiMethodDetails += $spotifyApiMethodDetail
    }

    return $spotifyApiMethodDetails
}

function Get-SpotifyApiMethodDetailMarkdown {
    <#
    .SYNOPSIS
        Generates a markdown file with all Spotify Web API method documentation details.
    .DESCRIPTION
        Converts an array of Spotify API method detail objects into a well-formatted
        markdown table containing method names, descriptions, deprecation status,
        authorization scopes, HTTP methods, and API URLs. Methods are grouped by topic
        with each topic having its own table and header, and topics are sequenced alphabetically.
    .PARAMETER SpotifyApiMethodDetails
        An array of Spotify API method details objects to convert into a markdown table.
    .EXAMPLE
        Get-SpotifyApiMethodDetailMarkdown -SpotifyApiMethodDetails $methods
    #>
    param(
        [PSCustomObject[]]$SpotifyApiMethodDetails
    )

    if (-not $SpotifyApiMethodDetails) { return $null }

    $markdown = ""
    # Add header with current date
    $markdown += "# Spotify Web API Methods Summary`n"
    $markdown += "**Generated on: $(Get-Date -Format 'yyyy-MM-dd')**`n`n"
    
    # Group methods by topic and sort topics alphabetically
    $groupedMethods = $SpotifyApiMethodDetails | Group-Object -Property Topic
    $sortedTopics = $groupedMethods | Sort-Object -Property Name

    foreach ($topicGroup in $sortedTopics) {
        $topicName = $topicGroup.Name
        
        # Add topic header
        $markdown += "## $topicName`n"
        $markdown += "> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.`n`n"
        
        # Create table header for this topic
        $markdownTable = ""
        $markdownTable += "| Method | Description | Deprecated | Permissions | Http Method | API URL |`n"
        $markdownTable += "|--------|-------------|------------|-------------|-------------|---------|`n"

        foreach ($detail in $topicGroup.Group) {
            $row = "| [$($detail.MethodName)]($($detail.DocUrl)) | $($detail.Description) | $($detail.IsDeprecated) | $($detail.AuthorizationScopes -join ', ') | $($detail.HttpMethod) | $($detail.ApiUrl) |`n"
            $row = $row -replace '&#x27;', "'"

            $markdownTable += $row
        }

        # Add table and spacing
        $markdown += $markdownTable
        $markdown += "`n`n"
    }

    # Add footer
    $markdown += "***`n"
    $markdown += "***`n"
    $markdown += "*Generated $($SpotifyApiMethodDetails.Count) Spotify API method details*`n"
    $markdown += "*Source: [Spotify Web API Documentation](https://developer.spotify.com/documentation/web-api)*`n"

    return $markdown
}

if (-not $TestFilePath) {
    Write-Host "Fetching all Spotify Web API method details from $SourceUrl"
}
else {
    Write-Host "Testing with local HTML file: $TestFilePath"
}

$spotifyApiMethodDetails = if ($TestFilePath) {
    Get-SpotifyApiMethodDetailsTest -TestFilePath $TestFilePath
} else {
    Get-SpotifyApiMethodDetailsAll -SpotifyWebApiUri $SourceUrl
}

$markdown = Get-SpotifyApiMethodDetailMarkdown -SpotifyApiMethodDetails $spotifyApiMethodDetails

$fullOutputPath = [System.IO.Path]::GetFullPath($OutputPath)
$dir = Split-Path $fullOutputPath -Parent
Write-Host "Ensuring output directory exists: $dir" -ForegroundColor Yellow
if (-not (Test-Path $dir)) {
    New-Item -ItemType Directory -Path $dir -Force | Out-Null
}

Set-Content -Path $fullOutputPath -Value $markdown -Force
Write-Host "Markdown written to: $fullOutputPath" -ForegroundColor Green