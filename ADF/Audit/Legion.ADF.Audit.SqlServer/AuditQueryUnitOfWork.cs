using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.SqlServer;

internal partial class AuditQueryUnitOfWork : Legion.ADF.Audit.IAuditQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdAuditQueryUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IQueryUnitOfWork.ConnectionProvider => ConnectionProvider;
	System.IServiceProvider Legion.Model.Repositories.IQueryUnitOfWork.ServiceProvider => ConnectionProvider.ServiceProvider;
	
	public AuditQueryUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdAuditQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuditQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public AuditQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdAuditQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuditQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public AuditQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdAuditQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuditQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public AuditQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdAuditQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuditQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithNewTransaction(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}

	public AuditQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdAuditQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuditQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}

	protected Legion.ADF.Audit.SqlServer.IAuditQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Audit.SqlServer.IAuditQueryDbContext>(scopeContext);

	private Legion.ADF.Audit.Model.Repositories.IVwApplicationEntryRepository? vwApplicationEntry;
	public Legion.ADF.Audit.Model.Repositories.IVwApplicationEntryRepository VwApplicationEntryRepository
		=> vwApplicationEntry ??= new Legion.ADF.Audit.SqlServer.Model.Repositories.VwApplicationEntryRepository(ConnectionProvider);


	private Legion.ADF.Audit.Model.Repositories.IVwAuditEntryRepository? vwAuditEntry;
	public Legion.ADF.Audit.Model.Repositories.IVwAuditEntryRepository VwAuditEntryRepository
		=> vwAuditEntry ??= new Legion.ADF.Audit.SqlServer.Model.Repositories.VwAuditEntryRepository(ConnectionProvider);

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
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdAuditQueryUnitOfWork.ToString());
#endif

		if (_isInternalConnectionProvider && ConnectionProvider != null)
		{
			await ConnectionProvider.DisposeAsync();
		}
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdAuditQueryUnitOfWork.ToString());
#endif

			if (_isInternalConnectionProvider)
			{
				ConnectionProvider?.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
