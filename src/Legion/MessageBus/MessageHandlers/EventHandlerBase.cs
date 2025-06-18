using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.MessageHandlers;

public abstract class EventHandlerBase<TEvent> : IEventHandler<TEvent>
	where TEvent : IEvent
{
	public virtual Type? InterceptorType { get; }

	public abstract IResult Handle(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider);
}
