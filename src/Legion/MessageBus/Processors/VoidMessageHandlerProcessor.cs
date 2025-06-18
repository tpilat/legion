using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.MessageBus.Processors;

internal abstract class VoidMessageHandlerProcessor : MessageHandlerProcessorBase
{
	public abstract IResult Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		where TConnectionStringProvider : class, IConnectionStringProvider;
}

internal class VoidMessageHandlerProcessor<TRequestMessage> : VoidMessageHandlerProcessor
	where TRequestMessage : Messages.IRequestMessage
{
	protected override IMessageHandler CreateHandler(IServiceProvider serviceProvider)
	{
		var handler = serviceProvider.GetRequiredService<IMessageHandler<TRequestMessage>>();
		return handler;
	}

	public override IResult Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		Messages.IRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		=> Handle<TConnectionStringProvider>(invocationContext, (TRequestMessage)message, connectionProvider, callOptions);

	public IResult Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var resultBuilder = new ResultBuilder();

		if (resultBuilder.IsArgumentNull(ScopeContext.Create(nameof(VoidMessageHandlerProcessor<TRequestMessage>)), invocationContext))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, message))
			return resultBuilder.Build();

		IMessageHandler<TRequestMessage>? handler = null;

		try
		{
			handler = (IMessageHandler<TRequestMessage>)CreateHandler(invocationContext.ServiceProvider);
		}
		catch (Exception exHandler)
		{
			resultBuilder
				.WithCriticalError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.CreateHandlerException(typeof(IMessageHandler<TRequestMessage>).ToFriendlyFullName()),
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
					result = Result.Call(handler.Handle, invocationContext, message, connectionProvider);
				}
				else
				{
					result = Result.Call(handler.Handle, invocationContext, message, connectionProvider, callOptions);
				}
			}
			else
			{
				var interceptor = (IMessageHandlerInterceptor<TRequestMessage>)invocationContext.ServiceProvider.GetRequiredService(interceptorType);
				result = interceptor.InterceptHandle(invocationContext, message, connectionProvider, handler.Handle);
			}

			resultBuilder.MergeAll(result);
		}
		catch (Exception ex)
		{
			resultBuilder
				.WithError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.UnhandledHandlerForMessageException(message.GetType().ToFriendlyFullName(), typeof(IMessageHandler<TRequestMessage>).ToFriendlyFullName()),
					x => x.ExceptionInfo(ex));
		}

		var logResult = invocationContext.LogResultAllMessages(resultBuilder.Build());
		resultBuilder.MergeErrors(logResult);

		return resultBuilder.Build();
	}
}
