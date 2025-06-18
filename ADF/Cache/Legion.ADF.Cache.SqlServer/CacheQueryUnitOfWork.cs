using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.SqlServer;

internal partial class CacheQueryUnitOfWork : Legion.ADF.Cache.ICacheQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdCacheQueryUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IQueryUnitOfWork.ConnectionProvider => ConnectionProvider;
	System.IServiceProvider Legion.Model.Repositories.IQueryUnitOfWork.ServiceProvider => ConnectionProvider.ServiceProvider;
	
	public CacheQueryUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdCacheQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdCacheQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public CacheQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdCacheQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdCacheQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public CacheQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdCacheQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdCacheQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public CacheQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdCacheQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdCacheQueryUnitOfWork.ToString());
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

	public CacheQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdCacheQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdCacheQueryUnitOfWork.ToString());
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

	protected Legion.ADF.Cache.SqlServer.ICacheQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.SqlServer.ICacheQueryDbContext>(scopeContext);

	private Legion.ADF.Cache.Model.Repositories.IVwReloadableCacheKeyRepository? vwReloadableCacheKey;
	public Legion.ADF.Cache.Model.Repositories.IVwReloadableCacheKeyRepository VwReloadableCacheKeyRepository
		=> vwReloadableCacheKey ??= new Legion.ADF.Cache.SqlServer.Model.Repositories.VwReloadableCacheKeyRepository(ConnectionProvider);

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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdCacheQueryUnitOfWork.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdCacheQueryUnitOfWork.ToString());
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
