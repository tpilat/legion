using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserLoginStore<Model.User>
{
	public async Task AddLoginAsync(Model.User user, UserLoginInfo login, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);
		Legion.Throw.IfArgumentNull(login);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(login.LoginProvider), login.LoginProvider)
			.AddContextProperty(nameof(login.ProviderKey), login.ProviderKey);

		var dbLoginProvider = await UoW.LoginProviderRepository
			.GetLoginProviderByName(
				new Queries.LoginProvider.GetValidLoginProviderByNameQuery(login.LoginProvider, CheckReadPermissions: false, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		if (dbLoginProvider == null)
			Throw.InvalidOperationException($"{nameof(dbLoginProvider)} == null", scopeContext);

		var userTokenResult =
			Model.ExternalLogin.CreateExternalLogin(
				scopeContext,
				dbLoginProvider.IdLoginProvider,
				user.IdUser,
				login.ProviderKey,
				null,
				null,
				null);

		if (userTokenResult.HasError)
			userTokenResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.ExternalLoginException.InvalidExternalLogin, true);

		UoW.ExternalLoginRepository.Add(scopeContext, userTokenResult.Data!);
	}

	public async Task<Model.User?> FindByLoginAsync(string loginProvider, string externalUserIdentifier, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(loginProvider), loginProvider)
			.AddContextProperty(nameof(externalUserIdentifier), externalUserIdentifier);

		return await UoW.UserRepository
			.GetUserByExternalLoginProviderIdentifier(
				new Queries.User.GetValidUserByExternalLoginProviderIdentifierQuery(loginProvider, externalUserIdentifier, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	public async Task<IList<UserLoginInfo>> GetLoginsAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		var externalLogins = await UoW.ExternalLoginRepository
			.GetExternalLoginsByUserId(
				new Queries.ExternalLogin.GetValidExternalLoginsByUserIdQuery(user.IdUser, CheckReadPermissions: false, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return externalLogins
			.Select(el => new UserLoginInfo(el.LoginProvider.Name, el.ExternalUserIdentifier, el.LoginProvider.Name))
			.ToList();
	}

	public async Task RemoveLoginAsync(Model.User user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(loginProvider), loginProvider)
			.AddContextProperty(nameof(providerKey), providerKey);

		var externalLogin = await FindUserLoginAsync(scopeContext, user.IdUser, loginProvider, providerKey, cancellationToken);
		if (externalLogin != null)
			UoW.ExternalLoginRepository.Remove(scopeContext, externalLogin);
	}
}
