using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.MessageHandlers;

public abstract class AsyncMessageHandlerBase<TRequestMessage> : IAsyncMessageHandler<TRequestMessage>
	where TRequestMessage : IRequestMessage
{
	public virtual Type? InterceptorType { get; }

	public abstract Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default);
}
