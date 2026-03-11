using System.Diagnostics;
using System.Net;

namespace EC.Spotify.Tests.Core.Providers;

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

internal static class WebRedirectListenerProvider
{
    public static void ListenForRedirect(string? requestUri, string? responseUri, Action<HttpListenerContext>? responseContext)
    {
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("Value cannot be null or empty.", nameof(requestUri));
        if (string.IsNullOrEmpty(responseUri)) throw new ArgumentException("Value cannot be null or empty.", nameof(responseUri));                
        if (responseContext is null) throw new ArgumentNullException(nameof(responseContext));

        // start server to wait for redirect
        var listener = new HttpListener();
        var process = default(Process);
        try
        {
            // set listening path and start listening
            listener.Prefixes.Add(responseUri);
            listener.Start();

            // open browser        
            process = Process.Start(new ProcessStartInfo(requestUri)
            {
                UseShellExecute = true,
                Verb = "open"
            });
            ArgumentNullException.ThrowIfNull(process, nameof(process));

            // wait for request
            var listenerContext = listener.GetContext();

            listener.Stop();

            responseContext?.Invoke(listenerContext);
        }
        catch (Exception ex)
        {
            listener.Stop();
            throw new Exception("Error starting response server", ex);
        }
        // kill the process
        process?.Close();

    }
}
