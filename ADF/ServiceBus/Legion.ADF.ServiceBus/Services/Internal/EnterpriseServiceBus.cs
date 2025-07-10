using Legion.ADF.ServiceBus.Settings;
using Legion.Database;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ServiceBus.Services.Internal;

internal partial class EnterpriseServiceBus : BackgroundService, IDisposable
{
	private readonly EnterpriseServiceBusOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly IConnectionProviderFactory _connectionProviderFactory;
	private readonly ILogger<EnterpriseServiceBus> _logger;

	private readonly List<Model.VwHost> _availableHosts;

	private bool _disposed;

	public EnterpriseServiceBus(
		IOptions<EnterpriseServiceBusOptions> options,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		ILogger<EnterpriseServiceBus> logger)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_logger = logger;

		_availableHosts = [];
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create($"{_options.HostName} - EnterpriseServiceBus");

		var firstHearbeatDelay = await StartHostAsync(scopeContext, cancellationToken);

		Task hostHeartbeatTask = HostHeartbeatAsync(scopeContext, firstHearbeatDelay, cancellationToken);
		//TODO: check other hosts for heartbeat -> start jobs (own or others) AND stop (other jobs)

		await Task.WhenAny(hostHeartbeatTask, hostHeartbeatTask);

		await StopHostAsync(scopeContext);
	}

	private IServiceBusUnitOfWork CreateUnitOfWork(IScopeContext scopeContext, IServiceProvider serviceProvider)
	{
		var connectionProvider = _connectionProviderFactory.CreateWithoutTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider,
			_options.StoreId,
			false,
			false);

		var cacheUowResult = connectionProvider.UnitOfWorkProvider.Create<IServiceBusUnitOfWork>(scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusUnitOfWorkException.InvalidUoW, true);

		return cacheUowResult.Data!;
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

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
