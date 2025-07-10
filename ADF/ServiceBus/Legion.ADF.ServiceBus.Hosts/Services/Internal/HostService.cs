using Legion.ADF.ServiceBus.Settings;
using Legion.Database;
using Legion.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ServiceBus.Hosts.Services.Internal;

internal partial class HostService : BackgroundService, IDisposable
{
	private readonly EnterpriseServiceBusOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly IConnectionProviderFactory _connectionProviderFactory;
	private readonly ILogger<HostService> _logger;

	private bool _disposed;

	public HostService(
		IOptions<EnterpriseServiceBusOptions> options,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		ILogger<HostService> logger)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create($"HostService - {_options.HostName}");

		var firstDelay = await WriteStartAsync(scopeContext, cancellationToken);

		await WriteHeartbeatAsync(scopeContext, firstDelay, cancellationToken);
		//TODO: check other hosts for heartbeat -> start jobs (own or others) AND stop (other jobs)

		await WriteStopAsync(scopeContext);
	}

	private IResult LogHost(
		IScopeContext scopeContext,
		Guid idHost,
		string code,
		bool force,
		ILogMessage logMessage,
		bool isRunning,
		IHostsUnitOfWork uow)
	{
		if (!force && !_logger.IsEnabled((LogLevel)logMessage.IdLogLevel))
			return new ResultBuilder().Build();

		var createdResult = Model.HostLog.Create(
			scopeContext,
			idHost,
			code,
			logMessage,
			isRunning);

		if (!createdResult.HasErrorOrNullData)
			uow.HostLogRepository.Add(scopeContext, createdResult.Data!);

		return createdResult;
	}

	private async Task<TimeSpan> WriteStartAsync(
		IScopeContext originalScopeContext,
		CancellationToken cancellationToken = default)
	{
		var errorCount = 0;
		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = originalScopeContext.CreateNew()
				.AddContextProperty(nameof(_options.HostName), _options.HostName);

			Guid? hostId = null;
			DTOs.Hosts.HostConfigurationDto hostConfiguration = null!;

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;
				var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
				await using var connectionProvider = uow.ConnectionProvider;

				var host = await uow.HostRepository
					.GetHostByName(new Queries.Host.GetHostByNameQuery(_options.HostName, GetDisabledHost: true, CheckReadPermissions: false, AsNoTracking: false, DisableCahce: true, null))
					.ToResultAsync(scopeContext, cancellationToken);

				if (host == null)
				{
					_logger.LogErrorMessage(
						scopeContext,
						Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.NoHostFound(_options.HostName));

					errorCount++;
					await Task.Delay(TimeSpan.FromSeconds(_options.NoHostTimeoutInSeconds), cancellationToken);
					//REPEAT while
				}
				else
				{
					hostId = host.IdHost;
					var cfgResult = host.GetHostConfiguration(scopeContext);
					cfgResult.ThrowIfErrorOrNullData(scopeContext, null, true);
					hostConfiguration = cfgResult.Data!;

					var cfgValidationResult = DTOs.Hosts.HostConfigurationDto.DefaultValidator.Value.Validate(hostConfiguration);
					if (cfgValidationResult.HasError)
					{
						LogHost(
							scopeContext,
							host.IdHost,
							Codes.InvalidConfig,
							force: true,
							new LogMessageBuilder(scopeContext, errorCode: null)
								.LogLevel(LogLevel.Error)
								.InternalMessage($"Host {host.Name}::{host.IdHost} has invalid configuration")
								.Detail(cfgValidationResult.ToText(scopeContext, Environment.NewLine, withDetail: true, withSeverity: false))
								.Build(),
							isRunning: false,
							uow)
							.ThrowIfErrorOrNullData(scopeContext, null, true);

						var saveLogResult = await uow.SaveAsync(scopeContext, cancellationToken);
						saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

						errorCount++;
						await Task.Delay(hostConfiguration.GetDelay(errorCount), cancellationToken);
						//REPEAT while
					}
					else
					{
						if (!host.IsEnabled)
						{
							LogHost(
								scopeContext,
								host.IdHost,
								Codes.Disabled,
								force: true,
								new LogMessageBuilder(scopeContext, errorCode: null)
									.LogLevel(LogLevel.Warning)
									.InternalMessage($"Host {host.Name}::{host.IdHost} is disabled, cannot start")
									.Build(),
								isRunning: false,
								uow)
								.ThrowIfErrorOrNullData(scopeContext, null, true);

							var saveLogResult = await uow.SaveAsync(scopeContext, cancellationToken);
							saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

							errorCount++;
							await Task.Delay(hostConfiguration.GetDelay(errorCount), cancellationToken);
							//REPEAT while
						}
						else
						{
							var updated = host.SetStart(scopeContext);
							updated.ThrowIfError(scopeContext, null, true);

							LogHost(
								scopeContext,
								host.IdHost,
								Codes.Started,
								force: true,
								new LogMessageBuilder(scopeContext, errorCode: null)
									.LogLevel(LogLevel.Information)
									.InternalMessage($"Host {host.Name}::{host.IdHost} is started.")
									.Build(),
								isRunning: true,
								uow)
								.ThrowIfErrorOrNullData(scopeContext, null, true);

							var saveResult = await uow.SaveAsync(scopeContext, cancellationToken);
							saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

							errorCount = 0;
							return TimeSpan.FromSeconds(hostConfiguration.HeartbeatInSeconds); //SUCCEEDED STOP while
						}
					}
				}
			}
			catch (Exception ex)
			{
				errorCount++;

				_logger.LogCriticalMessage(
					scopeContext,
					Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, hostId),
					x => x.ExceptionInfo(ex, force: true));

				await Task.Delay(hostConfiguration != null
					? hostConfiguration.GetDelay(errorCount)
					: TimeSpan.FromSeconds(DTOs.Hosts.HostConfigurationDto.MAX_TIMEOUT_SECONDS), cancellationToken);
				//REPEAT while
			}
		}

		return TimeSpan.Zero; //fake
	}

	private async Task WriteHeartbeatAsync(
		IScopeContext originalScopeContext,
		TimeSpan firstDelay,
		CancellationToken cancellationToken = default)
	{
		await Task.Delay(firstDelay, cancellationToken);

		var errorCount = 0;
		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = originalScopeContext.CreateNew()
				.AddContextProperty(nameof(_options.HostName), _options.HostName);

			Guid? hostId = null;
			DTOs.Hosts.HostConfigurationDto hostConfiguration = null!;

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;
				var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
				await using var connectionProvider = uow.ConnectionProvider;

				var host = await uow.HostRepository
					.GetHostByName(new Queries.Host.GetHostByNameQuery(_options.HostName, GetDisabledHost: true, CheckReadPermissions: false, AsNoTracking: false, DisableCahce: true, null))
					.ToResultAsync(scopeContext, cancellationToken);

				if (host == null)
				{
					_logger.LogErrorMessage(
						scopeContext,
						Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.NoHostFound(_options.HostName));

					errorCount++;
					await Task.Delay(TimeSpan.FromSeconds(_options.NoHostTimeoutInSeconds), cancellationToken);
					//REPEAT while
				}
				else
				{
					hostId = host.IdHost;
					var cfgResult = host.GetHostConfiguration(scopeContext);
					cfgResult.ThrowIfErrorOrNullData(scopeContext, null, true);
					hostConfiguration = cfgResult.Data!;

					var cfgValidationResult = DTOs.Hosts.HostConfigurationDto.DefaultValidator.Value.Validate(hostConfiguration);
					if (cfgValidationResult.HasError)
					{
						LogHost(
							scopeContext,
							host.IdHost,
							Codes.InvalidConfig,
							force: true,
							new LogMessageBuilder(scopeContext, errorCode: null)
								.LogLevel(LogLevel.Error)
								.InternalMessage($"Host {host.Name}::{host.IdHost} has invalid configuration")
								.Detail(cfgValidationResult.ToText(scopeContext, Environment.NewLine, withDetail: true, withSeverity: false))
								.Build(),
							isRunning: false,
							uow)
							.ThrowIfErrorOrNullData(scopeContext, null, true);

						var saveLogResult = await uow.SaveAsync(scopeContext, cancellationToken);
						saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

						errorCount++;
						await Task.Delay(hostConfiguration.GetDelay(errorCount), cancellationToken);
						//REPEAT while
					}
					else
					{
						if (!host.IsEnabled)
						{
							LogHost(
								scopeContext,
								host.IdHost,
								Codes.Disabled,
								force: true,
								new LogMessageBuilder(scopeContext, errorCode: null)
									.LogLevel(LogLevel.Warning)
									.InternalMessage($"Host {host.Name}::{host.IdHost} is disabled, cannot update last activity")
									.Build(),
								isRunning: false,
								uow)
								.ThrowIfErrorOrNullData(scopeContext, null, true);

							var saveLogResult = await uow.SaveAsync(scopeContext, cancellationToken);
							saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

							errorCount++;
							await Task.Delay(hostConfiguration.GetDelay(errorCount), cancellationToken);
							//REPEAT while
						}
						else
						{
							var updated = host.UpdateLastActivity(scopeContext);
							updated.ThrowIfError(scopeContext, null, true);

							LogHost(
								scopeContext,
								host.IdHost,
								Codes.Heartbeat,
								force: false,
								new LogMessageBuilder(scopeContext, errorCode: null)
									.LogLevel(LogLevel.Trace)
									.InternalMessage($"Host {host.Name}::{host.IdHost} heartbeat")
									.Build(),
								isRunning: true,
								uow)
								.ThrowIfErrorOrNullData(scopeContext, null, true);

							var saveResult = await uow.SaveAsync(scopeContext, cancellationToken);
							saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

							errorCount = 0;
							await Task.Delay(TimeSpan.FromSeconds(hostConfiguration.HeartbeatInSeconds), cancellationToken);
							//REPEAT while
						}
					}
				}
			}
			catch (Exception ex)
			{
				errorCount++;

				_logger.LogCriticalMessage(
					scopeContext,
					Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, hostId),
					x => x.ExceptionInfo(ex, force: true));

				await Task.Delay(hostConfiguration != null
					? hostConfiguration.GetDelay(errorCount)
					: TimeSpan.FromSeconds(DTOs.Hosts.HostConfigurationDto.MAX_TIMEOUT_SECONDS), cancellationToken);
				//REPEAT while
			}
		}
	}

	private async Task WriteStopAsync(
		IScopeContext scopeContext
		/*CancellationToken cancellationToken = default -- cancellationToken WAS ALWAYS CANCELED*/)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName);

		Guid? hostId = null;

		try
		{
			await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
			var scopedServiceProvider = asyncServiceScope.ServiceProvider;
			var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
			await using var connectionProvider = uow.ConnectionProvider;

			var host = await uow.HostRepository
				.GetHostByName(new Queries.Host.GetHostByNameQuery(_options.HostName, GetDisabledHost: true, CheckReadPermissions: false, AsNoTracking: false, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken: default);

			if (host == null)
				return;

			hostId = host.IdHost;

			if (host.IsEnabled)
			{
				var updated = host.SetStop(scopeContext);
				updated.ThrowIfError(scopeContext, null, true);
			}

			LogHost(
				scopeContext,
				host.IdHost,
				Codes.Stopped,
				force: true,
				new LogMessageBuilder(scopeContext, errorCode: null)
					.LogLevel(LogLevel.Information)
					.InternalMessage($"Host {host.Name}::{host.IdHost} is started.")
					.Build(),
				isRunning: true,
				uow)
				.ThrowIfErrorOrNullData(scopeContext, null, true);

			var saveResult = await uow.SaveAsync(scopeContext, cancellationToken: default);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		}
		catch (Exception ex)
		{
			_logger.LogCriticalMessage(
				scopeContext,
				Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, hostId),
				x => x.ExceptionInfo(ex, force: true));
		}
	}

	private IHostsUnitOfWork CreateUnitOfWork(IScopeContext scopeContext, IServiceProvider serviceProvider)
	{
		var connectionProvider = _connectionProviderFactory.CreateWithoutTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider,
			_options.StoreId,
			false,
			false);

		var cacheUowResult = connectionProvider.UnitOfWorkProvider.Create<IHostsUnitOfWork>(scopeContext);

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
