
namespace EC.Spotify.Models;

public class SpotifyResult<T>    
{
    public bool IsSuccess => Error is null;
    public T? Data { get; set; }
    public SpotifyError? Error { get; set; }
}
