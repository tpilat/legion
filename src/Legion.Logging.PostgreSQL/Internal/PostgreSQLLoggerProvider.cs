using Legion.Logging.PostgreSQL.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Legion.Logging.PostgreSQL;

[ProviderAlias("PostgreSQLLogger")]
internal sealed class PostgreSQLLoggerProvider : ILoggerProvider
{
	private readonly IDisposable? _onChangeToken;
	private PostgreSQLLoggerConfiguration _currentConfig;
	private readonly ConcurrentDictionary<string, PostgreSQLLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

	public PostgreSQLLoggerProvider(IOptionsMonitor<PostgreSQLLoggerConfiguration> config)
	{
		_currentConfig = config.CurrentValue;
		_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
	}

	private PostgreSQLLoggerConfiguration GetCurrentConfig()
		=> _currentConfig;

	public ILogger CreateLogger(string categoryName) =>
		_loggers.GetOrAdd(
			categoryName,
			name => new PostgreSQLLogger(name, GetCurrentConfig));

	public void Dispose()
	{
		_loggers.Clear();
		_onChangeToken?.Dispose();
	}
}
