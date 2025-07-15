using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.Database;

public abstract class UnitOfWorkServiceBase<TUoW, TCSP> : IDisposable, IAsyncDisposable
	where TUoW : Model.Repositories.IUnitOfWork
	where TCSP : class, IConnectionStringProvider
{
	private readonly IScopeContext _scopeContext;

	protected readonly string? _storeId;
	protected readonly ILogger<UnitOfWorkServiceBase<TUoW, TCSP>> _logger;

	private readonly Lazy<TUoW> _uow;
	private readonly Lazy<TUoW> _uowTran;

	protected IConnectionProvider? _connectionProvider;
	protected IConnectionProvider? _connectionProviderTran;
	protected bool _disposed;

	protected IServiceProvider ServiceProvider { get; }

	/// <summary>
	/// for implicit transaction inside DbContext, like:
	/// 
	/// change entity1;
	/// change entity2;
	/// change entity3;
	/// await dbContext.SaveChangesAsync();
	/// </summary>
	public TUoW UoW => _uow.Value;

	/// <summary>
	/// for multiple saves on DbContext, like:
	/// 
	/// change entity1;
	/// await dbContext.SaveChangesAsync();
	/// 
	/// change entity2;
	/// await dbContext.SaveChangesAsync();
	/// 
	/// change entity3;
	/// await dbContext.SaveChangesAsync();
	/// 
	/// await transaction.CommitAsync();
	/// </summary>
	public TUoW UoWTran => _uowTran.Value;

	protected Guid IdUnitOfWorkService { get; }

	public UnitOfWorkServiceBase(
		IScopeContext scopeContext,
		string? storeId,
		IServiceProvider serviceProvider,
		ILogger<UnitOfWorkServiceBase<TUoW, TCSP>> logger)
	{
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(logger);

		_storeId = storeId;
		ServiceProvider = serviceProvider;
		_logger = logger;

		_uow = new(CreateUnitOfWorkWithoutTransaction);
		_uowTran = new(CreateUnitOfWorkWithTransaction);
		_scopeContext = scopeContext;
		IdUnitOfWorkService = GlobalContext.Instance.NewGuid();
	}

	protected TUoW CreateUnitOfWorkWithoutTransaction()
	{
		var _connectionProviderFactory = ServiceProvider.GetRequiredService<IConnectionProviderFactory>();
		_connectionProvider = _connectionProviderFactory.CreateWithoutTransactionByStoreId<TCSP>(
			ServiceProvider,
			_storeId,
			false,
			false);

		var cacheUowResult = _connectionProvider.UnitOfWorkProvider.Create<TUoW>(_scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(_scopeContext, Legion.Exceptions.Internal.ErrorCodes.UnitOfWorkException.InvalidUoW, true);

		return cacheUowResult.Data!;
	}

	protected TUoW CreateUnitOfWorkWithTransaction()
	{
		var _connectionProviderFactory = ServiceProvider.GetRequiredService<IConnectionProviderFactory>();
		_connectionProviderTran = _connectionProviderFactory.CreateWithNewTransactionByStoreId<TCSP>(
			ServiceProvider,
			_storeId,
			transactionIsolationLevel: null,
			false,
			false);

		var cacheUowResult = _connectionProviderTran.UnitOfWorkProvider.Create<TUoW>(_scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(_scopeContext, Legion.Exceptions.Internal.ErrorCodes.UnitOfWorkException.InvalidUoW, true);

		return cacheUowResult.Data!;
	}

	protected TUoW CreateStandaloneUnitOfWorkWithoutTransaction(
		IServiceProvider? serviceProvider = null,
		bool? allowLocking = null,
		bool createAuditEntryStore = false)
	{
		serviceProvider ??= ServiceProvider;

		var connectionStringProvider = serviceProvider.GetRequiredService<TCSP>();
		var uowFactrory = serviceProvider.GetRequiredService<IUnitOfWorkFactory<TUoW>>();
		var uow = uowFactrory.CreateWithoutTransaction(
			serviceProvider,
			connectionStringProvider.GetConncetionString(_storeId!),
			allowLocking,
			createAuditEntryStore);

		return uow;
	}

	protected TUoW CreateStandaloneUnitOfWorkWithTransaction(
		IServiceProvider? serviceProvider = null,
		System.Data.IsolationLevel? isolationLevel = null,
		bool? allowLocking = null,
		bool createAuditEntryStore = false)
	{
		serviceProvider ??= ServiceProvider;

		var connectionStringProvider = serviceProvider.GetRequiredService<TCSP>();
		var uowFactrory = serviceProvider.GetRequiredService<IUnitOfWorkFactory<TUoW>>();
		var uow = uowFactrory.Create(
			serviceProvider,
			connectionStringProvider.GetConncetionString(_storeId!),
			isolationLevel,
			allowLocking,
			createAuditEntryStore);

		return uow;
	}

	protected TQUoW GetQueryUnitOfWork<TQUoW>(IServiceProvider? serviceProvider = null)
		where TQUoW : Legion.Model.Repositories.IQueryUnitOfWork
	{
		serviceProvider ??= ServiceProvider;

		var quowFactrory = serviceProvider.GetRequiredService<IQueryUnitOfWorkFactory<TQUoW>>();
		var quow = quowFactrory.Create(_uow.Value);
		return quow;
	}

	protected TQUoW GetQueryUnitOfWorkWithTransaction<TQUoW>(IServiceProvider? serviceProvider = null)
		where TQUoW : Legion.Model.Repositories.IQueryUnitOfWork
	{
		serviceProvider ??= ServiceProvider;

		var quowFactrory = serviceProvider.GetRequiredService<IQueryUnitOfWorkFactory<TQUoW>>();
		var quow = quowFactrory.Create(_uowTran.Value);
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdUnitOfWorkService.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdUnitOfWorkService.ToString());
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
