using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.MessageBus.Processors;

internal abstract class EventHandlerProcessor : EventHandlerProcessorBase
{
	public abstract IResult<bool> Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		IEvent @vent,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		where TConnectionStringProvider : class, IConnectionStringProvider;
}

internal class EventHandlerProcessor<TEvent> : EventHandlerProcessor
	where TEvent : IEvent
{
	protected override IEnumerable<IEventHandler> CreateHandlers(IServiceProvider serviceProvider)
	{
		var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();
		return handlers;
	}

	public override IResult<bool> Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		=> Handle<TConnectionStringProvider>(invocationContext, (TEvent)@event, connectionProvider, callOptions);

	public IResult<bool> Handle<TConnectionStringProvider>(
		IInvocationContext invocationContext,
		TEvent @event,
		IConnectionProvider connectionProvider,
		CallOptions? callOptions)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var resultBuilder = new ResultBuilder<bool>();

		if (resultBuilder.IsArgumentNull(ScopeContext.Create(nameof(EventHandlerProcessor<TEvent>)), invocationContext))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return resultBuilder.Build();

		if (resultBuilder.IsArgumentNull(invocationContext, @event))
			return resultBuilder.Build();

		IEnumerable<IEventHandler<TEvent>> handlersAndUowProviders = [];

		try
		{
			handlersAndUowProviders = CreateHandlers(invocationContext.ServiceProvider).Select(x => (IEventHandler<TEvent>)x);
		}
		catch (Exception exHandler)
		{
			resultBuilder
				.WithCriticalError(
					invocationContext,
					Exceptions.Internal.ErrorCodes.Bus.CreateHandlerException(typeof(IEventHandler<TEvent>).ToFriendlyFullName()),
					x => x.ExceptionInfo(exHandler));

			var res = invocationContext.LogResultErrorMessages(resultBuilder.Build());
			resultBuilder.MergeErrors(res);

			return resultBuilder.Build();
		}

		if (handlersAndUowProviders.Any())
		{
			resultBuilder.WithData(true);

			var parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = 2 //TODO add to global/static bus config
			};

			Parallel.ForEach(handlersAndUowProviders, parallelOptions, handler =>
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
							result = Result.Call(handler.Handle, invocationContext, @event, connectionProvider);
						}
						else
						{
							result = Result.Call(handler.Handle, invocationContext, @event, connectionProvider, callOptions);
						}
					}
					else
					{
						var interceptor = (IEventHandlerInterceptor<TEvent>)invocationContext.ServiceProvider.GetRequiredService(interceptorType);
						result = interceptor.InterceptHandle(invocationContext, @event, connectionProvider, handler.Handle);
					}

					resultBuilder.MergeAll(result);
				}
				catch (Exception ex)
				{
					resultBuilder
						.WithError(
							invocationContext,
							Exceptions.Internal.ErrorCodes.Bus.UnhandledHandlerForEventException(@event.GetType().ToFriendlyFullName(), typeof(IEventHandler<TEvent>).ToFriendlyFullName()),
							x => x.ExceptionInfo(ex));
				}
			});

			var logResult = invocationContext.LogResultAllMessages(resultBuilder.Build());
			resultBuilder.MergeErrors(logResult);
		}
		else
		{
			resultBuilder.WithData(false);
			invocationContext.Logger?.LogWarningMessage(invocationContext, null, x => x.InternalMessage($"No event hadler of type {typeof(IEventHandler<TEvent>).ToFriendlyFullName()} resolved"));
		}

		return resultBuilder.Build();
	}
}
