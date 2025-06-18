using Legion.Database;
using Legion.MessageBus;
using Legion.MessageBus.MessageResolvers;
using Legion.MessageBus.Processors;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Legion.Bus;

internal partial class InMemoryMessageBus<TConnectionStringProvider> : IMessageBus<TConnectionStringProvider>
	where TConnectionStringProvider : class, IConnectionStringProvider
{
	private static readonly ConcurrentDictionary<Type, MessageHandlerProcessorBase> _messageAsyncProcessors = new();
	private static readonly ConcurrentDictionary<Type, EventHandlerProcessorBase> _eventAsyncProcessors = new();

	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger _logger;

	public IConnectionProvider? ConnectionProvider { get; private set; }

	public InMemoryMessageBus(IServiceProvider serviceProvider, ILogger<InMemoryMessageBus<TConnectionStringProvider>> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	private readonly object _lockConnectionProvider = new();
	public bool SetConnectionProvider(IConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(connectionProvider.TransactionsController);

		if (ConnectionProvider != null)
			return false;

		lock (_lockConnectionProvider)
		{
			if (ConnectionProvider != null)
				return false;

			ConnectionProvider = connectionProvider;
			return true;
		}
	}

	public bool CanSendOrPublish(Type type)
		=> MessagesRegistry.CanBeHandled(type);
}
