using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.MessageBus.Processors;

internal abstract class AsyncVoidMessageHandlerProcessor : MessageHandlerProcessorBase
{
	public abstract Task<IResult> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		where TConnectionStringProvider : class, IConnectionStringProvider;
}

internal class AsyncVoidMessageHandlerProcessor<TRequestMessage> : AsyncVoidMessageHandlerProcessor
	where TRequestMessage : Messages.IRequestMessage
{
	protected override IMessageHandler CreateHandler(IServiceProvider serviceProvider)
	{
		var handler = serviceProvider.GetRequiredService<IAsyncMessageHandler<TRequestMessage>>();
		return handler;
	}

	public override Task<IResult> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		=> HandleAsync<TConnectionStringProvider>(invocationContext, (TRequestMessage)message, connectionProvider, callOptions, cancellationToken);

	public async Task<IResult> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var resultBuilder = new ResultBuilder();

		if (resultBuilder.IsArgumentNull(ScopeContext.Create(nameof(AsyncVoidMessageHandlerProcessor<TRequestMessage>)), invocationContext))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, message))
			return resultBuilder.Build();

		IAsyncMessageHandler<TRequestMessage>? handler = null;

		try
		{
			handler = (IAsyncMessageHandler<TRequestMessage>)CreateHandler(invocationContext.ServiceProvider);
		}
		catch (Exception exHandler)
		{
			resultBuilder
				.WithCriticalError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.CreateHandlerException(typeof(IAsyncMessageHandler<TRequestMessage>).ToFriendlyFullName()),
					x => x.ExceptionInfo(exHandler));

			var res = invocationContext.LogResultErrorMessages(resultBuilder.Build());
			resultBuilder.MergeErrors(res);

			return resultBuilder.Build();
		}

		try
		{
			IResult result;
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
				var interceptor = (IAsyncMessageHandlerInterceptor<TRequestMessage>)invocationContext.ServiceProvider.GetRequiredService(interceptorType);
				result = await interceptor.InterceptHandleAsync(invocationContext, message, connectionProvider, handler.HandleAsync, cancellationToken).ConfigureAwait(false);
			}

			resultBuilder.MergeAll(result);

			return resultBuilder.Build();
		}
		catch (Exception ex)
		{
			resultBuilder
				.WithError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.UnhandledHandlerForMessageException(message.GetType().ToFriendlyFullName(), typeof(IAsyncMessageHandler<TRequestMessage>).ToFriendlyFullName()),
					x => x.ExceptionInfo(ex));
		}

		var logResult = invocationContext.LogResultAllMessages(resultBuilder.Build());
		resultBuilder.MergeErrors(logResult);

		return resultBuilder.Build();
	}
}
