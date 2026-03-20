using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Providers;
using EC.Spotify.Tests.Core.Providers;
using EC.Spotify.Tests.Mocks.Providers;
using EC.Spotify.Tests.Mocks.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace EC.Spotify.Tests;


[TestClass]
[DoNotParallelize]
public static class Initializer
{
    private static IServiceProvider? _container { get; set; }

    public static object? Resolve(Type type) => _container?.GetService(type);
    public static T? Resolve<T>() => _container is not null ? _container.GetService<T>() : default;    
    
    public static string? LoadData(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return default;
        var asm = Assembly.GetExecutingAssembly();
        var path = Path.Combine(Directory.GetParent(asm.Location)?.FullName ?? string.Empty, "TestData", fileName);
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(fileStream);
        return reader.ReadToEnd();
    }

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        ArgumentException.ThrowIfNullOrEmpty(dir, nameof(dir));

        var configurationBuilder = new ConfigurationBuilder().AddJsonFile(Path.Combine(dir, "appsettings.json"), false);
        foreach (var path in Directory.GetFiles(dir, "appsettings.*.json")) 
            configurationBuilder.AddJsonFile(path, true);

        var configuration = configurationBuilder.Build();
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);

        services.AddSpotify(configuration.GetSection("Spotify"));

        services.RemoveAll<ISpotifyHttpProvider>();
        services.AddSingleton<ISpotifyHttpProvider, MockSpotifyHttpProvider>();

        services.RemoveAll<ISpotifyJsonProvider>();
        services.AddSingleton<SpotifyJsonProvider>();
        services.AddSingleton<ISpotifyJsonProvider, MockSpotifyJsonProvider>();        

        services.RemoveAll<IAuthorizationService>();
        services.AddSingleton<IAuthorizationService, MockAuthorizationService>();

        _container = services.BuildServiceProvider();

        DummyProvider.AddDummy<SpotifyError?>(null);

        var fullEnd2EndTest = configuration.GetValue<bool>("FullEnd2EndTest");
        if (fullEnd2EndTest)
        {
            var listenUri = configuration.GetValue<string>("Spotify:ListenUri");
            StartAuthenticationServer(listenUri);
        }
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        if (_container is null) return;
        if (_container is IDisposable disposable) disposable.Dispose();

        _container = null;
    }    


    private static void StartAuthenticationServer(string? listenUri)
    {
        ArgumentException.ThrowIfNullOrEmpty(listenUri, nameof(listenUri));

        // get service and url
        var authService = Resolve<IAuthorizationService>();
        ArgumentNullException.ThrowIfNull(authService, nameof(authService));

        var url = authService.AuthorizationCodeUrl();
        ArgumentException.ThrowIfNullOrEmpty(url, nameof(url));

        WebRedirectListenerProvider.ListenForRedirect(url, listenUri, (context) =>
        {
            var authorizationCode = context.Request.QueryString.Get("code");

            // add response code, generate token for tests
            _ = authService.AuthorizationCodeAddAsync(authorizationCode).Result;
            Task.Delay(1000).Wait(); // wait for code to be stored

            _ = authService.AuthorizationTokenGetAsync().Result;
            Task.Delay(1000).Wait(); // wait for token to be generated
        });
    }    
}
