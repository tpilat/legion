using Legion.ADF.Config.Settings;
using Legion.Caching;
using Legion.Database;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Config.Services;

public partial class ConfigStore : IDisposable, IAsyncDisposable
{
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory? _connectionProviderFactory;
	protected readonly ConfigStoreOptions _options;
	protected readonly ILogger _logger;
	private readonly Lazy<IADFCache?> _cache;

	private readonly Lazy<IConfigAccessControlManager?> _accessControlManager;

	private bool _disposed;

	protected IConfigUnitOfWork UoW { get; private set; }
	protected IConnectionProvider? ConnectionProvider { get; private set; }
	protected bool IsInternalConnectionProvider { get; private set; }
	public bool AutoSaveChanges { get; set; } = true;
	public IConfigAccessControlManager? AccessControlManager => _accessControlManager.Value;
	public IADFCache? Cache => _cache.Value;

	public ConfigStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<ConfigStoreOptions> options,
		ILogger<ConfigStore> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Config");

		CreateUnitOfWork(scopeContext);

		_accessControlManager = new(() => UoW!.ServiceProvider.GetService<IConfigAccessControlManager>());
		_cache = new(() => UoW!.ServiceProvider.GetService<IADFCache>());
	}

	protected void CreateUnitOfWork(IScopeContext scopeContext)
	{
		IsInternalConnectionProvider = true;
		ConnectionProvider = _connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			_serviceProvider!,
			_options.ConfigStoreId,
			transactionIsolationLevel: null,
			false,
			_options.EnableAuditing);

		var configUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IConfigUnitOfWork>(scopeContext);

		if (configUowResult.HasError)
			configUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Config.Exceptions.Internal.ErrorCodes.ConfigUnitOfWorkException.InvalidUoW, true);

		UoW = configUowResult.Data!;
	}

	public ConfigStore(
		IConnectionProvider connectionProvider,
		IOptions<ConfigStoreOptions> options,
		ILogger<ConfigStore> logger)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Config");

		ConnectionProvider = connectionProvider;
		_serviceProvider = ConnectionProvider.ServiceProvider;

		IsInternalConnectionProvider = false;
		var configUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IConfigUnitOfWork>(scopeContext);

		if (configUowResult.HasError)
			configUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Config.Exceptions.Internal.ErrorCodes.ConfigUnitOfWorkException.InvalidUoW, true);

		UoW = configUowResult.Data!;

		_accessControlManager = new(() => UoW.ServiceProvider.GetService<IConfigAccessControlManager>());
		_cache = new(() => UoW.ServiceProvider.GetService<IADFCache>());
	}

	public async Task<IResult> SaveAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default)
		=> await SaveInternalAsync(scopeContext, true, [typeof(Model.ConfigurationClass).FullName, typeof(Model.ConfigurationKeyValue).FullName], cancellationToken);

	public IResult Save(IScopeContext scopeContext)
		=> SaveInternal(scopeContext, true, [typeof(Model.ConfigurationClass).FullName, typeof(Model.ConfigurationKeyValue).FullName]);

	protected async Task<IResult> SaveInternalAsync(IScopeContext scopeContext, bool save, List<string>? removeAllCacheTags, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		if (save)
		{
			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken);
			if (result.MergeHasError(saveResult))
				return result.Build();

			if (0 < removeAllCacheTags?.Count)
			{
				if (Cache != null)
				{
					foreach (var tag in removeAllCacheTags)
						Cache.RemoveValuesForTag(tag);
				}

				var reloadableCacheKeyStore = UoW.ConnectionProvider.GetOrCreateReloadableCacheKeyStore();
				if (reloadableCacheKeyStore != null)
				{
					foreach (var tag in removeAllCacheTags)
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

				if (result.MergeHasError(commitResult))
					return result.Build();

				CreateUnitOfWork(scopeContext);
				//ConnectionProvider.ReCreateTransaction(scopeContext);
			}
		}

		return result.Build();
	}

	protected IResult SaveInternal(IScopeContext scopeContext, bool save, List<string>? removeAllCacheTags)
	{
		var result = new ResultBuilder();

		if (save)
		{
			var saveResult = UoW.Save(scopeContext);
			if (result.MergeHasError(saveResult))
				return result.Build();

			if (0 < removeAllCacheTags?.Count)
			{
				if (Cache != null)
				{
					foreach (var tag in removeAllCacheTags)
						Cache.RemoveValuesForTag(tag);
				}

				var reloadableCacheKeyStore = UoW.ConnectionProvider.GetOrCreateReloadableCacheKeyStore();
				if (reloadableCacheKeyStore != null)
				{
					foreach (var tag in removeAllCacheTags)
						reloadableCacheKeyStore.SaveReloadableCacheKey(scopeContext, key: null, [tag], reloadAtUtc: null, checkPermissions: false);
				}
			}

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
