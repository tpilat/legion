using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.DependencyInjection;
using Legion.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.Outbox.Services.Internal;

public class OutboxStartup : IStartupTask
{
	private static readonly AsyncLock _lock = new();
	private bool _executed;

	public async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
	{
		if (_executed)
			return;

		var scopeContext = ScopeContext.Create(nameof(OutboxStartup));

		using (await _lock.LockAsync().ConfigureAwait(false))
		{
			if (_executed)
				return;

			var connectionProviderFactory = serviceProvider.GetRequiredService<IConnectionProviderFactory>();
			var outboxMessageProcessingServiceOptions = serviceProvider.GetRequiredService<IOptions<OutboxMessageProcessingServiceOptions>>().Value;
			var storeOptions = serviceProvider.GetRequiredService<IOptions<MessagingOutboxStoreOptions>>().Value;
			var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<OutboxStartup>();

			await Initializer.InitializeAsync(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				outboxMessageProcessingServiceOptions,
				storeOptions,
				logger,
				cancellationToken);

			_executed = true;
		}
	}
}
