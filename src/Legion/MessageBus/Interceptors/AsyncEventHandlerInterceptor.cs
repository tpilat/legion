using Legion.Database;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.Logging;

namespace Legion.MessageBus.Interceptors;

public abstract class AsyncEventHandlerInterceptor<TEvent> : IAsyncEventHandlerInterceptor<TEvent>, IEventHandlerInterceptor
	where TEvent : IEvent
{
	protected ILogger Logger { get; }

	public AsyncEventHandlerInterceptor(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public virtual Task<IResult> InterceptHandleAsync(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TEvent, IConnectionProvider, CancellationToken, Task<IResult>> next,
		CancellationToken cancellationToken)
	{
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable L_SCOPE_SET // Type or member is obsolete
		invocationContext.InvocationSetLogger(Logger, false);
#pragma warning restore L_SCOPE_SET // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete

		using var loggerScope = invocationContext.CreateLoggerScope();

		return Result.CallAsync(next, invocationContext, @event!, connectionProvider, cancellationToken);
	}
}
