using EC.Spotify.Models.Albums;
using EC.Spotify.Models.Audiobooks;
using EC.Spotify.Models.Players;
using EC.Spotify.Models.Shows;
using System.Text.Json.Serialization;

namespace EC.Spotify.Abstractions.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Album), "album")]
[JsonDerivedType(typeof(Artist), "artist")]
[JsonDerivedType(typeof(Audiobook), "audiobook")]
[JsonDerivedType(typeof(Chapter), "chapter")]
[JsonDerivedType(typeof(Episode), "episode")]
[JsonDerivedType(typeof(Playlist), "playlist")]
[JsonDerivedType(typeof(Show), "show")]
[JsonDerivedType(typeof(Track), "track")]
public interface IPolymorphicItem;