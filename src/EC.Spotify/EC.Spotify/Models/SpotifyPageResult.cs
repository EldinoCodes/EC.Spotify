using System.Text.Json.Serialization;

namespace EC.Spotify.Models;

/// <summary>
/// Represents a single page of results from a paginated Spotify API response.
/// </summary>
/// <typeparam name="T">The type of items contained in this page.</typeparam>
public class SpotifyPageResult<T>
{
    private string? _next;
    private string? _prev;

    /// <summary>
    /// Gets or sets the maximum number of items returned in this page.
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    
    /// <summary>
    /// Gets or sets the query string for requesting the next page of results, or <see langword="null"/> if
    /// there are no more pages.
    /// </summary>
    /// <remarks>The full Spotify-provided URL is reduced to its query string portion (e.g.
    /// <c>?limit=20&amp;offset=20</c>) when this property is set.</remarks>
    [JsonPropertyName("next")]
    public string? Next
    {
        get
        {
            return _next;
        }
        set {
            _next = Uri.TryCreate(value, new UriCreationOptions(), out var uri) ? uri.Query: value;
        }
    }
    /// <summary>
    /// Gets or sets the zero-based index of the first item in this page within the full result set.
    /// </summary>
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    /// <summary>
    /// Gets or sets the query string for requesting the previous page of results, or <see langword="null"/>
    /// if this is the first page.
    /// </summary>
    /// <remarks>The full Spotify-provided URL is reduced to its query string portion (e.g.
    /// <c>?limit=20&amp;offset=0</c>) when this property is set.</remarks>
    [JsonPropertyName("previous")]
    public string? Previous
    {
        get
        {
            return _prev;
        }
        set
        {
            _prev = Uri.TryCreate(value, new UriCreationOptions(), out var uri) ? uri.Query : value;
        }
    }
    /// <summary>
    /// Gets or sets the total number of items available across all pages.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }
    /// <summary>
    /// Gets or sets the items in this page. <see langword="null"/> if no items were returned.
    /// </summary>
    [JsonPropertyName("items")]
    public List<T>? Items { get; set; }
}
