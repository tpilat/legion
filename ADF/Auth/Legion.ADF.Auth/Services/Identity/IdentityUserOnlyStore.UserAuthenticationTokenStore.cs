using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserAuthenticationTokenStore<Model.User>
{
	public async Task<string?> GetTokenAsync(Model.User user, string loginProvider, string name, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var userToken = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);
		return userToken?.Value;
	}

	public async Task RemoveTokenAsync(Model.User user, string loginProvider, string name, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var userToken = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);

		if (userToken != null)
			await RemoveUserTokenAsync(userToken).ConfigureAwait(false);
	}

	public async Task SetTokenAsync(Model.User user, string loginProvider, string name, string? value, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);
		Legion.Throw.IfArgumentNullOrWhiteSpace(loginProvider);
		Legion.Throw.IfArgumentNullOrWhiteSpace(name);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(loginProvider), loginProvider)
			.AddContextProperty(nameof(name), name)
			.AddContextProperty(nameof(value), value);

		var userToken = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);
		if (userToken == null)
		{
			var dbLoginProvider = await UoW.LoginProviderRepository
				.GetLoginProviderByName(
					new Queries.LoginProvider.GetValidLoginProviderByNameQuery(loginProvider, CheckReadPermissions: false, AsNoTracking: true))
				.ToResultAsync(scopeContext, cancellationToken);

			if (dbLoginProvider == null)
				Throw.InvalidOperationException($"{nameof(dbLoginProvider)} == null", scopeContext);

			var userTokenResult =
				Model.UserToken.CreateUserToken(
					scopeContext,
					dbLoginProvider.IdLoginProvider,
					user.IdUser,
					name,
					value,
					null,
					null,
					null);

			if (userTokenResult.HasError)
				userTokenResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserTokenException.InvalidUserToken, true);

			UoW.UserTokenRepository.Add(scopeContext, userTokenResult.Data!);
		}
		else
		{
			var result = userToken.SetValue(scopeContext, value!);

			if (result.HasError)
				result.ThrowIfError(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserTokenException.InvalidUserToken, true);
		}
	}

	protected async Task<Model.UserToken?> FindTokenAsync(Model.User user, string loginProvider, string name, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(loginProvider), loginProvider)
			.AddContextProperty(nameof(name), name);

		var dbUserToken = await UoW.UserTokenRepository
			.GetUserTokenByUserProviderTokenName(
				new Queries.UserToken.GetValidUserTokenByUserProviderTokenNameQuery(user.IdUser, loginProvider, name, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbUserToken;
	}

	protected Task AddUserTokenAsync(Model.UserToken token, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(token);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(token.IdUserToken), token.IdUserToken.ToString());

		UoW.UserTokenRepository.Add(scopeContext, token);

		return Task.CompletedTask;
	}

	protected Task RemoveUserTokenAsync(Model.UserToken token, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(token);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(token.IdUserToken), token.IdUserToken.ToString());

		UoW.UserTokenRepository.Remove(scopeContext, token);

		return Task.CompletedTask;
	}
}
