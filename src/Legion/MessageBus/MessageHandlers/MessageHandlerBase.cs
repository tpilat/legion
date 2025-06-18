using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.MessageHandlers;

public abstract class MessageHandlerBase<TRequestMessage, TResponse> : IMessageHandler<TRequestMessage, TResponse>
	where TRequestMessage : IRequestMessage<TResponse>
{
	public virtual Type? InterceptorType { get; }

	public abstract IResult<TResponse> Handle(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider);
}
