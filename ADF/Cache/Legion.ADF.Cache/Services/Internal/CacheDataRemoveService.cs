using Legion.ADF.Cache.Settings;
using Legion.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Cache.Services.Internal;

/// <summary>
/// Service to periodically remove cache keys from IADFCache.
/// </summary>
public class CacheDataRemoveService : BackgroundService
{
	private readonly CacheDataRemoveServiceOptions _options;
	private readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory _connectionProviderFactory;
	private readonly ILogger<CacheDataRemoveService> _logger;

	public CacheDataRemoveService(
		IOptions<CacheDataRemoveServiceOptions> options,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		ILogger<CacheDataRemoveService> logger)
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

	private ICacheUnitOfWork CreateUnitOfWork(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider)
	{
		var options = serviceProvider.GetRequiredService<IOptions<ADFPersistentCacheOptions>>().Value;

		var connectionProvider = _connectionProviderFactory.CreateWithoutTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider,
			options.CacheStoreId,
			false,
			false);

		var cacheUowResult = connectionProvider.UnitOfWorkProvider.Create<ICacheUnitOfWork>(scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Cache.Exceptions.Internal.ErrorCodes.CacheUnitOfWorkException.InvalidUoW, true);

		return cacheUowResult.Data!;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContextGlobal = ScopeContext.Create(nameof(CacheDataRemoveService));

		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = ScopeContext.Create(scopeContextGlobal, correlationId: Guid.NewGuid());

			_logger.LogTraceMessage(scopeContext, x => x.InternalMessage($"{nameof(CacheDataRemoveService)}.{nameof(ExecuteAsync)}: START"));

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;
				var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
				await using var connectionProvider = uow.ConnectionProvider;

				await uow.CacheDataRepository.DeleteExpiredAsync(scopeContext, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.CacheDataRemoveService.Default, x => x.ExceptionInfo(ex));
			}
			finally
			{
				await Task.Delay(TimeSpan.FromSeconds(_options.IdleTimeoutInSeconds), cancellationToken);
			}
		}
	}
}
