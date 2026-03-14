using EC.Spotify.Abstractions.Services;

namespace EC.Spotify.Tests.Services;

[TestClass]
public sealed class T00_AuthorizationServiceTests
{
    // this is only a valid call before authorization
    //[TestMethod]
    //public async Task Validate_ShouldReturnAuthorizationUrl()
    //{
    //    var sut = Initializer.Resolve<IAuthorizationService>();
    //    ArgumentNullException.ThrowIfNull(sut, nameof(sut));

    //    var res = await sut.Validate();
    //    Assert.IsNotNull(res);
    //}

    [TestMethod]
    public void AuthorizationCodeUrl_ShouldReturnUrl()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var ret = sut.AuthorizationCodeUrl();
        Assert.IsNotNull(ret);
    }

    [TestMethod]
    public async Task AuthorizationCodeAddAsync_ShouldAddCode()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var code = await sut.AuthorizationCodeGetAsync();
        var ret = await sut.AuthorizationCodeAddAsync("test_code");
        _ = await sut.AuthorizationCodeAddAsync(code);

        Assert.IsTrue(ret);
    }

    [TestMethod]
    public async Task AuthorizationCodeGetAsync_ShouldReturnCode()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var newCode = "test_code";

        var existingCode = await sut.AuthorizationCodeGetAsync();
        if (string.IsNullOrEmpty(existingCode))
            await sut.AuthorizationCodeAddAsync(newCode);

        var ret = await sut.AuthorizationCodeGetAsync();
        Assert.IsNotNull(ret);
    }

    [TestMethod]
    public async Task AuthorizationCodeRemoveAsync_ShouldRemoveCode()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var code = await sut.AuthorizationCodeGetAsync();
        var ret = await sut.AuthorizationCodeRemoveAsync();
        var res = await sut.AuthorizationCodeAddAsync(code);

        Assert.IsTrue(ret);

        
    }

    [TestMethod]
    public async Task AuthorizationTokenGetAsync_ShouldReturnToken()
    {
        var sut = Initializer.Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var newCode = "test_code";

        var existingCode = await sut.AuthorizationCodeGetAsync();
        if (string.IsNullOrEmpty(existingCode))
            await sut.AuthorizationCodeAddAsync(newCode);
        
        var ret = await sut.AuthorizationTokenGetAsync();

        Assert.IsNotNull(ret);
    }

    // this is only a valid call after all other tests have run
    //[TestMethod]
    //public async Task AuthorizationTokenReset_ShouldResetToken()
    //{
    //    var sut = Initializer.Resolve<IAuthorizationService>();
    //    ArgumentNullException.ThrowIfNull(sut, nameof(sut));

    //    // Ensure we have a code and token before testing reset
    //    var existingCode = await sut.AuthorizationCodeGetAsync();

    //    var before = await sut.Validate();
    //    _ = await sut.AuthorizationTokenReset();
    //    var after = await sut.Validate();

    //    // Re-add the code to ensure token can be retrieved again
    //    _ = await sut.AuthorizationCodeAddAsync(existingCode);
    //    var token = await sut.AuthorizationTokenGetAsync();

    //    Assert.AreNotEqual(before, after);
    //}
}
