using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface ISocialAuthService
{
   /// <summary>
   /// Asynchronously authenticates a user based on the provided authentication request.
   /// </summary>
   /// <param name="request">The authentication request containing the necessary details to perform user authentication.</param>
   /// <returns>
   /// A <see cref="Result{AuthPayload}"/> instance containing the authentication payload if the operation succeeds,
   /// or relevant error details if it fails.
   /// </returns>
   Task<Result<AuthPayload>> AuthenticateUserAsync(AuthRequest request);
}