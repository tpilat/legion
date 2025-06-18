using Legion.Database;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.Messages;
using Legion.Model.Repositories;
using System.Data;

namespace Legion.MessageBus.MessageHandlers;

/// <summary>
/// Defines a base handler for a request messages
/// </summary>
public interface IMessageHandler
{
}

/// <summary>
/// Defines a handler for a request message with response message
/// </summary>
public interface IMessageHandler<TRequestMessage, TResponse> : IMessageHandler
	where TRequestMessage : IRequestMessage<TResponse>
{
	/// <summary>
	/// Interceptor for handle method. Interceptor must implement <see cref="IMessageHandlerInterceptor{TRequestMessage, TResponse}"/>
	/// </summary>
	Type? InterceptorType { get; }

	/// <summary>
	/// Handles a request message
	/// </summary>
	/// <returns>Response from the request message</returns>
	IResult<TResponse> Handle(IInvocationContext invocationContext, TRequestMessage message, IConnectionProvider connectionProvider);
}

/// <summary>
/// Defines a handler for a request message with response message
/// </summary>
public interface IAsyncMessageHandler<TRequestMessage, TResponse> : IMessageHandler
	where TRequestMessage : IRequestMessage<TResponse>
{
	/// <summary>
	/// Interceptor for handle method. Interceptor must implement <see cref="IAsyncMessageHandlerInterceptor{TRequestMessage, TResponse}"/>
	/// </summary>
	Type? InterceptorType { get; }

	/// <summary>
	/// Handles a request message
	/// </summary>
	/// <returns>Response from the request message</returns>
	Task<IResult<TResponse>> HandleAsync(IInvocationContext invocationContext, TRequestMessage message, IConnectionProvider connectionProvider, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a handler for a request message with no response message
/// </summary>
public interface IMessageHandler<TRequestMessage> : IMessageHandler
	where TRequestMessage : IRequestMessage
{
	/// <summary>
	/// Interceptor for handle method. Interceptor must implement <see cref="IMessageHandlerInterceptor{TRequestMessage, TResponse}"/>
	/// </summary>
	Type? InterceptorType { get; }

	/// <summary>
	/// Handles a request message
	/// </summary>
	/// <returns>Response from the request message</returns>
	IResult Handle(IInvocationContext invocationContext, TRequestMessage message, IConnectionProvider connectionProvider);
}

/// <summary>
/// Defines a handler for a request message with no response message
/// </summary>
public interface IAsyncMessageHandler<TRequestMessage> : IMessageHandler
	where TRequestMessage : IRequestMessage
{
	/// <summary>
	/// Interceptor for handle method. Interceptor must implement <see cref="IAsyncMessageHandlerInterceptor{TRequestMessage, TResponse}"/>
	/// </summary>
	Type? InterceptorType { get; }

	/// <summary>
	/// Handles a request message
	/// </summary>
	/// <returns>Response from the request message</returns>
	Task<IResult> HandleAsync(IInvocationContext invocationContext, TRequestMessage message, IConnectionProvider connectionProvider, CancellationToken cancellationToken = default);
}

