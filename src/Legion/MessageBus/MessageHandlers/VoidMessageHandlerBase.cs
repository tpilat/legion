using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.MessageHandlers;

public abstract class MessageHandlerBase<TRequestMessage> : IMessageHandler<TRequestMessage>
	where TRequestMessage : IRequestMessage
{
	public virtual Type? InterceptorType { get; }

	public abstract IResult Handle(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider);
}
