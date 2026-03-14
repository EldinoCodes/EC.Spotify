using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class Z00_AuthorizationServiceTests
{
    [TestMethod]
    public async Task AuthorizationTokenReset_ShouldResetToken()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var before = await sut.Validate();
    
        _ = await sut.AuthorizationTokenReset();
        
        var after = await sut.Validate();

        Assert.AreNotEqual(before, after);
    }
}
