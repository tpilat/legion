using Legion.ADF.Logs.Settings;
using Legion.Database;
using Legion.Logging;
using Legion.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Logs.Services;

public partial class ADFLogger : ILogger
{
	private readonly string _categoryName;
	private readonly Func<LoggerSettings> _getCurrentConfig;
	private readonly Func<IServiceScope> _serviceScopeDelegate;
	private readonly IConnectionProviderFactory _connectionProviderFactory;
	private readonly LogsStoreOptions _options;
	private readonly IADFLoggerStore _adfLoggerStore;

	public ADFLogger(
		string categoryName,
		Func<LoggerSettings> getCurrentConfig,
		Func<IServiceScope> serviceScopeDelegate)
	{
		//Throw.ArgumentNullOrWhiteSpace(categoryName);
		Throw.IfArgumentNull(getCurrentConfig);
		Throw.IfArgumentNull(serviceScopeDelegate);

		_categoryName = categoryName;
		_getCurrentConfig = getCurrentConfig;
		_serviceScopeDelegate = serviceScopeDelegate;

		using var scope = _serviceScopeDelegate();
		var sp = scope.ServiceProvider;
		_connectionProviderFactory = sp.GetRequiredService<IConnectionProviderFactory>();
		_options = sp.GetRequiredService<IOptions<LogsStoreOptions>>().Value;
		_adfLoggerStore = sp.GetRequiredService<IADFLoggerStore>();
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		=> default!;

	public bool IsEnabled(LogLevel logLevel) =>
		true;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		bool isLogged = false;
		if (state is IEnumerable<KeyValuePair<string, object>> structure)
		{
			foreach (var item in structure)
			{
				if (item.Key == LogMessage.LogMessageParamName)
				{
					if (item.Value is ErrorMessage errorMessage)
					{
						SaveErrorMessage(errorMessage);
						isLogged = true;
					}
					else if (item.Value is LogMessage logMessage)
					{
						SaveLogMessage(logMessage);
						isLogged = true;
					}
				}
			}
		}

		if (!isLogged)
		{
			SaveUnstructuredLog(
				logLevel,
				eventId,
				state,
				exception,
				formatter);
		}
	}

	private void SaveErrorMessage(ErrorMessage errorMessage)
		=> SaveLogMessage(errorMessage);

	private readonly object _batchLogMessageStoreLock = new();
	private BatchLogMessageStore _batchLogMessageStore = null!;
	private BatchLogMessageStore GetBatchLogMessageStore()
	{
		if (_batchLogMessageStore != null)
			return _batchLogMessageStore;

		lock (_batchLogMessageStoreLock)
		{
			if (_batchLogMessageStore != null)
				return _batchLogMessageStore;

			using var scope = _serviceScopeDelegate();
			var sp = scope.ServiceProvider;
			_batchLogMessageStore = sp.GetRequiredService<BatchLogMessageStore>();
			return _batchLogMessageStore;
		}
	}

	private readonly object _batchUnstructuredLogStoreLock = new();
	private BatchUnstructuredLogStore _batchUnstructuredLogStore = null!;
	private BatchUnstructuredLogStore GetBatchUnstructuredLogStore()
	{
		if (_batchUnstructuredLogStore != null)
			return _batchUnstructuredLogStore;

		lock (_batchUnstructuredLogStoreLock)
		{
			if (_batchUnstructuredLogStore != null)
				return _batchUnstructuredLogStore;

			using var scope = _serviceScopeDelegate();
			var sp = scope.ServiceProvider;
			_batchUnstructuredLogStore = sp.GetRequiredService<BatchUnstructuredLogStore>();
			return _batchUnstructuredLogStore;
		}
	}

	private void SaveLogMessage(LogMessage logMessage)
	{
		if (logMessage == null)
			return;

		if (logMessage.LogLevel < _getCurrentConfig().LogMessageMinLogLevel)
			return;

		try
		{
			if (_getCurrentConfig().UseBatchWriter)
			{
				var logsStore = GetBatchLogMessageStore();

				if (string.IsNullOrWhiteSpace(logMessage.SourceContext))
					logMessage.SourceContext = _categoryName;
				
				logsStore.Write(logMessage);
			}
			else
			{
				using var scope = _serviceScopeDelegate();
				var sp = scope.ServiceProvider;
				//using var logsStore = sp.GetRequiredService<LogsStore>();
				var scopeContext = ScopeContext.Create(nameof(ADFLogger));

				using var connectionProvider = _connectionProviderFactory!.CreateWithoutTransactionByStoreId<ConnectionStringProvider>(
					sp!,
					_options.LogStoreId,
					false,
					false);

				using var connection = connectionProvider.GetOrCreateNewDbConnection(out _);


				var logResult = Model.Log.CreateLog(scopeContext, logMessage, _categoryName);

				if (!logResult.HasAnyMessage && logResult.Data != null)
				{
					//logsStore.SaveLog(scopeContext, logResult.Data, false);
					_adfLoggerStore.SaveLog(scopeContext, connection, logResult.Data);
				}
			}
		}
		catch { }
	}

	private void SaveUnstructuredLog<TState>(
		Microsoft.Extensions.Logging.LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		if (logLevel < _getCurrentConfig().UnstructuredLogMinLogLevel)
			return;

		try
		{
			string? message = null;
			if (formatter != null)
			{
				try
				{
					message = formatter(state, null);
				}
				catch { }
			}

			if (string.IsNullOrWhiteSpace(message))
			{
				try
				{
					message = formatter != null
						? formatter(state, null)
						: ToStringHelper.ToString(state);
				}
				catch
				{
					message = "?ADF?";
				}
			}

			var dto = new DTOs.UnstructuredLog
			{
				EventId = eventId,
				LogLevel = logLevel,
				Message = message,
				Exception = exception,
				SourceContext = _categoryName
			};

			if (_getCurrentConfig().UseBatchWriter)
			{
				var logsStore = GetBatchUnstructuredLogStore();
				logsStore.Write(dto);
			}
			else
			{
				using var scope = _serviceScopeDelegate();
				var sp = scope.ServiceProvider;
				//using var logsStore = sp.GetRequiredService<LogsStore>();
				var scopeContext = ScopeContext.Create(nameof(ADFLogger));

				using var connectionProvider = _connectionProviderFactory!.CreateWithoutTransactionByStoreId<ConnectionStringProvider>(
					sp!,
					_options.LogStoreId,
					false,
					false);

				using var connection = connectionProvider.GetOrCreateNewDbConnection(out _);

				var unstructuredLogResult = Model.UnstructuredLog.CreateUnstructuredLog(
					scopeContext,
					dto,
					_categoryName);

				if (!unstructuredLogResult.HasAnyMessage && unstructuredLogResult.Data != null)
				{
					//logsStore.SaveUnstructuredLog(scopeContext, unstructuredLogResult.Data, false);
					_adfLoggerStore.SaveUnstructuredLog(scopeContext, connection, unstructuredLogResult.Data);
				}
			}
		}
		catch { }
	}
}
