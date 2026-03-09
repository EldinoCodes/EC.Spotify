using EC.Spotify.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Net;
using System.Reflection;

[assembly: Parallelize(Scope = ExecutionScope.ClassLevel)]

namespace EC.Spotify.Tests;

/*
 * if HttpListener isnt working binding to https, but gives no errors try adding local cert:
 * 
 * in admin powershell:
 * 
 * -- add ssl cert to local machine store
 * New-SelfSignedCertificate -DnsName "127.0.0.1" -CertStoreLocation "cert:\LocalMachine\My"
 * 
 * -- create self signed cert (hash value is the thumbprint of the cert created in previous step)
 * netsh http add sslcert ipport=0.0.0.0:5001 certhash=<thumbprint> appid='{a0085f2a-94c0-4108-a63b-74d48c1a8f4c}'
 */


[TestClass]
public class Initializer
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

        _container = services.BuildServiceProvider();

        var listenUri = configuration.GetValue<string>("Spotify:ListenUri");

        StartAuthenticationServer(listenUri);
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

        // start server to wait for redirect
        var listener = new HttpListener();
        var process = default(Process);
        try
        {
            // set listening path and start listening
            listener.Prefixes.Add(listenUri);
            listener.Start();

            // open browser        
            process = Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true,
                Verb = "open"
            });
            ArgumentNullException.ThrowIfNull(process, nameof(process));

            // wait for request
            var context = listener.GetContext();
            listener.Stop();

            // pull auth code response
            var authorizationCode = context.Request.QueryString.Get("code");
            ArgumentException.ThrowIfNullOrEmpty(authorizationCode, nameof(authorizationCode));

            // add response code, generate token for tests
            _ = authService.AuthorizationCodeAddAsync(authorizationCode).Result;
            Task.Delay(1000).Wait(); // wait for code to be stored

            _ = authService.AuthorizationTokenGetAsync().Result;
            Task.Delay(1000).Wait(); // wait for token to be generated
        }
        catch (Exception ex)
        {
            listener.Stop();
            throw new Exception("Error starting authentication server", ex);
        }

        // kill the process
        process?.Close();

        Task.Delay(4000).Wait(); // wait for token to be generated
    }
}
