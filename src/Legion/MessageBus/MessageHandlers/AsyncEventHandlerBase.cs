using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.MessageHandlers;

public abstract class AsyncEventHandlerBase<TEvent> : IAsyncEventHandler<TEvent>
	where TEvent : IEvent
{
	public virtual Type? InterceptorType { get; }

	public abstract Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default);
}
