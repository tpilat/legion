using Legion.ADF.Audit.Settings;
using Legion.Database;
using Legion.Model.Audit;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Audit.Services;

public partial class AuditStore : IAuditEntryStore, IDisposable, IAsyncDisposable
{
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory? _connectionProviderFactory;
	protected readonly AuditStoreOptions _options;
	protected readonly ILogger _logger;

	private readonly Lazy<IAuditAccessControlManager?> _accessControlManager;

	private bool _disposed;

	protected IAuditUnitOfWork UoW { get; private set; }
	protected IConnectionProvider? ConnectionProvider { get; private set; }
	protected bool IsInternalConnectionProvider { get; private set; }

	public bool AutoSaveChanges { get; set; } = true;
	public IAuditAccessControlManager? AccessControlManager => _accessControlManager.Value;

	public AuditStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<AuditStoreOptions> options,
		ILogger<AuditStore> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Audit");

		CreateUnitOfWork(scopeContext);

		_accessControlManager = new(() => UoW!.ServiceProvider.GetService<IAuditAccessControlManager>());
	}

	protected void CreateUnitOfWork(IScopeContext scopeContext)
	{
		IsInternalConnectionProvider = true;
		ConnectionProvider = _connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			_serviceProvider!,
			_options.AuditStoreId,
			transactionIsolationLevel: null,
			false,
			false);

		var auditUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IAuditUnitOfWork>(scopeContext);

		if (auditUowResult.HasError)
			auditUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Audit.Exceptions.Internal.ErrorCodes.AuditUnitOfWorkException.InvalidUoW, true);

		UoW = auditUowResult.Data!;
	}

	public AuditStore(
		IConnectionProvider connectionProvider,
		IOptions<AuditStoreOptions> options,
		ILogger<AuditStore> logger)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Audit");

		ConnectionProvider = connectionProvider;
		_serviceProvider = ConnectionProvider.ServiceProvider;

		IsInternalConnectionProvider = false;
		var auditUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IAuditUnitOfWork>(scopeContext);

		if (auditUowResult.HasError)
			auditUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Audit.Exceptions.Internal.ErrorCodes.AuditUnitOfWorkException.InvalidUoW, true);

		UoW = auditUowResult.Data!;

		_accessControlManager = new(() => UoW.ServiceProvider.GetService<IAuditAccessControlManager>());
	}

	public async Task<IResult<int>> SaveAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default)
		=> await SaveInternalAsync(scopeContext, true, cancellationToken).ConfigureAwait(false);

	public IResult<int> Save(IScopeContext scopeContext)
		=> SaveInternal(scopeContext, true);

	protected async Task<IResult<int>> SaveInternalAsync(IScopeContext scopeContext, bool force, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder<int>();

		if (force || AutoSaveChanges)
		{
			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken);
			if (result.MergeAllWithDataHasError(saveResult))
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

	protected IResult<int> SaveInternal(IScopeContext scopeContext, bool force)
	{
		var result = new ResultBuilder<int>();

		if (force || AutoSaveChanges)
		{
			var saveResult = UoW.Save(scopeContext);
			if (result.MergeAllWithDataHasError(saveResult))
				return result.Build();

			if (IsInternalConnectionProvider)
			{
				var commitResult = ConnectionProvider!.TransactionsController!.CommitAll(
					scopeContext,
					TransactionsControllerStatus.NotIdle);

				//ConnectionProvider!.Dispose();

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
