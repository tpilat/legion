using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

internal partial class JobsQueryUnitOfWork : Legion.ADF.ServiceBus.Jobs.IJobsQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdJobsQueryUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IQueryUnitOfWork.ConnectionProvider => ConnectionProvider;
	System.IServiceProvider Legion.Model.Repositories.IQueryUnitOfWork.ServiceProvider => ConnectionProvider.ServiceProvider;
	
	public JobsQueryUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdJobsQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public JobsQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdJobsQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public JobsQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdJobsQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public JobsQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdJobsQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsQueryUnitOfWork.ToString());
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

	public JobsQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdJobsQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsQueryUnitOfWork.ToString());
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

	protected Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsQueryDbContext>(scopeContext);

	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IVwJobRepository? vwJob;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IVwJobRepository VwJobRepository
		=> vwJob ??= new Legion.ADF.ServiceBus.Jobs.PostgreSQL.Model.Repositories.VwJobRepository(ConnectionProvider);

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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdJobsQueryUnitOfWork.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdJobsQueryUnitOfWork.ToString());
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
