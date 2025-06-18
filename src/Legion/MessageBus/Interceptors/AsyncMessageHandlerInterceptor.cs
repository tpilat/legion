using Legion.Database;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.Logging;

namespace Legion.MessageBus.Interceptors;

public abstract class AsyncMessageHandlerInterceptor<TRequestMessage, TResponse> : IAsyncMessageHandlerInterceptor<TRequestMessage, TResponse>, IMessageHandlerInterceptor
	where TRequestMessage : IRequestMessage<TResponse>
{
	protected ILogger Logger { get; }

	public AsyncMessageHandlerInterceptor(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public virtual Task<IResult<TResponse>> InterceptHandleAsync(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TRequestMessage, IConnectionProvider, CancellationToken, Task<IResult<TResponse>>> next,
		CancellationToken cancellationToken)
	{
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable L_SCOPE_SET // Type or member is obsolete
		invocationContext.InvocationSetLogger(Logger, false);
#pragma warning restore L_SCOPE_SET // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete

		using var loggerScope = invocationContext.CreateLoggerScope();

		return Result.CallAsync(next, invocationContext, message!, connectionProvider, cancellationToken);
	}
}
