using System.Text;

namespace EC.Spotify.Extensions;

internal static class UriBuilderExtensions
{
    public static UriBuilder AddQuery(this UriBuilder uriBuilder, Dictionary<string, string?>? keyValuePairs)
    {
        var queryBuilder = new StringBuilder();

        queryBuilder.AppendJoin("&", keyValuePairs?.Select(i => $"{i.Key}={i.Value}") ?? []);

        uriBuilder.Query = queryBuilder.ToString();

        return uriBuilder;
    }
}
