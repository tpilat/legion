using Legion.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Config.Configuration;

public class DBConfigurationManager : IDBConfigurationManager, IDBConfigurationLoader, IDisposable, IAsyncDisposable
{
	private readonly string _connectionString;
	private readonly IDBConfigurationDataProvider _dBConfigurationDataProvider;
	private readonly ILogger<DBConfigurationManager> _logger;

	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdDBConfigurationManager { get; }
#endif

	public DBConfigurationManager(
		IServiceProvider serviceProvider,
		string connectionString)
	{
#if TRACK_OBJECTS
		IdDBConfigurationManager = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdDBConfigurationManager.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		_connectionString = connectionString;
		_logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DBConfigurationManager>();

		var scopeContext = ScopeContext.Create("Legion.ADF.Config.DBConfigurationManager");

		_dBConfigurationDataProvider = serviceProvider.GetRequiredService<IDBConfigurationDataProvider>();
		_dBConfigurationDataProvider.Initialize(scopeContext, _connectionString);
	}

	public IDictionary<string, string?> LoadAllData(IScopeContext scopeContext)
		=> _dBConfigurationDataProvider.LoadAllData(scopeContext);

	public IDictionary<string, string?> GetDataByPath(IScopeContext scopeContext, string path)
		=> _dBConfigurationDataProvider.GetDataByPath(scopeContext, path);

	public int SaveDataByPath(IScopeContext scopeContext, string path, IDictionary<string, string?> data, bool force, bool removeUnusedKeys)
	{
		Throw.IfArgumentNullOrWhiteSpace(path);
		Throw.IfArgumentNull(data);

		var dbData = _dBConfigurationDataProvider.GetConfigurationKeyValuesStartWithPath(path);

		int saved = 0;
		foreach (var kvp in data)
		{
			if (dbData.TryGetValue(kvp.Key, out var configurationKeyValue))
			{
				if (force)
				{
					var ckvResult = configurationKeyValue.UpdateValue(scopeContext, kvp.Value);

					ckvResult.Log(scopeContext, _logger, dataMustBeNotNull: false);

					Throw.ResultExceptionIfHasError(
						scopeContext,
						Exceptions.Internal.ErrorCodes.DBConfigurationManagerException.Write,
						ckvResult,
						true,
						true);

					saved++;
				}
			}
			else
			{
				var ckvResult = Config.Model.ConfigurationKeyValue.Create(scopeContext, kvp.Key, kvp.Value);

				ckvResult.Log(scopeContext, _logger, dataMustBeNotNull: false);

				Throw.ResultExceptionIfHasError(
					scopeContext,
					Exceptions.Internal.ErrorCodes.DBConfigurationManagerException.Write,
					ckvResult,
					true,
					true);

				_dBConfigurationDataProvider.AddConfigurationKeyValue(ckvResult.Data);
				saved++;
			}
		}

		if (removeUnusedKeys)
		{
			foreach (var kvp in dbData)
				if (!data.ContainsKey(kvp.Key))
					_dBConfigurationDataProvider.RemoveConfigurationKeyValue(kvp.Value);
		}

		_dBConfigurationDataProvider.Save(scopeContext);
		return saved;
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdDBConfigurationManager.ToString());
#endif

		await _dBConfigurationDataProvider.DisposeAsync();
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
			Trackers.ObjectLifetimeTracker.SetDisposed(this, IdDBConfigurationManager.ToString());
#endif

			_dBConfigurationDataProvider.Dispose();
		}
	}

	/// <inheritdoc/>
	public virtual void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
