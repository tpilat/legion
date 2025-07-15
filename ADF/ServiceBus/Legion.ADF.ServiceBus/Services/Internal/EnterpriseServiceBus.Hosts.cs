using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Legion.ADF.ServiceBus.Services.Internal;

internal partial class EnterpriseServiceBus : BackgroundService, IDisposable
{
	private async Task StartHostAsync(
		IScopeContext originalScopeContext,
		CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = originalScopeContext.CreateNew()
				.AddContextProperty(nameof(_options.HostName), _options.HostName);

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				await using var hostService = scopedServiceProvider.GetRequiredService<HostService>();
				await hostService.StartHostAsync(
					scopeContext,
					_hostContext,
					cancellationToken);

				if (_hostContext.Started)
					return; //exit
			}
			catch (Exception ex)
			{
				_hostContext.IncrementError();

				_logger.LogCriticalMessage(
					scopeContext,
					Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, _hostContext.IdHost),
					x => x.ExceptionInfo(ex, force: true));

				await Task.Delay(_hostContext.GetErrorDelay(), cancellationToken);
			}
		}
	}

	private async Task HostHeartbeatAsync(
		IScopeContext originalScopeContext,
		CancellationToken cancellationToken)
	{
		await Task.Delay(TimeSpan.FromSeconds(_hostContext.HeartbeatInSeconds), cancellationToken);

		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = originalScopeContext.CreateNew()
				.AddContextProperty(nameof(_options.HostName), _options.HostName);

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				await using var hostService = scopedServiceProvider.GetRequiredService<HostService>();
				await hostService.HostHeartbeatAsync(
					scopeContext,
					_hostContext,
					scopedServiceProvider,
					cancellationToken);
			}
			catch (Exception ex)
			{
				_hostContext.IncrementError();

				_logger.LogCriticalMessage(
					scopeContext,
					Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, _hostContext.IdHost),
					x => x.ExceptionInfo(ex, force: true));

				await Task.Delay(_hostContext.GetErrorDelay(), cancellationToken);
			}
		}
	}

	private async Task StopHostAsync(
		IScopeContext scopeContext
		/*CancellationToken cancellationToken -- cancellationToken WAS ALWAYS CANCELED*/)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName);

		try
		{
			await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
			var scopedServiceProvider = asyncServiceScope.ServiceProvider;

			await using var hostService = scopedServiceProvider.GetRequiredService<HostService>();
			await hostService.StopHostAsync(scopeContext, _hostContext);
		}
		catch (Exception ex)
		{
			_logger.LogCriticalMessage(
				scopeContext,
				Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, _hostContext.IdHost),
				x => x.ExceptionInfo(ex, force: true));
		}
	}
}
