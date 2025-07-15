using Legion.ADF.ServiceBus.Services.Internal.Dto;
using Legion.ADF.ServiceBus.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ServiceBus.Services.Internal;

internal partial class EnterpriseServiceBus : BackgroundService, IDisposable
{
	private readonly EnterpriseServiceBusOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<EnterpriseServiceBus> _logger;

	private readonly HostContext _hostContext;

	private bool _disposed;

	public EnterpriseServiceBus(
		IOptions<EnterpriseServiceBusOptions> options,
		IServiceProvider serviceProvider,
		ILogger<EnterpriseServiceBus> logger)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_serviceProvider = serviceProvider;
		_logger = logger;

		_hostContext = new HostContext();
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create($"{_options.HostName} - EnterpriseServiceBus");

		await StartHostAsync(scopeContext, cancellationToken);

		await HostHeartbeatAsync(scopeContext, cancellationToken);
		//TODO:start jobs (own or others) AND stop (other jobs)

		await StopHostAsync(scopeContext);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				//
			}

			_disposed = true;
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
