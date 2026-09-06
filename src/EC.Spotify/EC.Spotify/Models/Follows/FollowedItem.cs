namespace EC.Spotify.Models.Follows;

/// <summary>
/// Represents a followed artist or user item.
/// </summary>
public class FollowedItem
{
    /// <summary>
    /// The external URLs for this item.
    /// </summary>
    public Shared.ExternalUrl? ExternalUrls { get; set; }

    /// <summary>
    /// The href for this item.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// The ID of this item.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The type of this item (artist or user).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// The URI of this item.
    /// </summary>
    public string? Uri { get; set; }

    /// <summary>
    /// The display name of this item.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Whether the item is currently followed.
    /// </summary>
    public bool? Following { get; set; }
}
