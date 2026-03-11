using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Serialization;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Tests.Core.Providers;
using EC.Spotify.Tests.Mocks.Providers;
using EC.Spotify.Tests.Mocks.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using System.Runtime;

namespace EC.Spotify.Tests;



[TestClass]
[DoNotParallelize]
public static class Initializer
{
    private static IServiceProvider? _container { get; set; }
    public static T? Resolve<T>() => _container is not null ? _container.GetService<T>() : default;
    public static object? Resolve(Type type) => _container?.GetService(type);
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

        var fullEnd2EndTest = configuration.GetValue<bool>("FullEnd2EndTest");
        if (!fullEnd2EndTest)
        {
            services.RemoveAll<ISpotifyHttpProvider>();
            services.RemoveAll<ISpotifyJsonSerializer>();

            services.AddSingleton<ISpotifyHttpProvider, MockSpotifyHttpProvider>();
            services.AddSingleton<ISpotifyJsonSerializer, MockSpotifyJsonSerialization>();            
        }

        _container = services.BuildServiceProvider();

        if (fullEnd2EndTest)
        {
            var listenUri = configuration.GetValue<string>("Spotify:ListenUri");
            StartAuthenticationServer(listenUri);
        }
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        if (_container is not null)
        {
            if (_container is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _container = null;
        }
    }

    public static void StartAuthenticationServer(string? listenUri)
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
