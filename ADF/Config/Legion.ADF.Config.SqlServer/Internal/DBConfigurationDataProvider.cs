using Legion.ADF.Config.Configuration;
using Legion.ADF.Config.Model;
using Legion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.SqlServer.Configuration.Internal;

internal class DBConfigurationDataProvider : IDBConfigurationDataProvider, IDisposable, IAsyncDisposable
{
	private readonly IServiceProvider _serviceProvider;

	private bool _disposed;
	private ConfigDbContext _dbContext;
	private IEFConnectionProvider? _connectionProvider;

#if TRACK_OBJECTS
	public Guid IdDBConfigurationDataProvider { get; }
#endif

	public DBConfigurationDataProvider(IServiceProvider serviceProvider)
	{
#if TRACK_OBJECTS
		IdDBConfigurationDataProvider = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdDBConfigurationDataProvider.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);

		_serviceProvider = serviceProvider;
		_dbContext = null!;
	}

	private readonly object _initLock = new();
	public void Initialize(IScopeContext scopeContext, string connectionString)
	{
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		if (_dbContext != null)
			return;

		lock (_initLock)
		{
			if (_dbContext != null)
				return;

			var builder = new DbContextOptionsBuilder<ConfigDbContext>()
				.UseSqlServer(connectionString);

			_connectionProvider = _serviceProvider.GetRequiredService<IEFConnectionProviderFactory>()
				.CreateWithoutTransaction(_serviceProvider, connectionString, false, false);

			_dbContext = _connectionProvider.GetOrCreateDbContext<ConfigDbContext>(scopeContext);
		}
	}

	public IDictionary<string, string?> LoadAllData(IScopeContext scopeContext)
	{
		if (_dbContext == null)
			Throw.InitializationException("Not initialized", scopeContext);

		var data = _dbContext.ConfigurationKeyValue
			.ToDictionary(x => x.Key, x => x.Value);

		return data;
	}

	public IDictionary<string, string?> GetDataByPath(IScopeContext scopeContext, string path)
	{
		Throw.IfArgumentNullOrWhiteSpace(path);

		if (_dbContext == null)
			Throw.InitializationException("Not initialized", scopeContext);

		var data = _dbContext.ConfigurationKeyValue
			.Where(x => x.Key.StartsWith(path))
			.ToDictionary(x => x.Key, x => x.Value);

		return data;
	}

	public IDictionary<string, ConfigurationKeyValue> GetConfigurationKeyValuesStartWithPath(string path)
	{
		if (_dbContext == null)
			Throw.InitializationException("Not initialized", (IScopeContext?)null);

		var dbData = _dbContext.ConfigurationKeyValue
			.Where(x => x.Key.StartsWith(path))
			.ToDictionary(x => x.Key);

		return dbData;
	}

	public void AddConfigurationKeyValue(ConfigurationKeyValue configurationKeyValue)
	{
		if (_dbContext == null)
			Throw.InitializationException("Not initialized", (IScopeContext?)null);

		_dbContext.ConfigurationKeyValue.Add(configurationKeyValue);
	}

	public void RemoveConfigurationKeyValue(ConfigurationKeyValue configurationKeyValue)
	{
		if (_dbContext == null)
			Throw.InitializationException("Not initialized", (IScopeContext?)null);

		_dbContext.ConfigurationKeyValue.Remove(configurationKeyValue);
	}

	public int Save(IScopeContext scopeContext)
	{
		if (_dbContext == null)
			Throw.InitializationException("Not initialized", (IScopeContext?)null);

		return _dbContext.Save(scopeContext);
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdDBConfigurationDataProvider.ToString());
#endif

		if (_connectionProvider != null)
			await _connectionProvider.DisposeAsync();
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
			Trackers.ObjectLifetimeTracker.SetDisposed(this, IdDBConfigurationDataProvider.ToString());
#endif

			_connectionProvider?.Dispose();
		}
	}

	/// <inheritdoc/>
	public virtual void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
