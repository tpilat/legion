using Legion.Database;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.Logging;

namespace Legion.MessageBus.Interceptors;

public abstract class AsyncVoidMessageHandlerInterceptor<TRequestMessage> : IAsyncMessageHandlerInterceptor<TRequestMessage>, IMessageHandlerInterceptor
	where TRequestMessage : IRequestMessage
{
	protected ILogger Logger { get; }

	public AsyncVoidMessageHandlerInterceptor(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public virtual Task<IResult> InterceptHandleAsync(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TRequestMessage, IConnectionProvider, CancellationToken, Task<IResult>> next,
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
