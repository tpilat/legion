using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.MessageBus.Processors;

internal abstract class AsyncMessageHandlerProcessor<TResponse> : MessageHandlerProcessorBase
{
	public abstract Task<IResult<TResponse>> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage<TResponse> message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		where TConnectionStringProvider : class, IConnectionStringProvider;
}

internal class AsyncMessageHandlerProcessor<TRequestMessage, TResponse> : AsyncMessageHandlerProcessor<TResponse>
	where TRequestMessage : Messages.IRequestMessage<TResponse>
{
	protected override IMessageHandler CreateHandler(IServiceProvider serviceProvider)
	{
		var handler = serviceProvider.GetRequiredService<IAsyncMessageHandler<TRequestMessage, TResponse>>();
		return handler;
	}

	public override Task<IResult<TResponse>> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage<TResponse> message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		=> HandleAsync<TConnectionStringProvider>(invocationContext, (TRequestMessage)message, connectionProvider, callOptions, cancellationToken);

	public async Task<IResult<TResponse>> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var resultBuilder = new ResultBuilder<TResponse>();

		if (resultBuilder.IsArgumentNull(ScopeContext.Create(nameof(AsyncMessageHandlerProcessor<TRequestMessage, TResponse>)), invocationContext))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, message))
			return resultBuilder.Build();

		IAsyncMessageHandler<TRequestMessage, TResponse>? handler;

		try
		{
			handler = (IAsyncMessageHandler<TRequestMessage, TResponse>)CreateHandler(invocationContext.ServiceProvider);
		}
		catch (Exception exHandler)
		{
			resultBuilder
				.WithCriticalError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.CreateHandlerException(typeof(IAsyncMessageHandler<TRequestMessage, TResponse>).ToFriendlyFullName()),
					x => x.ExceptionInfo(exHandler));

			var res = invocationContext.LogResultErrorMessages(resultBuilder.Build());
			resultBuilder.MergeErrors(res);

			return resultBuilder.Build();
		}

		try
		{
			IResult<TResponse> result;
			var interceptorType = handler.InterceptorType;
			if (interceptorType == null)
			{
				using var scope = invocationContext.CreateLoggerScope();

				if (callOptions == null)
				{
					result = await Result.CallAsync(handler.HandleAsync, invocationContext, message!, connectionProvider, cancellationToken);
				}
				else
				{
					result = await Result.CallAsync(handler.HandleAsync, invocationContext, message!, connectionProvider, callOptions, cancellationToken);
				}
			}
			else
			{
				var interceptor = (IAsyncMessageHandlerInterceptor<TRequestMessage, TResponse>)invocationContext.ServiceProvider.GetRequiredService(interceptorType);
				result = await interceptor.InterceptHandleAsync(invocationContext, message, connectionProvider, handler.HandleAsync, cancellationToken).ConfigureAwait(false);
			}

			resultBuilder.MergeAllWithData(result);
		}
		catch (Exception ex)
		{
			resultBuilder
				.WithError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.UnhandledHandlerForMessageException(message.GetType().ToFriendlyFullName(), typeof(IAsyncMessageHandler<TRequestMessage, TResponse>).ToFriendlyFullName()),
					x => x.ExceptionInfo(ex));
		}

		var logResult = invocationContext.LogResultAllMessages(resultBuilder.Build());
		resultBuilder.MergeErrors(logResult);

		return resultBuilder.Build();
	}
}
