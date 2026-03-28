using EC.Spotify.Models.Auth;

namespace EC.Spotify.Abstractions.Services;

public interface IAuthorizationService
{
    /// <summary>
    /// Validates the current authentication state and determines whether user authorization is required.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the validation operation.</param>
    /// <returns>A URL as a string that directs the user to authorize the application if authorization is required; otherwise,
    /// <see langword="null"/> if the user is already authorized.</returns>
    Task<string?> Validate(CancellationToken cancellationToken = default);
    /// <summary>
    /// Generates the URL to initiate the OAuth 2.0 authorization code flow.
    /// </summary>
    /// <remarks>Use the returned URL to redirect the user agent to the authorization server, where the user
    /// can grant access to the application. The URL includes all required query parameters for the authorization code
    /// flow.</remarks>
    /// <returns>A string containing the authorization endpoint URL that can be used to redirect users for authentication and
    /// consent.</returns>
    string? AuthorizationCodeUrl();
    /// <summary>
    /// Asynchronously adds a new authorization code to the underlying store.
    /// </summary>
    /// <param name="authorizationCode">The authorization code to add. Can be null or empty if the implementation allows; otherwise, must be a non-empty
    /// string.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
    /// authorization code was added successfully; otherwise, <see langword="false"/>.</returns>
    /// 
    Task<bool> AuthorizationCodeAddAsync(string? authorizationCode, string? state = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Asynchronously retrieves the current authorization code, if available.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the authorization code as a string,
    /// or null if no code is available.</returns>
    Task<string?> AuthorizationCodeGetAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Asynchronously removes the current authorization code from the underlying store.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
    /// authorization code was successfully removed; otherwise, <see langword="false"/>.</returns>
    Task<bool> AuthorizationCodeRemoveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the current authentication token, if available.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the authentication token if one is
    /// available; otherwise, null.</returns>
    Task<AuthToken?> AuthorizationTokenGetAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Attempts to reset the current authentication token asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
    /// authentication token was successfully reset; otherwise, <see langword="false"/>.</returns>
    Task<bool> AuthorizationTokenReset();
}