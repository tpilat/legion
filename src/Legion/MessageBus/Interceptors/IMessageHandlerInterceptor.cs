using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus.Interceptors;

/// <summary>
/// Defines a base interceptor for message handlers
/// </summary>
public interface IMessageHandlerInterceptor
{
}

/// <summary>
/// Defines an interceptor for message handlers
/// </summary>
public interface IMessageHandlerInterceptor<TRequestMessage, TResponse> : IMessageHandlerInterceptor
	where TRequestMessage : IRequestMessage<TResponse>
{
	/// <summary>
	/// Intercepts the message handler handle method
	/// </summary>
	IResult<TResponse> InterceptHandle(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TRequestMessage, IConnectionProvider, IResult<TResponse>> next);
}

/// <summary>
/// Defines an interceptor for message handlers
/// </summary>
public interface IAsyncMessageHandlerInterceptor<TRequestMessage, TResponse> : IMessageHandlerInterceptor
	where TRequestMessage : IRequestMessage<TResponse>
{
	/// <summary>
	/// Intercepts the message handler handle method
	/// </summary>
	Task<IResult<TResponse>> InterceptHandleAsync(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TRequestMessage, IConnectionProvider, CancellationToken, Task<IResult<TResponse>>> next,
		CancellationToken cancellationToken);
}

/// <summary>
/// Defines an interceptor for message handlers
/// </summary>
public interface IMessageHandlerInterceptor<TRequestMessage> : IMessageHandlerInterceptor
	where TRequestMessage : IRequestMessage
{
	/// <summary>
	/// Intercepts the message handler handle method
	/// </summary>
	IResult InterceptHandle(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TRequestMessage, IConnectionProvider, IResult> next);
}

/// <summary>
/// Defines an interceptor for message handlers
/// </summary>
public interface IAsyncMessageHandlerInterceptor<TRequestMessage> : IMessageHandlerInterceptor
	where TRequestMessage : IRequestMessage
{
	/// <summary>
	/// Intercepts the message handler handle method
	/// </summary>
	Task<IResult> InterceptHandleAsync(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TRequestMessage, IConnectionProvider, CancellationToken, Task<IResult>> next,
		CancellationToken cancellationToken);
}
