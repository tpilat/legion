using Legion.ADF.Auth.Settings;
using Legion.Caching;
using Legion.Database;
using Legion.Extensions;
using Legion.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore :
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
	IProtectedUserStore<Model.User>
{
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory? _connectionProviderFactory;
	protected readonly IdentityStoreOptions _options;
	protected readonly ILogger _logger;
	private readonly Lazy<IADFCache?> _cache;
	private readonly Lazy<List<string>> _removeAllCacheTags;

	private readonly Lazy<IAuthAccessControlManager?> _accessControlManager;

	private bool _disposed;

	protected IAuthUnitOfWork UoW { get; private set; }
	protected IConnectionProvider? ConnectionProvider { get; private set; }
	protected bool IsInternalConnectionProvider { get; private set; }
	public bool AutoSaveChanges { get; set; } = true;
	public IAuthAccessControlManager? AccessControlManager => _accessControlManager.Value;
	public IADFCache? Cache => _cache.Value;

	public IdentityUserOnlyStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<IdentityStoreOptions> options,
		ILogger<IdentityUserOnlyStore> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity");

		CreateUnitOfWork(scopeContext);

		_accessControlManager = new(() => UoW!.ServiceProvider.GetService<IAuthAccessControlManager>());
		_cache = new(() => UoW!.ServiceProvider.GetService<IADFCache>());
		_removeAllCacheTags = new(() =>
		[
			typeof(Model.ExternalLogin).FullName,
			typeof(Model.LoginProvider).FullName,
			typeof(Model.Permission).FullName,
			typeof(Model.Role).FullName,
			typeof(Model.RolePermission).FullName,
			typeof(Model.User).FullName,
			typeof(Model.UserPermission).FullName,
			typeof(Model.UserRole).FullName,
			typeof(Model.UserToken).FullName,
		]);
	}

	protected void CreateUnitOfWork(IScopeContext scopeContext)
	{
		IsInternalConnectionProvider = true;
		ConnectionProvider = _connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			_serviceProvider!,
			_options.IdentityStoreId,
			transactionIsolationLevel: null,
			false,
			_options.EnableAuditing);

		var authUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(scopeContext);

		if (authUowResult.HasError)
			authUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.AuthUnitOfWorkException.InvalidUoW, true);

		UoW = authUowResult.Data!;
	}

	public IdentityUserOnlyStore(
		IConnectionProvider connectionProvider,
		IOptions<IdentityStoreOptions> options,
		ILogger<IdentityUserOnlyStore> logger)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity");

		ConnectionProvider = connectionProvider;
		_serviceProvider = ConnectionProvider.ServiceProvider;

		IsInternalConnectionProvider = false;
		var authUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(scopeContext);

		if (authUowResult.HasError)
			authUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.AuthUnitOfWorkException.InvalidUoW, true);

		UoW = authUowResult.Data!;

		_accessControlManager = new(() => UoW.ServiceProvider.GetService<IAuthAccessControlManager>());
		_cache = new(() => UoW.ServiceProvider.GetService<IADFCache>());
		_removeAllCacheTags = new(() =>
		[
			typeof(Model.ExternalLogin).FullName,
			typeof(Model.LoginProvider).FullName,
			typeof(Model.Permission).FullName,
			typeof(Model.Role).FullName,
			typeof(Model.RolePermission).FullName,
			typeof(Model.User).FullName,
			typeof(Model.UserPermission).FullName,
			typeof(Model.UserRole).FullName,
			typeof(Model.UserToken).FullName,
		]);
	}

	protected async Task Save(IScopeContext scopeContext, CancellationToken cancellationToken = default)
	{
		if (AutoSaveChanges)
		{
			var result = await UoW.SaveAsync(scopeContext, cancellationToken);
			result.ThrowIfError(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserStoreException.Default, true);

			if (0 < _removeAllCacheTags.Value.Count)
			{
				if (Cache != null)
				{
					foreach (var tag in _removeAllCacheTags.Value)
						Cache.RemoveValuesForTag(tag);
				}

				var reloadableCacheKeyStore = UoW.ConnectionProvider.GetOrCreateReloadableCacheKeyStore();
				if (reloadableCacheKeyStore != null)
				{
					foreach (var tag in _removeAllCacheTags.Value)
						await reloadableCacheKeyStore.SaveReloadableCacheKeyAsync(scopeContext, key: null, [tag], reloadAtUtc: null, checkPermissions: false, cancellationToken);
				}
			}

			if (IsInternalConnectionProvider)
			{
				var commitResult = await ConnectionProvider!.TransactionsController!.CommitAllAsync(
					scopeContext,
					TransactionsControllerStatus.NotIdle,
					cancellationToken: default);

				await ConnectionProvider!.DisposeAsync();

				commitResult.ThrowIfError(scopeContext, null, true);

				CreateUnitOfWork(scopeContext);
				//ConnectionProvider.ReCreateTransaction(scopeContext);
			}
		}
	}

	public async Task<IdentityResult> CreateAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		UoW.UserRepository.Add(scopeContext, user);

		await Save(scopeContext, cancellationToken);

		return IdentityResult.Success;
	}

	public async Task<IdentityResult> UpdateAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		var dbUser = await UoW.UserRepository
			.GetUserById(
				new Queries.User.GetValidUserByIdQuery(user.IdUser, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (dbUser == null)
			Throw.InvalidOperationException($"{nameof(dbUser)} == null", scopeContext);

		user.MapTo(dbUser);

		dbUser.SetNewConcurrencyToken();

		//TODO
		//try
		//{
			await Save(scopeContext, cancellationToken);
		//}
		//catch (DbUpdateConcurrencyException)
		//{
		//	return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure()); //Optimistic concurrency failure, object has been modified.
		//}

		return IdentityResult.Success;
	}

	public async Task<IdentityResult> DeleteAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		var dbUser = await UoW.UserRepository
			.GetUserById(
				new Queries.User.GetValidUserByIdQuery(user.IdUser, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (dbUser == null)
			Throw.InvalidOperationException($"{nameof(dbUser)} == null", scopeContext);

		dbUser.SetSoftDelete(scopeContext);

		//TODO
		//try
		//{
			await Save(scopeContext, cancellationToken);
		//}
		//catch (DbUpdateConcurrencyException)
		//{
		//	return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure()); //Optimistic concurrency failure, object has been modified.
		//}

		return IdentityResult.Success;
	}

	public async Task<Model.User?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		var id = userId.ConvertTo<Guid>();

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(userId), userId);

		var dbUser = await UoW.UserRepository
			.GetUserById(
				new Queries.User.GetValidUserByIdQuery(id, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbUser;
	}

	protected async Task<Model.User?> FindUserAsync(Guid userId, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(userId), userId.ToString());

		var dbUser = await UoW.UserRepository
			.GetUserById(
				new Queries.User.GetValidUserByIdQuery(userId, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbUser;
	}

	protected async Task<Model.ExternalLogin?> FindUserLoginAsync(IScopeContext scopeContext, Guid userId, string loginProvider, string externalUserIdentifier, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		scopeContext = ScopeContext.Create(scopeContext)
			.AddContextProperty(nameof(userId), userId.ToString())
			.AddContextProperty(nameof(loginProvider), loginProvider)
			.AddContextProperty(nameof(externalUserIdentifier), externalUserIdentifier);

		var dbExternalLogin = await UoW.ExternalLoginRepository
			.GetExternalLoginByUserAndExternalIdentifier(
				new Queries.ExternalLogin.GetValidExternalLoginByUserAndExternalIdentifierQuery(userId, loginProvider, externalUserIdentifier, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbExternalLogin;
	}

	protected async Task<Model.ExternalLogin?> FindUserLoginAsync(IScopeContext scopeContext, string loginProvider, string externalUserIdentifier, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		scopeContext = ScopeContext.Create(scopeContext)
			.AddContextProperty(nameof(loginProvider), loginProvider)
			.AddContextProperty(nameof(externalUserIdentifier), externalUserIdentifier);

		var dbExternalLogin = await UoW.ExternalLoginRepository
			.GetExternalLoginByExternalIdentifier(
				new Queries.ExternalLogin.GetValidExternalLoginByExternalIdentifierQuery(loginProvider, externalUserIdentifier, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbExternalLogin;
	}

	public async Task<Model.User?> FindByNameAsync(string normalizedLogin, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(normalizedLogin), normalizedLogin);

		var dbUser = await UoW.UserRepository
			.GetUserByNormalizedLogin(
				new Queries.User.GetValidUserByNormalizedLoginQuery(normalizedLogin, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbUser;
	}

	public Task<string?> GetNormalizedUserNameAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		//var keyRing = _services.GetRequiredService<ILookupProtectorKeyRing>();
		//var protector = _services.GetRequiredService<ILookupProtector>();
		//return protector.Protect(keyRing.CurrentKeyId, data);

		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		return Task.FromResult(user.NormalizedLogin);
	}

	public Task<string> GetUserIdAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		return Task.FromResult(object.Equals(user.IdUser, default(Guid))
			? null!
			: user.IdUser.ToString()!);
	}

	public Task<string?> GetUserNameAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		return Task.FromResult(user.Login);
	}

	public Task SetNormalizedUserNameAsync(Model.User user, string? normalizedLogin, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(normalizedLogin), normalizedLogin);

		var result = user.SetNormalizedLogin(scopeContext, normalizedLogin!);

		if (result.HasError)
			result.ThrowIfError(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserException.InvalidNormalizedLogin, true);

		return Task.CompletedTask;
	}

	public Task SetUserNameAsync(Model.User user, string? login, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(login), login);

		var result = user.SetLogin(scopeContext, login!);

		if (result.HasError)
			result.ThrowIfError(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserException.InvalidLogin, true);

		return Task.CompletedTask;
	}

	protected void ThrowIfDisposed(
		IScopeContext? scopeContext = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		scopeContext = scopeContext?.CreateNew(true, memberName, sourceFilePath, sourceLineNumber);

		if (_disposed)
			Throw.ObjectDisposedException(this.GetType().ToFriendlyFullName(), scopeContext);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
			if (UoW != null)
			{
				if (IsInternalConnectionProvider)
				{
					var scopeContext = ScopeContext.Create($"{this.GetType().FullName} {nameof(Dispose)}");

					var commitResult = ConnectionProvider!.TransactionsController!.CommitAll(
						scopeContext,
						TransactionsControllerStatus.None);

					ConnectionProvider!.Dispose();
				}

				UoW?.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
