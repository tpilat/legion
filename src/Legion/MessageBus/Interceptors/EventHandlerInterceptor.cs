using Legion.Database;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.Logging;

namespace Legion.MessageBus.Interceptors;

public abstract class EventHandlerInterceptor<TEvent> : IEventHandlerInterceptor<TEvent>, IEventHandlerInterceptor
	where TEvent : IEvent
{
	protected ILogger Logger { get; }

	public EventHandlerInterceptor(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public virtual IResult InterceptHandle(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TEvent, IConnectionProvider, IResult> next)
	{
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable L_SCOPE_SET // Type or member is obsolete
		invocationContext.InvocationSetLogger(Logger, false);
#pragma warning restore L_SCOPE_SET // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete

		using var loggerScope = invocationContext.CreateLoggerScope();

		return Result.Call(next, invocationContext, @event!, connectionProvider);
	}
}
