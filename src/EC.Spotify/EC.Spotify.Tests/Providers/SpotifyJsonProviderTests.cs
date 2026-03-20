using EC.Spotify.Abstractions.Providers;
using EC.Spotify.Abstractions.Services;
using EC.Spotify.Models;
using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.Models.Auth;
using EC.Spotify.Models.Players;
using EC.Spotify.Models.Playlists;
using EC.Spotify.Models.Shows;
using EC.Spotify.Providers;

namespace EC.Spotify.Tests.Providers;

[TestClass]
public sealed class SpotifyJsonProviderTests
{
    [TestMethod]
    [DataRow("AuthToken.json", typeof(AuthToken))]
    [DataRow("SpotifyResult_Album.json", typeof(SpotifyResult<Album>))]
    [DataRow("SpotifyResult_Artist.json", typeof(SpotifyResult<Artist>))]
    [DataRow("SpotifyResult_Audiobook.json", typeof(SpotifyResult<Audiobook>))]
    [DataRow("SpotifyResult_bool.json", typeof(SpotifyResult<bool>))]
    [DataRow("SpotifyResult_Chapter.json", typeof(SpotifyResult<Chapter>))]
    [DataRow("SpotifyResult_Episode.json", typeof(SpotifyResult<Episode>))]
    [DataRow("SpotifyResult_List_bool.json", typeof(SpotifyResult<List<bool>>))]
    [DataRow("SpotifyResult_List_Device.json", typeof(SpotifyResult<List<Device>>))]
    [DataRow("SpotifyResult_PlayerQueue.json", typeof(SpotifyResult<PlayerQueue>))]
    [DataRow("SpotifyResult_Playlist.json", typeof(SpotifyResult<Playlist>))]
    [DataRow("SpotifyResult_Show.json", typeof(SpotifyResult<Show>))]
    [DataRow("SpotifyResult_SpotifyPageResult.json", typeof(SpotifyResult<SpotifyPageResult>))]
    [DataRow("SpotifyResult_Track.json", typeof(SpotifyResult<Track>))]
    public async Task DeserializeTest(string? filePath, Type type)
    {
        var data = Initializer.LoadData(filePath);

        var sut = Initializer.Resolve<SpotifyJsonProvider>();
        ArgumentNullException.ThrowIfNull(sut, nameof(sut));

        var serializerType = typeof(ISpotifyJsonProvider);
        var deserializeMethod = serializerType.GetMethod("Deserialize");
        ArgumentNullException.ThrowIfNull(deserializeMethod, nameof(deserializeMethod));

        var ret = deserializeMethod.MakeGenericMethod(type).Invoke(sut, [data, null]);

        Assert.IsTrue(ret is not null && ret.GetType() == type);
    }
}
