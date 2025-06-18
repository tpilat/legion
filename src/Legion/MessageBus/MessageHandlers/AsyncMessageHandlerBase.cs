using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.MessageHandlers;

public abstract class AsyncMessageHandlerBase<TRequestMessage, TResponse> : IAsyncMessageHandler<TRequestMessage, TResponse>
	where TRequestMessage : IRequestMessage<TResponse>
{
	public virtual Type? InterceptorType { get; }

	public abstract Task<IResult<TResponse>> HandleAsync(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default);
}
