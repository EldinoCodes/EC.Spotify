using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class A00_AuthorizationServiceTests
{
    [TestMethod]
    public async Task Validate_ShouldReturnAuthorizationUrl()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var code = await sut.AuthorizationCodeGetAsync();
        _ = sut.AuthorizationCodeRemoveAsync();

        var res = await sut.Validate();

        _ = sut.AuthorizationCodeAddAsync(code);

        Assert.IsNotNull(res);
    }
}
