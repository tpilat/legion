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

public partial class IdentityRoleStore :
	IRoleStore<Model.Role>,
	IQueryableRoleStore<Model.Role>,
	IRoleClaimStore<Model.Role>
{
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory? _connectionProviderFactory;
	private readonly IdentityStoreOptions _options;
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

	public IdentityRoleStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<IdentityStoreOptions> options,
		ILogger<IdentityRoleStore> logger)
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
			false);

		var authUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(scopeContext);

		if (authUowResult.HasError)
			authUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.AuthUnitOfWorkException.InvalidUoW, true);

		UoW = authUowResult.Data!;
	}

	public IdentityRoleStore(
		IConnectionProvider connectionProvider,
		IOptions<IdentityStoreOptions> options,
		ILogger<IdentityRoleStore> logger)
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
			result.ThrowIfError(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.RoleStoreException.Default, true);

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

	public async Task<IdentityResult> CreateAsync(Model.Role role, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString());

		UoW.RoleRepository.Add(scopeContext, role);

		await Save(scopeContext, cancellationToken);

		return IdentityResult.Success;
	}

	public async Task<IdentityResult> UpdateAsync(Model.Role role, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString());

		var dbRole = await UoW.RoleRepository
			.GetRoleById(
				new Queries.Role.GetValidRoleByIdQuery(role.IdRole, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (dbRole == null)
			Throw.InvalidOperationException($"{nameof(dbRole)} == null", scopeContext);

		role.MapTo(dbRole);

		dbRole.SetNewConcurrencyToken();

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

	public async Task<IdentityResult> DeleteAsync(Model.Role role, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString());

		var dbRole = await UoW.RoleRepository
			.GetRoleById(
				new Queries.Role.GetValidRoleByIdQuery(role.IdRole, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (dbRole == null)
			Throw.InvalidOperationException($"{nameof(dbRole)} == null", scopeContext);

		dbRole.SetSoftDelete(scopeContext);

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

	public Task<string> GetRoleIdAsync(Model.Role role, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		return Task.FromResult(object.Equals(role.IdRole, default(Guid))
			? null!
			: role.IdRole.ToString()!);
	}

	public Task<string?> GetRoleNameAsync(Model.Role role, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		return Task.FromResult(role.Name)!;
	}

	public Task SetRoleNameAsync(Model.Role role, string? roleName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString())
			.AddContextProperty(nameof(role.Name), role.Name);

		var result = role.SetName(scopeContext, roleName!);

		if (result.HasError)
			result.ThrowIfError(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.RoleException.InvalidRoleName, true);

		return Task.CompletedTask;
	}

	public async Task<Model.Role?> FindByIdAsync(string roleId, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		var id = roleId.ConvertTo<Guid>();

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(roleId), roleId);

		var dbRole = await UoW.RoleRepository
			.GetRoleById(
				new Queries.Role.GetValidRoleByIdQuery(id, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbRole;
	}

	public async Task<Model.Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(normalizedRoleName), normalizedRoleName);

		var dbRole = await UoW.RoleRepository
			.GetRoleByNormalizedName(
				new Queries.Role.GetValidRoleByNormalizedNameQuery(normalizedRoleName, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		return dbRole;
	}

	public Task<string?> GetNormalizedRoleNameAsync(Model.Role role, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		return Task.FromResult(role.NormalizedName)!;
	}

	public Task SetNormalizedRoleNameAsync(Model.Role role, string? normalizedName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString())
			.AddContextProperty(nameof(normalizedName), normalizedName);

		var result = role.SetNormalizedName(scopeContext, normalizedName!);

		if (result.HasError)
			result.ThrowIfError(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.RoleException.InvalidRoleNormalizedName, true);

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
