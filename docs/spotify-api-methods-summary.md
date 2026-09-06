# Spotify Web API Methods Summary
**Generated on: 2026-09-05**

## Albums
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Album](https://developer.spotify.com/documentation/web-api/reference/get-an-album) | Get Spotify catalog information for a single album. | False |  | GET | /albums/{id} |
| [Get Several Albums](https://developer.spotify.com/documentation/web-api/reference/get-multiple-albums) | Get Spotify catalog information for multiple albums identified by their Spotify IDs. | True |  | GET | /albums |
| [Get Album Tracks](https://developer.spotify.com/documentation/web-api/reference/get-an-albums-tracks) | Get Spotify catalog information about an album’s tracks.Optional parameters can be used to limit the number of tracks returned. | False |  | GET | /albums/{id}/tracks |
| [Get User's Saved Albums](https://developer.spotify.com/documentation/web-api/reference/get-users-saved-albums) | Get a list of the albums saved in the current Spotify user's 'Your Music' library. | False | user-library-read | GET | /me/albums |
| [Save Albums for Current User](https://developer.spotify.com/documentation/web-api/reference/save-albums-user) | Save one or more albums to the current user's 'Your Music' library. | True | user-library-modify | PUT | /me/albums |
| [Remove Users' Saved Albums](https://developer.spotify.com/documentation/web-api/reference/remove-albums-user) | Remove one or more albums from the current user's 'Your Music' library. | True | user-library-modify | DELETE | /me/albums |
| [Check User's Saved Albums](https://developer.spotify.com/documentation/web-api/reference/check-users-saved-albums) | Check if one or more albums is already saved in the current Spotify user's 'Your Music' library. | True | user-library-read | GET | /me/albums/contains |
| [Get New Releases](https://developer.spotify.com/documentation/web-api/reference/get-new-releases) | Get a list of new album releases featured in Spotify (shown, for example, on a Spotify player’s “Browse” tab). | True |  | GET | /browse/new-releases |


## Artists
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Artist](https://developer.spotify.com/documentation/web-api/reference/get-an-artist) | Get Spotify catalog information for a single artist identified by their unique Spotify ID. | False |  | GET | /artists/{id} |
| [Get Several Artists](https://developer.spotify.com/documentation/web-api/reference/get-multiple-artists) | Get Spotify catalog information for several artists based on their Spotify IDs. | True |  | GET | /artists |
| [Get Artist's Albums](https://developer.spotify.com/documentation/web-api/reference/get-an-artists-albums) | Get Spotify catalog information about an artist's albums. | False |  | GET | /artists/{id}/albums |
| [Get Artist's Top Tracks](https://developer.spotify.com/documentation/web-api/reference/get-an-artists-top-tracks) | Get Spotify catalog information about an artist's top tracks by country. | True |  | GET | /artists/{id}/top-tracks |
| [Get Artist's Related Artists](https://developer.spotify.com/documentation/web-api/reference/get-an-artists-related-artists) | Get Spotify catalog information about artists similar to a given artist. Similarity is based on analysis of the Spotify community's listening history. | True |  | GET | /artists/{id}/related-artists |


## Audiobooks
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get an Audiobook](https://developer.spotify.com/documentation/web-api/reference/get-an-audiobook) | Get Spotify catalog information for a single audiobook. Audiobooks are only available within the US, UK, Canada, Ireland, New Zealand and Australia markets. | False |  | GET | /audiobooks/{id} |
| [Get Several Audiobooks](https://developer.spotify.com/documentation/web-api/reference/get-multiple-audiobooks) | Get Spotify catalog information for several audiobooks identified by their Spotify IDs. Audiobooks are only available within the US, UK, Canada, Ireland, New Zealand and Australia markets. | True |  | GET | /audiobooks |
| [Get Audiobook Chapters](https://developer.spotify.com/documentation/web-api/reference/get-audiobook-chapters) | Get Spotify catalog information about an audiobook's chapters. Audiobooks are only available within the US, UK, Canada, Ireland, New Zealand and Australia markets. | False |  | GET | /audiobooks/{id}/chapters |
| [Get User's Saved Audiobooks](https://developer.spotify.com/documentation/web-api/reference/get-users-saved-audiobooks) | Get a list of the audiobooks saved in the current Spotify user's 'Your Music' library. | False | user-library-read | GET | /me/audiobooks |
| [Save Audiobooks for Current User](https://developer.spotify.com/documentation/web-api/reference/save-audiobooks-user) | Save one or more audiobooks to the current Spotify user's library. | True | user-library-modify | PUT | /me/audiobooks |
| [Remove User's Saved Audiobooks](https://developer.spotify.com/documentation/web-api/reference/remove-audiobooks-user) | Remove one or more audiobooks from the Spotify user's library. | True | user-library-modify | DELETE | /me/audiobooks |
| [Check User's Saved Audiobooks](https://developer.spotify.com/documentation/web-api/reference/check-users-saved-audiobooks) | Check if one or more audiobooks are already saved in the current Spotify user's library. | True | user-library-read | GET | /me/audiobooks/contains |


## Categories
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Several Browse Categories](https://developer.spotify.com/documentation/web-api/reference/get-categories) | Get a list of categories used to tag items in Spotify (on, for example, the Spotify player’s “Browse” tab). | True |  | GET | /browse/categories |
| [Get Single Browse Category](https://developer.spotify.com/documentation/web-api/reference/get-a-category) | Get a single category used to tag items in Spotify (on, for example, the Spotify player’s “Browse” tab). | True |  | GET | /browse/categories/{category_id} |


## Chapters
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get a Chapter](https://developer.spotify.com/documentation/web-api/reference/get-a-chapter) | Get Spotify catalog information for a single audiobook chapter. Chapters are only available within the US, UK, Canada, Ireland, New Zealand and Australia markets. | False |  | GET | /chapters/{id} |
| [Get Several Chapters](https://developer.spotify.com/documentation/web-api/reference/get-several-chapters) | Get Spotify catalog information for several audiobook chapters identified by their Spotify IDs. Chapters are only available within the US, UK, Canada, Ireland, New Zealand and Australia markets. | True |  | GET | /chapters |


## Episodes
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Episode](https://developer.spotify.com/documentation/web-api/reference/get-an-episode) | Get Spotify catalog information for a single episode identified by itsunique Spotify ID. | False | user-read-playback-position | GET | /episodes/{id} |
| [Get Several Episodes](https://developer.spotify.com/documentation/web-api/reference/get-multiple-episodes) | Get Spotify catalog information for several episodes based on their Spotify IDs. | True | user-read-playback-position | GET | /episodes |
| [Get User's Saved Episodes](https://developer.spotify.com/documentation/web-api/reference/get-users-saved-episodes) | Get a list of the episodes saved in the current Spotify user's library. | False | user-library-read, user-read-playback-position | GET | /me/episodes |
| [Save Episodes for Current User](https://developer.spotify.com/documentation/web-api/reference/save-episodes-user) | Save one or more episodes to the current user's library. | True | user-library-modify | PUT | /me/episodes |
| [Remove User's Saved Episodes](https://developer.spotify.com/documentation/web-api/reference/remove-episodes-user) | Remove one or more episodes from the current user's library. | True | user-library-modify | DELETE | /me/episodes |
| [Check User's Saved Episodes](https://developer.spotify.com/documentation/web-api/reference/check-users-saved-episodes) | Check if one or more episodes is already saved in the current Spotify user's 'Your Episodes' library. | True | user-library-read | GET | /me/episodes/contains |


## Genres
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Available Genre Seeds](https://developer.spotify.com/documentation/web-api/reference/get-recommendation-genres) | Retrieve a list of available genres seed parameter values for | True |  | GET | /recommendations/available-genre-seeds |


## Library
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Save Items to Library](https://developer.spotify.com/documentation/web-api/reference/save-library-items) | Add one or more items to the current user's library. Accepts Spotify URIs for tracks, albums, episodes, shows, audiobooks, users, and playlists. | False | user-library-modify, user-follow-modify, playlist-modify-public | PUT | /me/library |
| [Remove Items from Library](https://developer.spotify.com/documentation/web-api/reference/remove-library-items) | Remove one or more items from the current user's library. Accepts Spotify URIs for tracks, albums, episodes, shows, audiobooks, users, and playlists. | False | user-library-modify, user-follow-modify, playlist-modify-public | DELETE | /me/library |
| [Check User's Saved Items](https://developer.spotify.com/documentation/web-api/reference/check-library-contains) | Check if one or more items are already saved in the current user's library. Accepts Spotify URIs for tracks, albums, episodes, shows, audiobooks, artists, users, and playlists. | False | user-library-read, user-follow-read, playlist-read-private | GET | /me/library/contains |


## Markets
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Available Markets](https://developer.spotify.com/documentation/web-api/reference/get-available-markets) | Get the list of markets where Spotify is available. | True |  | GET | /markets |


## Player
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Playback State](https://developer.spotify.com/documentation/web-api/reference/get-information-about-the-users-current-playback) | Get information about the user’s current playback state, including track or episode, progress, and active device. | False | user-read-playback-state | GET | /me/player |
| [Transfer Playback](https://developer.spotify.com/documentation/web-api/reference/transfer-a-users-playback) | Transfer playback to a new device and optionally begin playback. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | PUT | /me/player |
| [Get Available Devices](https://developer.spotify.com/documentation/web-api/reference/get-a-users-available-devices) | Get information about a user’s available Spotify Connect devices. Some device models are not supported and will not be listed in the API response. | False | user-read-playback-state | GET | /me/player/devices |
| [Get Currently Playing Track](https://developer.spotify.com/documentation/web-api/reference/get-the-users-currently-playing-track) | Get the object currently being played on the user's Spotify account. | False | user-read-currently-playing | GET | /me/player/currently-playing |
| [Start/Resume Playback](https://developer.spotify.com/documentation/web-api/reference/start-a-users-playback) | Start a new context or resume current playback on the user's active device. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | PUT | /me/player/play |
| [Pause Playback](https://developer.spotify.com/documentation/web-api/reference/pause-a-users-playback) | Pause playback on the user's account. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | PUT | /me/player/pause |
| [Skip To Next](https://developer.spotify.com/documentation/web-api/reference/skip-users-playback-to-next-track) | Skips to next track in the user’s queue. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | POST | /me/player/next |
| [Skip To Previous](https://developer.spotify.com/documentation/web-api/reference/skip-users-playback-to-previous-track) | Skips to previous track in the user’s queue. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | POST | /me/player/previous |
| [Seek To Position](https://developer.spotify.com/documentation/web-api/reference/seek-to-position-in-currently-playing-track) | Seeks to the given position in the user’s currently playing track. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | PUT | /me/player/seek |
| [Set Repeat Mode](https://developer.spotify.com/documentation/web-api/reference/set-repeat-mode-on-users-playback) | Set the repeat mode for the user's playback. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | PUT | /me/player/repeat |
| [Set Playback Volume](https://developer.spotify.com/documentation/web-api/reference/set-volume-for-users-playback) | Set the volume for the user’s current playback device. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | PUT | /me/player/volume |
| [Toggle Playback Shuffle](https://developer.spotify.com/documentation/web-api/reference/toggle-shuffle-for-users-playback) | Toggle shuffle on or off for user’s playback. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | PUT | /me/player/shuffle |
| [Get Recently Played Tracks](https://developer.spotify.com/documentation/web-api/reference/get-recently-played) | Get tracks from the current user's recently played tracks. | False | user-read-recently-played | GET | /me/player/recently-played |
| [Get the User's Queue](https://developer.spotify.com/documentation/web-api/reference/get-queue) | Get the list of objects that make up the user's queue. | False | user-read-currently-playing, user-read-playback-state | GET | /me/player/queue |
| [Add Item to Playback Queue](https://developer.spotify.com/documentation/web-api/reference/add-to-queue) | Add an item to be played next in the user's current playback queue. This API only works for users who have Spotify Premium. The order of execution is not guaranteed when you use this API with other Player API endpoints. | False | user-modify-playback-state | POST | /me/player/queue |


## Playlists
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Playlist](https://developer.spotify.com/documentation/web-api/reference/get-playlist) | Get a playlist owned by a Spotify user. | False |  | GET | /playlists/{playlist_id} |
| [Change Playlist Details](https://developer.spotify.com/documentation/web-api/reference/change-playlist-details) | Change a playlist's name and public/private state. (The user must, ofcourse, own the playlist.) | False | playlist-modify-public, playlist-modify-private | PUT | /playlists/{playlist_id} |
| [Get Playlist Items [DEPRECATED]](https://developer.spotify.com/documentation/web-api/reference/get-playlists-tracks) | Get full details of the items of a playlist owned by a Spotify user. | True | playlist-read-private | GET | /playlists/{playlist_id}/tracks |
| [Update Playlist Items [DEPRECATED]](https://developer.spotify.com/documentation/web-api/reference/reorder-or-replace-playlists-tracks) | Update Playlist Items | True | playlist-modify-public, playlist-modify-private | PUT | /playlists/{playlist_id}/tracks |
| [Add Items to Playlist [DEPRECATED]](https://developer.spotify.com/documentation/web-api/reference/add-tracks-to-playlist) | Add Items to Playlist | True | playlist-modify-public, playlist-modify-private | POST | /playlists/{playlist_id}/tracks |
| [Remove Playlist Items [DEPRECATED]](https://developer.spotify.com/documentation/web-api/reference/remove-tracks-playlist) | Remove Playlist Items | True | playlist-modify-public, playlist-modify-private | DELETE | /playlists/{playlist_id}/tracks |
| [Get Playlist Items](https://developer.spotify.com/documentation/web-api/reference/get-playlists-items) | Get full details of the items of a playlist owned by a Spotify user. | False | playlist-read-private | GET | /playlists/{playlist_id}/items |
| [Update Playlist Items](https://developer.spotify.com/documentation/web-api/reference/reorder-or-replace-playlists-items) | Either reorder or replace items in a playlist depending on the request's parameters.To reorder items, include | False | playlist-modify-public, playlist-modify-private | PUT | /playlists/{playlist_id}/items |
| [Add Items to Playlist](https://developer.spotify.com/documentation/web-api/reference/add-items-to-playlist) | Add one or more items to a user's playlist. | False | playlist-modify-public, playlist-modify-private | POST | /playlists/{playlist_id}/items |
| [Remove Playlist Items](https://developer.spotify.com/documentation/web-api/reference/remove-items-playlist) | Remove one or more items from a user's playlist. | False | playlist-modify-public, playlist-modify-private | DELETE | /playlists/{playlist_id}/items |
| [Get Current User's Playlists](https://developer.spotify.com/documentation/web-api/reference/get-a-list-of-current-users-playlists) | Get a list of the playlists owned or followed by the current Spotifyuser. | False | playlist-read-private | GET | /me/playlists |
| [Create Playlist](https://developer.spotify.com/documentation/web-api/reference/create-playlist) | Create a playlist for the current Spotify user. (The playlist will be empty untilyou | False | playlist-modify-public, playlist-modify-private | POST | /me/playlists |
| [Get User's Playlists](https://developer.spotify.com/documentation/web-api/reference/get-list-users-playlists) | Get a list of the playlists owned or followed by a Spotify user. | True | playlist-read-private, playlist-read-collaborative | GET | /users/{user_id}/playlists |
| [Create Playlist for user](https://developer.spotify.com/documentation/web-api/reference/create-playlist-for-user) | Create a playlist for a Spotify user. (The playlist will be empty untilyou | True | playlist-modify-public, playlist-modify-private | POST | /users/{user_id}/playlists |
| [Get Featured Playlists](https://developer.spotify.com/documentation/web-api/reference/get-featured-playlists) | Get a list of Spotify featured playlists (shown, for example, on a Spotify player's 'Browse' tab). | True |  | GET | /browse/featured-playlists |
| [Get Category's Playlists](https://developer.spotify.com/documentation/web-api/reference/get-a-categories-playlists) | Get a list of Spotify playlists tagged with a particular category. | True |  | GET | /browse/categories/{category_id}/playlists |
| [Get Playlist Cover Image](https://developer.spotify.com/documentation/web-api/reference/get-playlist-cover) | Get the current image associated with a specific playlist. | False |  | GET | /playlists/{playlist_id}/images |
| [Add Custom Playlist Cover Image](https://developer.spotify.com/documentation/web-api/reference/upload-custom-playlist-cover) | Replace the image used to represent a specific playlist. | False | ugc-image-upload, playlist-modify-public, playlist-modify-private | PUT | /playlists/{playlist_id}/images |


## Search
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Search for Item](https://developer.spotify.com/documentation/web-api/reference/search) | Get Spotify catalog information about albums, artists, playlists, tracks, shows, episodes or audiobooksthat match a keyword string. Audiobooks are only available within the US, UK, Canada, Ireland, New Zealand and Australia markets. | False |  | GET | /search |


## Shows
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Show](https://developer.spotify.com/documentation/web-api/reference/get-a-show) | Get Spotify catalog information for a single show identified by itsunique Spotify ID. | False | user-read-playback-position | GET | /shows/{id} |
| [Get Several Shows](https://developer.spotify.com/documentation/web-api/reference/get-multiple-shows) | Get Spotify catalog information for several shows based on their Spotify IDs. | True |  | GET | /shows |
| [Get Show Episodes](https://developer.spotify.com/documentation/web-api/reference/get-a-shows-episodes) | Get Spotify catalog information about an show’s episodes. Optional parameters can be used to limit the number of episodes returned. | False | user-read-playback-position | GET | /shows/{id}/episodes |
| [Get User's Saved Shows](https://developer.spotify.com/documentation/web-api/reference/get-users-saved-shows) | Get a list of shows saved in the current Spotify user's library. Optional parameters can be used to limit the number of shows returned. | False | user-library-read | GET | /me/shows |
| [Save Shows for Current User](https://developer.spotify.com/documentation/web-api/reference/save-shows-user) | Save one or more shows to current Spotify user's library. | True | user-library-modify | PUT | /me/shows |
| [Remove User's Saved Shows](https://developer.spotify.com/documentation/web-api/reference/remove-shows-user) | Delete one or more shows from current Spotify user's library. | True | user-library-modify | DELETE | /me/shows |
| [Check User's Saved Shows](https://developer.spotify.com/documentation/web-api/reference/check-users-saved-shows) | Check if one or more shows is already saved in the current Spotify user's library. | True | user-library-read | GET | /me/shows/contains |


## Tracks
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Track](https://developer.spotify.com/documentation/web-api/reference/get-track) | Get Spotify catalog information for a single track identified by itsunique Spotify ID. | False |  | GET | /tracks/{id} |
| [Get Several Tracks](https://developer.spotify.com/documentation/web-api/reference/get-several-tracks) | Get Spotify catalog information for multiple tracks based on their Spotify IDs. | True |  | GET | /tracks |
| [Get User's Saved Tracks](https://developer.spotify.com/documentation/web-api/reference/get-users-saved-tracks) | Get a list of the songs saved in the current Spotify user's 'Your Music' library. | False | user-library-read | GET | /me/tracks |
| [Save Tracks for Current User](https://developer.spotify.com/documentation/web-api/reference/save-tracks-user) | Save one or more tracks to the current user's 'Your Music' library. | True | user-library-modify | PUT | /me/tracks |
| [Remove User's Saved Tracks](https://developer.spotify.com/documentation/web-api/reference/remove-tracks-user) | Remove one or more tracks from the current user's 'Your Music' library. | True | user-library-modify | DELETE | /me/tracks |
| [Check User's Saved Tracks](https://developer.spotify.com/documentation/web-api/reference/check-users-saved-tracks) | Check if one or more tracks is already saved in the current Spotify user's 'Your Music' library. | True | user-library-read | GET | /me/tracks/contains |
| [Get Several Tracks' Audio Features](https://developer.spotify.com/documentation/web-api/reference/get-several-audio-features) | Get audio features for multiple tracks based on their Spotify IDs. | True |  | GET | /audio-features |
| [Get Track's Audio Features](https://developer.spotify.com/documentation/web-api/reference/get-audio-features) | Get audio feature information for a single track identified by its uniqueSpotify ID. | True |  | GET | /audio-features/{id} |
| [Get Track's Audio Analysis](https://developer.spotify.com/documentation/web-api/reference/get-audio-analysis) | Get a low-level audio analysis for a track in the Spotify catalog. The audio analysis describes the track’s structure and musical content, including rhythm, pitch, and timbre. | True |  | GET | /audio-analysis/{id} |
| [Get Recommendations](https://developer.spotify.com/documentation/web-api/reference/get-recommendations) | Recommendations are generated based on the available information for a given seed entity and matched against similar artists and tracks. If there is sufficient information about the provided seeds, a list of tracks will be returned together with pool size details. | True |  | GET | /recommendations |


## Users
> Comprehensive reference of all documented Spotify Web API endpoints for the EC.Spotify project.

| Method | Description | Deprecated | Permissions | Http Method | API URL |
|--------|-------------|------------|-------------|-------------|---------|
| [Get Current User's Profile](https://developer.spotify.com/documentation/web-api/reference/get-current-users-profile) | Get detailed profile information about the current user (including thecurrent user's username). | False | user-read-private, user-read-email | GET | /me |
| [Get User's Top Items](https://developer.spotify.com/documentation/web-api/reference/get-users-top-artists-and-tracks) | Get the current user's top artists or tracks based on calculated affinity. | False | user-top-read | GET | /me/top/{type} |
| [Get User's Profile](https://developer.spotify.com/documentation/web-api/reference/get-users-profile) | Get public profile information about a Spotify user. | True |  | GET | /users/{user_id} |
| [Follow Playlist](https://developer.spotify.com/documentation/web-api/reference/follow-playlist) | Add the current user as a follower of a playlist. | True | playlist-modify-public, playlist-modify-private | PUT | /playlists/{playlist_id}/followers |
| [Unfollow Playlist](https://developer.spotify.com/documentation/web-api/reference/unfollow-playlist) | Remove the current user as a follower of a playlist. | True | playlist-modify-public, playlist-modify-private | DELETE | /playlists/{playlist_id}/followers |
| [Get Followed Artists](https://developer.spotify.com/documentation/web-api/reference/get-followed) | Get the current user's followed artists. | False | user-follow-read | GET | /me/following |
| [Follow Artists or Users](https://developer.spotify.com/documentation/web-api/reference/follow-artists-users) | Add the current user as a follower of one or more artists or other Spotify users. | True | user-follow-modify | PUT | /me/following |
| [Unfollow Artists or Users](https://developer.spotify.com/documentation/web-api/reference/unfollow-artists-users) | Remove the current user as a follower of one or more artists or other Spotify users. | True | user-follow-modify | DELETE | /me/following |
| [Check If User Follows Artists or Users](https://developer.spotify.com/documentation/web-api/reference/check-current-user-follows) | Check to see if the current user is following one or more artists or other Spotify users. | True | user-follow-read | GET | /me/following/contains |
| [Check if Current User Follows Playlist](https://developer.spotify.com/documentation/web-api/reference/check-if-user-follows-playlist) | Check to see if the current user is following a specified playlist. | True |  | GET | /playlists/{playlist_id}/followers/contains |


***
***
*Generated 96 Spotify API method details*
*Source: [Spotify Web API Documentation](https://developer.spotify.com/documentation/web-api)*

