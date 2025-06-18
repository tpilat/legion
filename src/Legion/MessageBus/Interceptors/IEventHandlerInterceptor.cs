using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.Interceptors;

/// <summary>
/// Defines a base interceptor for event handlers
/// </summary>
public interface IEventHandlerInterceptor
{
}

/// <summary>
/// Defines an interceptor for event handlers
/// </summary>
public interface IEventHandlerInterceptor<TEvent> : IEventHandlerInterceptor
	where TEvent : IEvent
{
	/// <summary>
	/// Intercepts the event handler handle method
	/// </summary>
	IResult InterceptHandle(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TEvent, IConnectionProvider, IResult> next);
}

/// <summary>
/// Defines an interceptor for event handlers
/// </summary>
public interface IAsyncEventHandlerInterceptor<TEvent> : IEventHandlerInterceptor
	where TEvent : IEvent
{
	/// <summary>
	/// Intercepts the event handler handle method
	/// </summary>
	Task<IResult> InterceptHandleAsync(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TEvent, IConnectionProvider, CancellationToken, Task<IResult>> next,
		CancellationToken cancellationToken);
}
