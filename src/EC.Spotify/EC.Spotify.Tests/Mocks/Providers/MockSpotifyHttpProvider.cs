using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Models.Players;
using EC.Spotify.Tests.Core.Providers;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace EC.Spotify.Tests.Mocks.Providers;

internal class MockSpotifyHttpProvider() : ISpotifyHttpProvider
{
    public Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(method, nameof(method));
        ArgumentException.ThrowIfNullOrEmpty(uri, nameof(uri));

        string? ret = default;

        var modelTypes = Assembly.GetAssembly(typeof(ISpotifyHttpProvider))
            ?.GetTypes()
            ?.Where(t => t.Namespace?.Contains("EC.Spotify.Models") ?? false)
            .ToList();

        Type? modelType = default;

        var slugs = uri?.Replace("https://api.spotify.com/v1/", "")?.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Reverse();
        foreach(var slug in slugs ?? []) 
        {
            modelType = modelTypes?.FirstOrDefault(t => 
                slug.Contains(t.Name.ToLower(), StringComparison.InvariantCultureIgnoreCase) 
                || t.Name.Contains(slug, StringComparison.InvariantCultureIgnoreCase)
            );
            if (modelType is not null) break;
        }

        var obj = ReflectionProvider.PopulateObjectRecursive(modelType);

        if (modelType == typeof(Device)) obj = new { devices = new[] { obj } };

        ret = JsonSerializer.Serialize(obj);

        return Task.FromResult<string?>(ret);
    }
}
