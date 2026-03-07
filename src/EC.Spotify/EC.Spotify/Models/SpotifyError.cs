
namespace EC.Spotify.Models;

public class SpotifyError
{
    public int? Status { get; set; }
    public string? Message { get; set; }
    public string? Reason { get; set; }
}
