using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserAuthenticatorKeyStore<Model.User>
{
	private const string InternalLoginProvider = "AuthenticatorApp";
	private const string AuthenticatorKeyTokenName = "AuthenticatorKey";

	public Task<string?> GetAuthenticatorKeyAsync(Model.User user, CancellationToken cancellationToken = default)
		=> GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

	public Task SetAuthenticatorKeyAsync(Model.User user, string key, CancellationToken cancellationToken = default)
		=> SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
}
