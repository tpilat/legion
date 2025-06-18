using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.DependencyInjection;
using Legion.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.Inbox.Services.Internal;

public class InboxStartup : IStartupTask
{
	private static readonly AsyncLock _lock = new();
	private bool _executed;

	public async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
	{
		if (_executed)
			return;

		var scopeContext = ScopeContext.Create(nameof(InboxStartup));

		using (await _lock.LockAsync().ConfigureAwait(false))
		{
			if (_executed)
				return;

			var connectionProviderFactory = serviceProvider.GetRequiredService<IConnectionProviderFactory>();
			var inboxMessageProcessingServiceOptions = serviceProvider.GetRequiredService<IOptions<InboxMessageProcessingServiceOptions>>().Value;
			var storeOptions = serviceProvider.GetRequiredService<IOptions<MessagingInboxStoreOptions>>().Value;
			var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<InboxStartup>();

			await Initializer.InitializeAsync(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				inboxMessageProcessingServiceOptions,
				storeOptions,
				logger,
				cancellationToken);

			_executed = true;
		}
	}
}
