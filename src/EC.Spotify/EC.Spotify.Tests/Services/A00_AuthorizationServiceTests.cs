using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class A00_AuthorizationServiceTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task T001_Validate_ShouldReturnAuthorizationUrl()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var code = await sut.AuthorizationCodeGetAsync(cancellationToken: TestContext.CancellationToken);
        _ = sut.AuthorizationCodeRemoveAsync(cancellationToken: TestContext.CancellationToken);

        var res = await sut.ValidateAsync(cancellationToken: TestContext.CancellationToken);

        _ = sut.AuthorizationCodeAddAsync(code, cancellationToken: TestContext.CancellationToken);

        Assert.IsNotNull(res);
    }
}
