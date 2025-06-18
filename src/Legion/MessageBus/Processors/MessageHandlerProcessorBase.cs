using Legion.MessageBus.MessageHandlers;

namespace Legion.MessageBus.Processors;

internal abstract class MessageHandlerProcessorBase
{
	protected abstract IMessageHandler CreateHandler(IServiceProvider serviceProvider);
}
