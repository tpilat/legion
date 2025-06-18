using Legion.ADF.Cache.Settings;
using Legion.Caching;
using Legion.Database;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Cache.Services;

public partial class ReloadableCacheKeyStore : IReloadableCacheKeyStore, IDisposable, IAsyncDisposable
{
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory? _connectionProviderFactory;
	protected readonly ReloadableCacheKeyStoreOptions _options;
	protected readonly ILogger _logger;

	private readonly Lazy<ICacheAccessControlManager?> _accessControlManager;

	private bool _disposed;

	protected ICacheUnitOfWork UoW { get; private set; }
	protected IConnectionProvider? ConnectionProvider { get; private set; }
	protected bool IsInternalConnectionProvider { get; private set; }
	public bool AutoSaveChanges { get; set; } = true;
	public ICacheAccessControlManager? AccessControlManager => _accessControlManager.Value;

	public ReloadableCacheKeyStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<ReloadableCacheKeyStoreOptions> options,
		ILogger<ReloadableCacheKeyStore> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Cache");

		CreateUnitOfWork(scopeContext);

		_accessControlManager = new(() => UoW!.ServiceProvider.GetService<ICacheAccessControlManager>());
	}

	protected void CreateUnitOfWork(IScopeContext scopeContext)
	{
		IsInternalConnectionProvider = true;
		ConnectionProvider = _connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			_serviceProvider!,
			_options.CacheStoreId,
			transactionIsolationLevel: null,
			false,
			false);

		var cacheUowResult = ConnectionProvider.UnitOfWorkProvider.Create<ICacheUnitOfWork>(scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Cache.Exceptions.Internal.ErrorCodes.CacheUnitOfWorkException.InvalidUoW, true);

		UoW = cacheUowResult.Data!;
	}

	public ReloadableCacheKeyStore(
		IConnectionProvider connectionProvider,
		IOptions<ReloadableCacheKeyStoreOptions> options,
		ILogger<ReloadableCacheKeyStore> logger)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Cache");

		ConnectionProvider = connectionProvider;
		_serviceProvider = ConnectionProvider.ServiceProvider;

		IsInternalConnectionProvider = false;
		var cacheUowResult = ConnectionProvider.UnitOfWorkProvider.Create<ICacheUnitOfWork>(scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Cache.Exceptions.Internal.ErrorCodes.CacheUnitOfWorkException.InvalidUoW, true);

		UoW = cacheUowResult.Data!;

		_accessControlManager = new(() => UoW.ServiceProvider.GetService<ICacheAccessControlManager>());
	}

	public async Task<IResult> SaveAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default)
		=> await SaveInternalAsync(scopeContext, true, cancellationToken).ConfigureAwait(false);

	public IResult Save(IScopeContext scopeContext)
		=> SaveInternal(scopeContext, true);

	protected async Task<IResult> SaveInternalAsync(IScopeContext scopeContext, bool force, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		if (force || AutoSaveChanges)
		{
			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken);
			if (result.MergeHasError(saveResult))
				return result.Build();

			if (IsInternalConnectionProvider)
			{
				var commitResult = await ConnectionProvider!.TransactionsController!.CommitAllAsync(
					scopeContext,
					TransactionsControllerStatus.NotIdle,
					cancellationToken: default);

				await ConnectionProvider!.DisposeAsync();

				if (result.MergeHasError(commitResult))
					return result.Build();

				CreateUnitOfWork(scopeContext);
				//ConnectionProvider.ReCreateTransaction(scopeContext);
			}
		}

		return result.Build();
	}

	protected IResult SaveInternal(IScopeContext scopeContext, bool force)
	{
		var result = new ResultBuilder();

		if (force || AutoSaveChanges)
		{
			var saveResult = UoW.Save(scopeContext);
			if (result.MergeHasError(saveResult))
				return result.Build();

			if (IsInternalConnectionProvider)
			{
				var commitResult = ConnectionProvider!.TransactionsController!.CommitAll(
					scopeContext,
					TransactionsControllerStatus.NotIdle);

				ConnectionProvider!.Dispose();

				if (result.MergeHasError(commitResult))
					return result.Build();

				CreateUnitOfWork(scopeContext);
				//ConnectionProvider.ReCreateTransaction(scopeContext);
			}
		}

		return result.Build();
	}

	public async ValueTask DisposeAsync()
	{
		if (_disposed)
			return;

		_disposed = true;

		await DisposeAsyncCoreAsync().ConfigureAwait(false);

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

	private async ValueTask DisposeAsyncCoreAsync()
	{
		if (UoW != null)
		{
			if (IsInternalConnectionProvider)
			{
				var scopeContext = ScopeContext.Create($"{this.GetType().FullName} {nameof(DisposeAsyncCoreAsync)}");

				var commitResult = await ConnectionProvider!.TransactionsController!.CommitAllAsync(
					scopeContext,
					TransactionsControllerStatus.None,
					cancellationToken: default);

				await ConnectionProvider!.DisposeAsync();
			}

			await UoW.DisposeAsync();
		}
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
