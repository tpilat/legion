using Legion.Database;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.Messages;
using Legion.Model.Repositories;
using System.Data;

namespace Legion.MessageBus.MessageHandlers;

/// <summary>
/// Defines a base handler for events
/// </summary>
public interface IEventHandler
{
}

/// <summary>
/// Defines a handler for an event
/// </summary>
public interface IEventHandler<TEvent> : IEventHandler
	where TEvent : IEvent
{
	/// <summary>
	/// Interceptor for handle method. Interceptor must implement <see cref="IEventHandlerInterceptor{TEvent}"/>
	/// </summary>
	Type? InterceptorType { get; }

	/// <summary>
	/// Handles an event
	/// </summary>
	/// <returns>Response from the event</returns>
	IResult Handle(IInvocationContext invocationContext, TEvent @event, IConnectionProvider connectionProvider);
}

/// <summary>
/// Defines a handler for an event
/// </summary>
public interface IAsyncEventHandler<TEvent> : IEventHandler
	where TEvent : IEvent
{
	/// <summary>
	/// Interceptor for handle method. Interceptor must implement <see cref="IAsyncEventHandlerInterceptor{TEvent}"/>
	/// </summary>
	Type? InterceptorType { get; }

	/// <summary>
	/// Handles an event
	/// </summary>
	/// <returns>Response from the event</returns>
	Task<IResult> HandleAsync(IInvocationContext invocationContext, TEvent @event, IConnectionProvider connectionProvider, CancellationToken cancellationToken = default);
}
