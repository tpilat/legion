using Legion.ADF.Auth.Settings;
using Legion.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserRoleStore : IdentityUserOnlyStore,
	IUserStore<Model.User>,
	IUserLoginStore<Model.User>,
	IUserClaimStore<Model.User>,
	IUserPasswordStore<Model.User>,
	IUserSecurityStampStore<Model.User>,
	IUserEmailStore<Model.User>,
	IUserLockoutStore<Model.User>,
	IUserPhoneNumberStore<Model.User>,
	IQueryableUserStore<Model.User>,
	IUserAuthenticationTokenStore<Model.User>,
	IUserAuthenticatorKeyStore<Model.User>,
	IUserTwoFactorStore<Model.User>,
	IUserTwoFactorRecoveryCodeStore<Model.User>,
	IProtectedUserStore<Model.User>,
	IUserRoleStore<Model.User>
{
	public IdentityUserRoleStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<IdentityStoreOptions> options,
		ILogger<IdentityUserOnlyStore> logger)
		: base(serviceProvider, connectionProviderFactory, options, logger)
	{
	}

	public IdentityUserRoleStore(
		IConnectionProvider connectionProvider,
		IOptions<IdentityStoreOptions> options,
		ILogger<IdentityUserOnlyStore> logger)
		: base(connectionProvider, options, logger)
	{
	}
}
