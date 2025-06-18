using Legion.MessageBus.MessageHandlers;

namespace Legion.MessageBus.Processors;

internal abstract class EventHandlerProcessorBase
{
	protected abstract IEnumerable<IEventHandler> CreateHandlers(IServiceProvider serviceProvider);
}
