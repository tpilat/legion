using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.MessageBus.Processors;

internal abstract class MessageHandlerProcessor<TResponse> : MessageHandlerProcessorBase
{
	public abstract IResult<TResponse> Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage<TResponse> message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		where TConnectionStringProvider : class, IConnectionStringProvider;
}

internal class MessageHandlerProcessor<TRequestMessage, TResponse> : MessageHandlerProcessor<TResponse>
	where TRequestMessage : Messages.IRequestMessage<TResponse>
{
	protected override IMessageHandler CreateHandler(IServiceProvider serviceProvider)
	{
		var handler = serviceProvider.GetRequiredService<IMessageHandler<TRequestMessage, TResponse>>();
		return handler;
	}

	public override IResult<TResponse> Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage<TResponse> message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		=> Handle<TConnectionStringProvider>(invocationContext, (TRequestMessage)message, connectionProvider, callOptions);

	public IResult<TResponse> Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var resultBuilder = new ResultBuilder<TResponse>();

		if (resultBuilder.IsArgumentNull(ScopeContext.Create(nameof(MessageHandlerProcessor<TRequestMessage, TResponse>)), invocationContext))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, message))
			return resultBuilder.Build();

		IMessageHandler<TRequestMessage, TResponse>? handler = null;

		try
		{
			handler = (IMessageHandler<TRequestMessage, TResponse>)CreateHandler(invocationContext.ServiceProvider);
		}
		catch (Exception exHandler)
		{
			resultBuilder
				.WithCriticalError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.CreateHandlerException(typeof(IMessageHandler<TRequestMessage, TResponse>).ToFriendlyFullName()),
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
					result = Result.Call(handler.Handle, invocationContext, message, connectionProvider);
				}
				else
				{
					result = Result.Call(handler.Handle, invocationContext, message, connectionProvider, callOptions);
				}
			}
			else
			{
				var interceptor = (IMessageHandlerInterceptor<TRequestMessage, TResponse>)invocationContext.ServiceProvider.GetRequiredService(interceptorType);
				result = interceptor.InterceptHandle(invocationContext, message, connectionProvider, handler.Handle);
			}

			resultBuilder.MergeAllWithData(result);
		}
		catch (Exception ex)
		{
			resultBuilder
				.WithError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.UnhandledHandlerForMessageException(message.GetType().ToFriendlyFullName(), typeof(IMessageHandler<TRequestMessage, TResponse>).ToFriendlyFullName()),
					x => x.ExceptionInfo(ex));
		}

		var logResult = invocationContext.LogResultAllMessages(resultBuilder.Build());
		resultBuilder.MergeErrors(logResult);

		return resultBuilder.Build();
	}
}
