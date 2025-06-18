using Legion.Logging.PostgreSQL.Config;
using Microsoft.Extensions.Logging;

namespace Legion.Logging.PostgreSQL;

internal sealed class PostgreSQLLogger : ILogger
{
	private readonly string _categoryName;
	private readonly Func<PostgreSQLLoggerConfiguration> _getCurrentConfig;

	public PostgreSQLLogger(string categoryName, Func<PostgreSQLLoggerConfiguration> getCurrentConfig)
	{
		//Throw.ArgumentNullOrWhiteSpace(categoryName);
		Throw.IfArgumentNull(getCurrentConfig);

		_categoryName = categoryName;
		_getCurrentConfig = getCurrentConfig;
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

	public bool IsEnabled(LogLevel logLevel) =>
		true;

	private bool IsEnabledLogMessage(LogLevel logLevel) =>
		_getCurrentConfig().LogMessageMinLogLevel <= logLevel;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		if (state is IEnumerable<KeyValuePair<string, object>> structure)
		{
			foreach (var item in structure)
			{
				if (item.Key == LogMessage.LogMessageParamName)
				{
					if (item.Value is ErrorMessage errorMessage)
					{
						SaveErrorMessage(errorMessage);
						return;
					}

					if (item.Value is LogMessage logMessage)
					{
						SaveLogMessage(logMessage);
						return;
					}
				}
			}
		}
	}

	private void SaveErrorMessage(ErrorMessage logMessage)
	{
		if (!IsEnabledLogMessage(logMessage.LogLevel))
			return;

		//TODO save to DB
	}

	private void SaveLogMessage(LogMessage logMessage)
	{
		if (!IsEnabledLogMessage(logMessage.LogLevel))
			return;

		//TODO save to DB
	}
}


