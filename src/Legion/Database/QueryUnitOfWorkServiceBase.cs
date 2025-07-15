using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.Database;

public abstract class QueryUnitOfWorkServiceBase<TQUoW, TCSP> : IDisposable, IAsyncDisposable
	where TQUoW : Model.Repositories.IQueryUnitOfWork
	where TCSP : class, IConnectionStringProvider
{
	private readonly IScopeContext _scopeContext;

	protected readonly string? _storeId;
	protected readonly IServiceProvider _serviceProvider;
	protected readonly ILogger<QueryUnitOfWorkServiceBase<TQUoW, TCSP>> _logger;

	private readonly Lazy<TQUoW> _uow;

	protected IConnectionProvider? _connectionProvider;
	protected IConnectionProvider? _connectionProviderTran;
	protected bool _disposed;

	public TQUoW QUoW => _uow.Value;

	protected Guid IdQueryUnitOfWorkService { get; }

	public QueryUnitOfWorkServiceBase(
		IScopeContext scopeContext,
		string? storeId,
		IServiceProvider serviceProvider,
		ILogger<QueryUnitOfWorkServiceBase<TQUoW, TCSP>> logger)
	{
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(logger);

		_storeId = storeId;
		_serviceProvider = serviceProvider;
		_logger = logger;

		_uow = new(CreateUnitOfWorkWithoutTransaction);
		_scopeContext = scopeContext;
		IdQueryUnitOfWorkService = GlobalContext.Instance.NewGuid();
	}

	protected TQUoW CreateUnitOfWorkWithoutTransaction()
	{
		var _connectionProviderFactory = _serviceProvider.GetRequiredService<IConnectionProviderFactory>();
		_connectionProvider = _connectionProviderFactory.CreateWithoutTransactionByStoreId<TCSP>(
			_serviceProvider,
			_storeId,
			false,
			false);

		var cacheUowResult = _connectionProvider.UnitOfWorkProvider.CreateQuery<TQUoW>(_scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(_scopeContext, Legion.Exceptions.Internal.ErrorCodes.UnitOfWorkException.InvalidUoW, true);

		return cacheUowResult.Data!;
	}

	protected TQUoW CreateStandaloneUnitOfWorkWithoutTransaction(
		bool? allowLocking = null,
		bool createAuditEntryStore = false)
	{
		var connectionStringProvider = _serviceProvider.GetRequiredService<TCSP>();
		var quowFactrory = _serviceProvider.GetRequiredService<IQueryUnitOfWorkFactory<TQUoW>>();
		var quow = quowFactrory.CreateWithoutTransaction(
			_serviceProvider,
			connectionStringProvider.GetDefaultConncetionString(),
			allowLocking,
			createAuditEntryStore);

		return quow;
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

	protected virtual async ValueTask DisposeAsyncCoreAsync()
	{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdQueryUnitOfWorkService.ToString());
#endif

		if (_connectionProvider != null)
			await _connectionProvider.DisposeAsync();

		if (_connectionProviderTran != null)
			await _connectionProviderTran.DisposeAsync();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdQueryUnitOfWorkService.ToString());
#endif

			_connectionProvider?.Dispose();
			_connectionProviderTran?.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
