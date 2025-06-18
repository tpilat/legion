using Legion.Database;
using Legion.MessageBus.Messages;

namespace Legion.MessageBus;

public interface IMessageBus<TConnectionStringProvider>
	where TConnectionStringProvider : class, IConnectionStringProvider
{
	IConnectionProvider? ConnectionProvider { get; }

	bool SetConnectionProvider(IConnectionProvider connectionProvider);

	Task<IResult> SendAsync(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default);

	Task<IResult> SendAsync(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default);

	Task<IResult<TResponse>> SendAsync<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default);

	Task<IResult<TResponse>> SendAsync<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default);

	Task<IResult> SendAsync(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default);

	Task<IResult> SendAsync(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default);

	Task<IResult<TResponse>> SendAsync<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default);

	Task<IResult<TResponse>> SendAsync<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default);

	IResult Send(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null);

	IResult Send(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null);

	IResult<TResponse> Send<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null);

	IResult<TResponse> Send<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null);

	IResult Send(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions);

	IResult Send(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions);

	IResult<TResponse> Send<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions);

	IResult<TResponse> Send<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions);


	Task<IResult<bool>> PublishAsync(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default);

	Task<IResult<bool>> PublishAsync(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default);

	IResult<bool> Publish(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null);

	IResult<bool> Publish(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null);


	Task<IResult<bool>> PublishAsync(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default);

	Task<IResult<bool>> PublishAsync(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default);

	IResult<bool> Publish(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions);

	IResult<bool> Publish(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions);

	bool CanSendOrPublish(Type type);
}

//public interface IMessageBus : IMessageBus<IConnectionStringProvider>
//{
//}
