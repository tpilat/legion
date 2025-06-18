using Legion.ADF.Logs.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Legion.ADF.Logs.Services;

[ProviderAlias(nameof(ADFLoggerProvider))]
internal sealed class ADFLoggerProvider : ILoggerProvider
{
	private readonly IDisposable? _onChangeToken;
	private LoggerSettings _currentConfig;
	private readonly ConcurrentDictionary<string, ADFLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
	private readonly IServiceProvider _serviceProvider;

	public ADFLoggerProvider(IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(serviceProvider);

		var config = serviceProvider.GetRequiredService<IOptionsMonitor<LoggerSettings>>();

		Throw.IfNull(config);

		_currentConfig = config.CurrentValue;
		_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
		_serviceProvider = serviceProvider;
	}

	private LoggerSettings GetCurrentConfig()
		=> _currentConfig;

	public ILogger CreateLogger(string categoryName) =>
		_loggers.GetOrAdd(
			categoryName,
			name => new ADFLogger(name, GetCurrentConfig, _serviceProvider.CreateScope));

	public void Dispose()
	{
		_loggers.Clear();
		_onChangeToken?.Dispose();
	}
}
