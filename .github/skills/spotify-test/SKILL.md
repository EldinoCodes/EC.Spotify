---
name: spotify-test
description: 'Use when: creating or updating unit and integration tests; when asked to run /spotify tests; after library code changes; after running SpotifyUpdateSkill'
user-invocable: true
---

# spotify-test

## Purpose
Create and maintain unit tests and integration tests for the EC.Spotify library to ensure code quality and catch regressions.

## When to Use
- User explicitly runs `/spotify tests`
- After running SpotifyUpdateSkill to test new/changed methods
- When test coverage is below threshold
- When new services or methods have been added
- Before releasing a new version

## Input
- Service implementations in `src/EC.Spotify/EC.Spotify/Services/`
- Service interfaces in `src/EC.Spotify/EC.Spotify/Abstractions/Services/`
- Existing unit tests in `src/EC.Spotify.UnitTests/Services/`
- Existing integration tests in `src/EC.Spotify.Tests/Services/`
- Mock implementations in `src/EC.Spotify.UnitTests/Mocks/` and `src/EC.Spotify.Tests/Mocks/`

## Output
- Updated unit tests in `src/EC.Spotify.UnitTests/Services/`
- Updated integration tests in `src/EC.Spotify.Tests/Services/`
- Updated mock implementations
- Updated test data and fixtures
- Test coverage report

## Execution Steps

### Step 1: Analyze Service Implementations
1. Scan all service implementations in `Services/`
2. Identify all public methods
3. Note dependencies on other services/providers
4. Extract method signatures and return types
5. Identify business logic that needs testing

### Step 2: Update Unit Tests
For each service:
1. Check if corresponding test file exists in `UnitTests/Services/`
2. If exists, compare methods against implementation
3. If missing, create new test file
4. For each public method:
   - Create unit test method
   - Mock dependencies using Moq
   - Test happy path (expected success)
   - Test error paths (exceptions, null inputs)
   - Test edge cases (empty collections, boundary values)
   - Use xUnit `[Fact]` or `[Theory]` attributes
   - Follow Arrange-Act-Assert pattern

### Step 3: Update Integration Tests
For each service:
1. Check if corresponding test file exists in `Tests/Services/`
2. If exists, compare methods against implementation
3. If missing, create new test file
4. For each public method:
   - Create integration test method
   - Use mocked HTTP provider
   - Test end-to-end API interactions
   - Validate request/response formats
   - Test OAuth scope requirements
   - Use xUnit `[Fact]` or `[Theory]` attributes

### Step 4: Update Mock Implementations
1. Check if mocks match current interfaces
2. Update mock return types and signatures
3. Add mocks for new dependencies
4. Remove mocks for obsolete dependencies
5. Ensure mocks simulate realistic API behavior

### Step 5: Update Test Data
1. Update test fixtures with current data structures
2. Add test data for new models/enums
3. Remove obsolete test data
4. Ensure test data is valid and representative

### Step 6: Run Tests and Report Coverage
1. Run `dotnet test` on both test projects
2. Verify all tests pass
3. Generate coverage report
4. Report any failures with details
5. Identify areas with low coverage

## Critical Rules

### Rule 1: Test All Public Methods
- **ALWAYS** create tests for all public methods
- **NEVER** omit methods even if they seem trivial
- **ALWAYS** test both success and error paths
- **REASON**: Complete test coverage prevents regressions

### Rule 2: Use Mocking Framework
- **USE** Moq for mocking dependencies
- **MOCK** all external dependencies (HTTP, database, etc.)
- **DO NOT** make real API calls in unit tests
- **ISOLATE** the code being tested from external systems

### Rule 3: Follow Testing Patterns
- **ARRANGE**: Set up test data and mocks
- **ACT**: Execute the method being tested
- **ASSERT**: Verify expected outcomes
- **NAME** tests clearly: `MethodName_Scenario_ExpectedResult`
- **EXAMPLE**: `GetAsync_ValidId_ReturnsAlbum`

### Rule 4: Test Error Conditions
- **TEST** null/empty inputs
- **TEST** invalid IDs/formats
- **TEST** exception scenarios
- **TEST** timeout conditions
- **TEST** rate limiting scenarios

### Rule 5: Maintain Test Data
- **USE** realistic test data values
- **UPDATE** test data when models change
- **REMOVE** obsolete test data
- **DOCUMENT** test data sources in comments

### Rule 6: Preserve User Tests
- **DO NOT** remove user-written test cases
- **DO** preserve custom test scenarios
- **DO** keep user test helpers and utilities
- **MERGE** new tests with existing user tests

## File Locations

### Input
- `src/EC.Spotify/EC.Spotify/Services/*.cs` - Service implementations
- `src/EC.Spotify.UnitTests/Services/*.cs` - Existing unit tests
- `src/EC.Spotify.Tests/Services/*.cs` - Existing integration tests
- `src/EC.Spotify.UnitTests/Mocks/*.cs` - Unit test mocks
- `src/EC.Spotify.Tests/Mocks/*.cs` - Integration test mocks

### Output
- `src/EC.Spotify.UnitTests/Services/{Service}Tests.cs` - Unit test files
- `src/EC.Spotify.Tests/Services/{Service}Tests.cs` - Integration test files
- `src/EC.Spotify.UnitTests/Mocks/{Service}Mock.cs` - Unit test mocks
- `src/EC.Spotify.Tests/Mocks/{Service}Mock.cs` - Integration test mocks

## Unit Test Template

```csharp
using EC.Spotify.Abstractions;
using EC.Spotify.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EC.Spotify.UnitTests.Services;

public class AlbumServiceTests
{
    private readonly Mock<ISpotifyHttpProvider> _httpProviderMock;
    private readonly Mock<ILogger<AlbumService>> _loggerMock;
    private readonly IAlbumService _albumService;

    public AlbumServiceTests()
    {
        _httpProviderMock = new Mock<ISpotifyHttpProvider>();
        _loggerMock = new Mock<ILogger<AlbumService>>();
        
        var jsonProvider = new SpotifyJsonProvider();
        _albumService = new AlbumService(_httpProviderMock.Object, _loggerMock.Object, jsonProvider);
    }

    [Fact]
    public async Task GetAsync_ValidId_ReturnsAlbum()
    {
        // Arrange
        var albumId = "7a7arAXDE0BiaMgHLhdjGF";
        var mockAlbum = new Album { Id = albumId, Name = "Test Album" };
        
        _httpProviderMock
            .Setup(p => p.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JsonDocument.Parse(JsonSerializer.Serialize(mockAlbum)));
        
        // Act
        var result = await _albumService.GetAsync(albumId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(albumId, result.Id);
        Assert.Equal("Test Album", result.Name);
        _httpProviderMock.Verify(p => p.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_InvalidId_ThrowsException()
    {
        // Arrange
        var invalidId = "invalid-id";
        
        _httpProviderMock
            .Setup(p => p.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Invalid ID"));
        
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _albumService.GetAsync(invalidId));
    }

    [Fact]
    public async Task GetAsync_NullId_ThrowsArgumentNullException()
    {
        // Arrange
        string? nullId = null;
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _albumService.GetAsync(nullId!));
    }
}
```

## Integration Test Template

```csharp
using EC.Spotify.Abstractions;
using EC.Spotify.Providers;
using EC.Spotify.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EC.Spotify.Tests.Services;

public class AlbumServiceIntegrationTests
{
    private readonly AlbumService _albumService;
    private readonly MockHttpProvider _mockHttpProvider;

    public AlbumServiceIntegrationTests()
    {
        _mockHttpProvider = new MockHttpProvider();
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        
        var jsonProvider = new SpotifyJsonProvider();
        _albumService = new AlbumService(_mockHttpProvider, loggerFactory.CreateLogger<AlbumService>(), jsonProvider);
    }

    [Fact]
    public async Task GetAsync_RealisticResponse_ReturnsValidAlbum()
    {
        // Arrange
        var albumId = "7a7arAXDE0BiaMgHLhdjGF";
        var mockResponse = new
        {
            id = albumId,
            name = "Test Album",
            album_type = "album",
            total_tracks = 12,
            images = new[] { new { url = "https://example.com/image.jpg" } }
        };
        
        _mockHttpProvider.AddResponse(
            $"https://api.spotify.com/v1/albums/{albumId}",
            System.Net.HttpStatusCode.OK,
            mockResponse
        );
        
        // Act
        var result = await _albumService.GetAsync(albumId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(albumId, result.Id);
        Assert.Equal("Test Album", result.Name);
        Assert.Equal(12, result.TotalTracks);
    }

    [Fact]
    public async Task GetAsync_ApiError_ThrowsSpotifyException()
    {
        // Arrange
        var albumId = "nonexistent";
        
        _mockHttpProvider.AddResponse(
            $"https://api.spotify.com/v1/albums/{albumId}",
            System.Net.HttpStatusCode.NotFound,
            new { error = new { message = "Album not found", status = 404 } }
        );
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<SpotifyException>(() => _albumService.GetAsync(albumId));
        Assert.Equal(404, exception.StatusCode);
    }
}
```

## Validation Commands

```powershell
# Run all unit tests
dotnet test src/EC.Spotify/EC.Spotify.UnitTests/EC.Spotify.UnitTests.csproj

# Run all integration tests
dotnet test src/EC.Spotify/EC.Spotify.Tests/EC.Spotify.Tests.csproj

# Run tests with coverage
dotnet test src/EC.Spotify/EC.Spotify.UnitTests/EC.Spotify.UnitTests.csproj --collect:"XPlat Code Coverage"

# Count test methods
Get-ChildItem src/EC.Spotify.UnitTests/Services/*.cs -Recurse | 
    Select-String "\[Fact\]" | 
    Measure-Object

# Verify all services have tests
$services = Get-ChildItem src/EC.Spotify/EC.Spotify/Services/*.cs | Select-Object -ExpandProperty BaseName
$tests = Get-ChildItem src/EC.Spotify.UnitTests/Services/*.cs | Select-Object -ExpandProperty BaseName
$missing = $services | Where-Object { $_ -notlike "*Tests" -and $_ -notin $tests }
$missing
```

## Example Execution

### Full Test Generation
```
Skill: spotify-test
Action: Generating unit and integration tests
Input: All service implementations
Changes:
  - Created 10 unit test files (one per service)
  - Created 10 integration test files (one per service)
  - Added 85 unit test methods (all service methods)
  - Added 85 integration test methods (all service methods)
  - Created 20 mock implementations
  - Updated test data fixtures
Validation: dotnet test SUCCESS (170 tests passed)
Coverage: 87%
Status: SUCCESS
```

### Incremental Update
```
Skill: spotify-test
Action: Updating tests for recent changes
Changes:
  - Updated 3 unit test files (new methods added)
  - Updated 3 integration test files (new methods added)
  - Added 12 unit test methods
  - Added 12 integration test methods
  - Updated 5 mock implementations
Validation: dotnet test SUCCESS (170 tests passed)
Coverage: 87%
Status: SUCCESS
```

### Test Failures Detected
```
Skill: spotify-test
Action: Running tests after library updates
Changes:
  - Updated 2 unit test files (signature changes)
  - Fixed 3 failing tests
Validation: dotnet test SUCCESS (170 tests passed)
Coverage: 87%
Status: SUCCESS (fixed 3 failures)
```

## Dependencies
- **Runs after**: SpotifyUpdateSkill (must test current implementation)
- **Runs after**: SpotifyExampleSkill (tests can use examples as reference)
- **Requires**: dotnet CLI, xUnit, Moq, code coverage tools

## Troubleshooting

### Test Compilation Errors
**Error**: "dotnet test failed to compile"  
**Fix**: Check for:
- Missing method implementations
- Incorrect method signatures
- Missing using statements
- Incorrect mock setups

### Test Failures
**Error**: "Test failed: Expected X but got Y"  
**Fix**: 
- Verify implementation matches expected behavior
- Update test if implementation changed intentionally
- Check mock setups are correct

### Low Coverage
**Error**: "Coverage below threshold (e.g., <80%)"  
**Fix**:
- Add tests for untested code paths
- Test error conditions and edge cases
- Add theory tests with multiple data points

### Missing Mocks
**Error**: "Mock not found for dependency"  
**Fix**: Create mock implementation in appropriate Mocks folder

### Integration Test Timeouts
**Error**: "Test timed out waiting for response"  
**Fix**:
- Ensure mock HTTP provider returns responses
- Increase timeout if testing real APIs
- Check network connectivity
