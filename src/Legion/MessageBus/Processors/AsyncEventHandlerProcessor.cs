using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Legion.MessageBus.Messages;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.MessageBus.Processors;

internal abstract class AsyncEventHandlerProcessor : EventHandlerProcessorBase
{
	public abstract Task<IResult<bool>> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		where TConnectionStringProvider : class, IConnectionStringProvider;
}

internal class AsyncEventHandlerProcessor<TEvent> : AsyncEventHandlerProcessor
	where TEvent : IEvent
{
	protected override IEnumerable<IEventHandler> CreateHandlers(IServiceProvider serviceProvider)
	{
		var handlers = serviceProvider.GetServices<IAsyncEventHandler<TEvent>>();
		return handlers;
	}

	public override Task<IResult<bool>> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		=> HandleAsync<TConnectionStringProvider>(invocationContext, (TEvent)@event, connectionProvider, callOptions, cancellationToken);

	public async Task<IResult<bool>> HandleAsync<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var resultBuilder = new ResultBuilder<bool>();

		if (resultBuilder.IsArgumentNull(ScopeContext.Create(nameof(AsyncEventHandlerProcessor<TEvent>)), invocationContext))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, @event))
			return resultBuilder.Build();

		IEnumerable<IAsyncEventHandler<TEvent>> handlersAndUowProviders = [];

		try
		{
			handlersAndUowProviders = CreateHandlers(invocationContext.ServiceProvider).Select(x => (IAsyncEventHandler<TEvent>)x);
		}
		catch (Exception exHandler)
		{
			resultBuilder
				.WithCriticalError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.CreateHandlerException(typeof(IAsyncEventHandler<TEvent>).ToFriendlyFullName()),
					x => x.ExceptionInfo(exHandler));

			var res = invocationContext.LogResultErrorMessages(resultBuilder.Build());
			resultBuilder.MergeErrors(res);

			return resultBuilder.Build();
		}

		if (handlersAndUowProviders.Any())
		{
			resultBuilder.WithData(true);
			var tasks = handlersAndUowProviders
				.Select(async handler =>
				{
					try
					{
						IResult result;
						var interceptorType = handler.InterceptorType;
						if (interceptorType == null)
						{
							using var scope = invocationContext.CreateLoggerScope();

							if (callOptions == null)
							{
								result = await Result.CallAsync(handler.HandleAsync, invocationContext, @event!, connectionProvider, cancellationToken);
							}
							else
							{
								result = await Result.CallAsync(handler.HandleAsync, invocationContext, @event!, connectionProvider, callOptions, cancellationToken);
							}
						}
						else
						{
							var interceptor = (IAsyncEventHandlerInterceptor<TEvent>)invocationContext.ServiceProvider.GetRequiredService(interceptorType);
							result = await interceptor.InterceptHandleAsync(invocationContext, @event, connectionProvider, handler.HandleAsync, cancellationToken).ConfigureAwait(false);
						}

						resultBuilder.MergeAll(result);
					}
					catch (Exception ex)
					{
						resultBuilder
							.WithError(
								invocationContext,
								Exceptions.Internal.ErrorCodes.Bus.UnhandledHandlerForEventException(@event.GetType().ToFriendlyFullName(), typeof(IAsyncEventHandler<TEvent>).ToFriendlyFullName()),
								x => x.ExceptionInfo(ex));
					}
				})
				.ToArray();

			await Task.WhenAll(tasks);

			var logResult = invocationContext.LogResultAllMessages(resultBuilder.Build());
			resultBuilder.MergeErrors(logResult);
		}
		else
		{
			resultBuilder.WithData(false);
			invocationContext.Logger?.LogWarningMessage(invocationContext, null, x => x.InternalMessage($"No event hadler of type {typeof(IAsyncEventHandler<TEvent>).ToFriendlyFullName()} resolved"));
		}

		return resultBuilder.Build();
	}
}
