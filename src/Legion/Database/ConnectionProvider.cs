using Legion.Caching;
using Legion.Model.Audit;
using Legion.Model.Messaging;
using Legion.Model.Repositories;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Legion.Database;

public abstract class ConnectionProvider : IConnectionProvider, IDisposable, IAsyncDisposable
{
	private readonly ConcurrentDictionary<IDisposable, bool> _disposables = [];
	private readonly Lazy<ILogger> _logger;

	protected readonly bool _isInternalTransaction;

	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdConnectionProvider { get; }
#endif

	public IServiceProvider ServiceProvider { get; }
	public ITransactionsController? TransactionsController { get; }
	public IUnitOfWorkProvider UnitOfWorkProvider { get; protected set; }
	public string? ConnectionString { get; protected set; }
	public DbConnection? DbConnection { get; protected set; }
	public bool WithTransaction { get; protected set; }
	public IsolationLevel? TransactionIsolationLevel { get; protected set; }
	public DbTransaction? ExternalDbTransaction { get; protected set; }

	public ILogger Logger => _logger.Value;

	public bool? AllowLocking { get; private set; }
	public bool CreateAuditEntryStore { get; }

	public IAuditEntryStore? AuditEntryStore { get; private set; }
	public IDomainEventStore? DomainEventStore { get; private set; }
	public IReloadableCacheKeyStore? ReloadableCacheKeyStore { get; private set; }

	protected ConnectionProvider(
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController,
		bool createInternalTransaction,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdConnectionProvider = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdConnectionProvider.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);

		ServiceProvider = serviceProvider;

		_logger = new(() => ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<ConnectionProvider>());

		if (transactionsController == null && createInternalTransaction)
		{
			TransactionsController = new TransactionsController();
			_isInternalTransaction = true;
		}
		else
		{
			TransactionsController = transactionsController;
			_isInternalTransaction = false;
		}

		AllowLocking = allowLocking;
		CreateAuditEntryStore = createAuditEntryStore;

		SetUnitOfWorkProvider();

		Throw.IfNull(UnitOfWorkProvider);

		if (CreateAuditEntryStore)
		{
			var auditEntryStoreFactory = ServiceProvider.GetService<IAuditEntryStoreFactory>();
			if (auditEntryStoreFactory != null)
				AuditEntryStore = auditEntryStoreFactory.Create(this);
		}

		var domainEventStoreFactory = ServiceProvider.GetService<IDomainEventStoreFactory>();
		if (domainEventStoreFactory != null)
			DomainEventStore = domainEventStoreFactory.Create(this);
	}

	public abstract DbConnection? GetDbConnection();

	public abstract DbConnection GetOrCreateNewDbConnection(out bool isNewConnection);

	public abstract DbConnection CreateNewDbConnection();

	private readonly object _lockGetOrCreateReloadableCacheKeyStore = new();
	public IReloadableCacheKeyStore? GetOrCreateReloadableCacheKeyStore()
	{
		if (ReloadableCacheKeyStore != null)
			return ReloadableCacheKeyStore;

		lock (_lockGetOrCreateReloadableCacheKeyStore)
		{
			if (ReloadableCacheKeyStore != null)
				return ReloadableCacheKeyStore;

			var reloadableCacheKeyStoreFactory = ServiceProvider.GetService<IReloadableCacheKeyStoreFactory>();
			if (reloadableCacheKeyStoreFactory != null)
				ReloadableCacheKeyStore = reloadableCacheKeyStoreFactory.Create(this);

			return ReloadableCacheKeyStore;
		}
	}

	protected abstract void SetUnitOfWorkProvider();

	public bool RegisterDisposable(IDisposable disposable)
		=> _disposables.TryAdd(disposable, false);

	public abstract bool ReCreateTransaction(IScopeContext scopeContext);

	public IResult<bool?> CommitAll(
		IScopeContext scopeContext,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		scopeContext = scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber);

		var result = new ResultBuilder<bool?>();

		if (TransactionsController == null)
			return result.WithData(false).Build();

		var commitResult = TransactionsController.CommitAll(
			scopeContext,
			throwInvalidStatuses);

		if (result.MergeHasError(commitResult))
		{
			Logger?.LogResultErrorMessages(commitResult, true, true);

			var rollbackResult = TransactionsController.RollbackAll(
				scopeContext,
				commitResult.ErrorMessages[0].Exception ?? commitResult.ToException(scopeContext, null, false, true),
				TransactionsControllerStatus.CommitInProgress);

			if (result.MergeHasError(rollbackResult))
				Logger?.LogResultErrorMessages(rollbackResult, true, true);

			return result/*.WithData(false)*/.Build();
		}

		return result.WithData(true).Build();
	}

	public async Task<IResult<bool?>> CommitAllAsync(
		IScopeContext scopeContext,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		scopeContext = scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber);

		var result = new ResultBuilder<bool?>();

		if (TransactionsController == null)
			return result.WithData(false).Build();

		var commitResult = await TransactionsController.CommitAllAsync(
			scopeContext,
			throwInvalidStatuses,
			cancellationToken);

		if (result.MergeHasError(commitResult))
		{
			Logger?.LogResultErrorMessages(commitResult, true, true);

			var rollbackResult = await TransactionsController.RollbackAllAsync(
				scopeContext,
				commitResult.ErrorMessages[0].Exception ?? commitResult.ToException(scopeContext, null, false, true),
				TransactionsControllerStatus.CommitInProgress,
				cancellationToken: default);

			if (result.MergeHasError(rollbackResult))
				Logger?.LogResultErrorMessages(rollbackResult, true, true);

			return result/*.WithData(false)*/.Build();
		}

		return result.WithData(true).Build();
	}

	public IResult<bool?> RollbackAll(
		IScopeContext scopeContext,
		Exception? exception,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		scopeContext = scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber);

		var result = new ResultBuilder<bool?>();

		if (TransactionsController == null)
			return result.WithData(false).Build();

		var rollbackResult = TransactionsController.RollbackAll(
			scopeContext,
			exception,
			throwInvalidStatuses);

		if (result.MergeHasError(rollbackResult))
		{
			Logger?.LogResultErrorMessages(rollbackResult, true, true);

			return result/*.WithData(false)*/.Build();
		}

		return result.WithData(true).Build();
	}

	public async Task<IResult<bool?>> RollbackAllAsync(
		IScopeContext scopeContext,
		Exception? exception,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		scopeContext = scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber);

		var result = new ResultBuilder<bool?>();

		if (TransactionsController == null)
			return result.WithData(false).Build();

		var rollbackResult = await TransactionsController.RollbackAllAsync(
			scopeContext,
			exception,
			throwInvalidStatuses,
			cancellationToken);

		if (result.MergeHasError(rollbackResult))
		{
			Logger?.LogResultErrorMessages(rollbackResult, true, true);

			return result/*.WithData(false)*/.Build();
		}

		return result.WithData(true).Build();
	}

	protected virtual void ClearBeforeDispose()
	{
	}

	/// <inheritdoc/>
	public virtual async ValueTask DisposeAsync()
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
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdConnectionProvider.ToString());
#endif

		Exception? exception = null;
		if (TransactionsController != null && _isInternalTransaction)
		{
			var scopeContext = ScopeContext.Create($"{this.GetType().FullName} {nameof(DisposeAsyncCoreAsync)}");

			var commitResult = await TransactionsController!.CommitAllAsync(
				scopeContext,
				TransactionsControllerStatus.None,
				cancellationToken: default);

			if (commitResult.HasError)
			{
				Logger.LogResultErrorMessages(commitResult, true, true);

				var rollbackResult = await TransactionsController!.RollbackAllAsync(
					scopeContext,
					commitResult.ErrorMessages[0].Exception ?? commitResult.ToException(scopeContext, null, false, true),
					TransactionsControllerStatus.None,
					cancellationToken: default);

				if (rollbackResult.HasError)
					Logger.LogResultErrorMessages(rollbackResult, true, true);
			}

			await TransactionsController.DisposeAsync();
		}

		ClearBeforeDispose();

		foreach (var kvp in _disposables)
		{
			if (kvp.Key is IAsyncDisposable asyncDisposable)
			{
				await asyncDisposable.DisposeAsync();
			}
			else
			{
				kvp.Key.Dispose();
			}
		}

		_disposables.Clear();

		if (AuditEntryStore != null)
			await AuditEntryStore.DisposeAsync();

		if (DomainEventStore != null)
			await DomainEventStore.DisposeAsync();

		if (ReloadableCacheKeyStore != null)
			await ReloadableCacheKeyStore.DisposeAsync();

		if (exception != null)
			throw exception;
	}

	/// <inheritdoc/>
	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
#if TRACK_OBJECTS
			Trackers.ObjectLifetimeTracker.SetDisposed(this, IdConnectionProvider.ToString());
#endif

			Exception? exception = null;
			if (TransactionsController != null && _isInternalTransaction)
			{
				var scopeContext = ScopeContext.Create($"{this.GetType().FullName} {nameof(Dispose)}");

				var commitResult = TransactionsController!.CommitAll(
					scopeContext,
					TransactionsControllerStatus.None);

				if (commitResult.HasError)
				{
					Logger.LogResultErrorMessages(commitResult, true, true);

					var rollbackResult = TransactionsController!.RollbackAll(
						scopeContext,
						commitResult.ErrorMessages[0].Exception ?? commitResult.ToException(scopeContext, null, false, true),
						TransactionsControllerStatus.None);

					if (rollbackResult.HasError)
						Logger.LogResultErrorMessages(rollbackResult, true, true);
				}

				TransactionsController.Dispose();
			}

			ClearBeforeDispose();

			foreach (var kvp in _disposables)
				kvp.Key.Dispose();

			_disposables.Clear();

			AuditEntryStore?.Dispose();
			DomainEventStore?.Dispose();
			ReloadableCacheKeyStore?.Dispose();

			if (exception != null)
				throw exception;
		}
	}

	/// <inheritdoc/>
	public virtual void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

}
