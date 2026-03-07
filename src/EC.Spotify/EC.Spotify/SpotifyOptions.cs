namespace EC.Spotify;

public class SpotifyOptions
{
    public virtual string? ClientId { get; set; }
    public virtual string? ClientSecret { get; set; }
    public virtual string? RedirectUri { get; set; }
    public virtual List<string> Scopes { get; set; } = [];
}
