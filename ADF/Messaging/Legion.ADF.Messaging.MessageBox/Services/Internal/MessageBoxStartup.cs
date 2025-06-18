using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.DependencyInjection;
using Legion.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.MessageBox.Services.Internal;

public class MessageBoxStartup : IStartupTask
{
	private static readonly AsyncLock _lock = new();
	private bool _executed;

	public async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
	{
		if (_executed)
			return;

		var scopeContext = ScopeContext.Create(nameof(MessageBoxStartup));

		using (await _lock.LockAsync().ConfigureAwait(false))
		{
			if (_executed)
				return;

			var connectionProviderFactory = serviceProvider.GetRequiredService<IConnectionProviderFactory>();
			var messageProcessingServiceOptions = serviceProvider.GetRequiredService<IOptions<MessageBoxMessageProcessingServiceOptions>>().Value;
			var storeOptions = serviceProvider.GetRequiredService<IOptions<MessagingMessageBoxStoreOptions>>().Value;
			var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<MessageBoxStartup>();

			await Initializer.InitializeAsync(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				messageProcessingServiceOptions,
				storeOptions,
				logger,
				cancellationToken);

			_executed = true;
		}
	}
}
