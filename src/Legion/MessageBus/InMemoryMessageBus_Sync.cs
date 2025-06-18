using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus;
using Legion.MessageBus.Messages;
using Legion.MessageBus.Processors;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Legion.Bus;

internal partial class InMemoryMessageBus<TConnectionStringProvider> : IMessageBus<TConnectionStringProvider>
	where TConnectionStringProvider : class, IConnectionStringProvider
{
	private static readonly ConcurrentDictionary<Type, MessageHandlerProcessorBase> _messageSyncProcessors = new();
	private static readonly ConcurrentDictionary<Type, EventHandlerProcessorBase> _eventSyncProcessors = new();

	public IResult Send(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return Send(invocationContext, message, connectionProvider, null);
	}

	public IResult Send(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null)
		=> Send(invocationContext, message, connectionProvider, null);


	public IResult Send(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return Send(invocationContext, message, connectionProvider, callOptions);
	}

	public IResult Send(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions)
	{
		var result = new ResultBuilder();

		if (result.IsArgumentNull(ScopeContext.Create(nameof(InMemoryMessageBus<TConnectionStringProvider>)), invocationContext))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, message))
			return result.Build();

		if (connectionProvider != null)
			Throw.IfArgumentNull(connectionProvider.TransactionsController);

		invocationContext = invocationContext.InvocationCreateNewWithLogger(_logger);

		ITransactionsController? transactionsController;
		var isLocalConnectionProvider = false;

		connectionProvider ??= ConnectionProvider;

		if (connectionProvider == null)
		{
			var connectionProviderFactory = invocationContext.ServiceProvider!.GetRequiredService<IConnectionProviderFactory>();
			transactionsController = new TransactionsController();
			isLocalConnectionProvider = true;

			connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
				invocationContext.ServiceProvider!,
				invocationContext.TargetStoreId, //If null, default conncetionString will be used
				transactionsController,
				transactionIsolationLevel: null,
				true,
				true);
		}
		else
		{
			transactionsController = connectionProvider.TransactionsController;
		}

		try
		{
			var requestMessageType = message.GetType();

			var handlerProcessor = (VoidMessageHandlerProcessor)_messageSyncProcessors.GetOrAdd(
				requestMessageType,
				requestMessageType =>
				{
					var processorType = typeof(VoidMessageHandlerProcessor<>).MakeGenericType(requestMessageType);
					var processor = Activator.CreateInstance(processorType) as MessageHandlerProcessorBase;

					if (processor == null)
						result
							.WithError(
								invocationContext,
								Exceptions.Internal.ErrorCodes.Bus.CreateHandlerProcessorException(processorType.ToFriendlyFullName()));

					return processor!;
				});

			if (result.HasError())
				return result.Build();

			var handlerResult = handlerProcessor.Handle<TConnectionStringProvider>(invocationContext, message, connectionProvider, callOptions);
			result.MergeAll(handlerResult);

			if (result.HasTransactionRollbackError())
			{
				var rollbackResult = transactionsController!.RollbackAll(invocationContext, null, TransactionsControllerStatus.CommitInProgress);
				result.MergeHasError(rollbackResult);
			}
			else
			{
				if (isLocalConnectionProvider)
				{
					var commitResult = transactionsController!.CommitAll(invocationContext, TransactionsControllerStatus.NotIdle);
					if (result.MergeHasError(commitResult))
					{
						var rollbackResult = transactionsController.RollbackAll(invocationContext, null, TransactionsControllerStatus.CommitInProgress);
						result.MergeHasError(rollbackResult);
					}
				}
			}

			return result.Build();
		}
		finally
		{
			if (isLocalConnectionProvider)
			{
				connectionProvider?.Dispose();
				transactionsController?.Dispose();
			}
		}
	}

	public IResult<TResponse> Send<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return Send(invocationContext, message, connectionProvider, null);
	}

	public IResult<TResponse> Send<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null)
		=> Send(invocationContext, message, connectionProvider, null);

	public IResult<TResponse> Send<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return Send(invocationContext, message, connectionProvider, callOptions);
	}

	public IResult<TResponse> Send<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions)
	{
		var result = new ResultBuilder<TResponse>();

		if (result.IsArgumentNull(ScopeContext.Create(nameof(InMemoryMessageBus<TConnectionStringProvider>)), invocationContext))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, message))
			return result.Build();

		if (connectionProvider != null)
			Throw.IfArgumentNull(connectionProvider.TransactionsController);

		invocationContext = invocationContext.InvocationCreateNewWithLogger(_logger);

		ITransactionsController? transactionsController;
		var isLocalConnectionProvider = false;

		connectionProvider ??= ConnectionProvider;

		if (connectionProvider == null)
		{
			var connectionProviderFactory = invocationContext.ServiceProvider!.GetRequiredService<IConnectionProviderFactory>();
			transactionsController = new TransactionsController();
			isLocalConnectionProvider = true;

			connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
				invocationContext.ServiceProvider!,
				invocationContext.TargetStoreId, //If null, default conncetionString will be used
				transactionsController,
				transactionIsolationLevel: null,
				true,
				true);
		}
		else
		{
			transactionsController = connectionProvider.TransactionsController;
		}

		try
		{
			var requestMessageType = message.GetType();

			var handlerProcessor = (MessageHandlerProcessor<TResponse>)_messageSyncProcessors.GetOrAdd(
				requestMessageType,
				requestMessageType =>
				{
					var processorType = typeof(MessageHandlerProcessor<,>).MakeGenericType(requestMessageType, typeof(TResponse));
					var processor = Activator.CreateInstance(processorType) as MessageHandlerProcessorBase;

					if (processor == null)
						result
							.WithError(
								invocationContext,
								Exceptions.Internal.ErrorCodes.Bus.CreateHandlerProcessorException(processorType.ToFriendlyFullName()));

					return processor!;
				});

			if (result.HasError())
				return result.Build();

			var handlerResult = handlerProcessor.Handle<TConnectionStringProvider>(invocationContext, message, connectionProvider, callOptions);
			result.MergeAllWithData(handlerResult);

			if (result.HasTransactionRollbackError())
			{
				var rollbackResult = transactionsController!.RollbackAll(invocationContext, null, TransactionsControllerStatus.CommitInProgress);
				result.MergeHasError(rollbackResult);
			}
			else
			{
				if (isLocalConnectionProvider)
				{
					var commitResult = transactionsController!.CommitAll(invocationContext, TransactionsControllerStatus.NotIdle);
					if (result.MergeHasError(commitResult))
					{
						var rollbackResult = transactionsController.RollbackAll(invocationContext, null, TransactionsControllerStatus.CommitInProgress);
						result.MergeHasError(rollbackResult);
					}
				}
			}

			return result.Build();
		}
		finally
		{
			if (isLocalConnectionProvider)
			{
				connectionProvider?.Dispose();
				transactionsController?.Dispose();
			}
		}
	}

	public IResult<bool> Publish(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return Publish(invocationContext, @event, connectionProvider, null);
	}

	public IResult<bool> Publish(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null)
		=> Publish(invocationContext, @event, connectionProvider, null);

	public IResult<bool> Publish(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return Publish(invocationContext, @event, connectionProvider, callOptions);
	}

	public IResult<bool> Publish(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(ScopeContext.Create(nameof(InMemoryMessageBus<TConnectionStringProvider>)), invocationContext))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, invocationContext.ServiceProvider))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, @event))
			return result.Build();

		if (connectionProvider != null)
			Throw.IfArgumentNull(connectionProvider.TransactionsController);

		invocationContext = invocationContext.InvocationCreateNewWithLogger(_logger);

		ITransactionsController? transactionsController;
		var isLocalConnectionProvider = false;

		connectionProvider ??= ConnectionProvider;

		if (connectionProvider == null)
		{
			var connectionProviderFactory = invocationContext.ServiceProvider!.GetRequiredService<IConnectionProviderFactory>();
			transactionsController = new TransactionsController();
			isLocalConnectionProvider = true;

			connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
				invocationContext.ServiceProvider!,
				invocationContext.TargetStoreId, //If null, default conncetionString will be used
				transactionsController,
				transactionIsolationLevel: null,
				true,
				true);
		}
		else
		{
			transactionsController = connectionProvider.TransactionsController;
		}

		try
		{
			var eventType = @event.GetType();

			var handlerProcessor = (EventHandlerProcessor)_eventSyncProcessors.GetOrAdd(
				eventType,
				eventType =>
				{
					var processorType = typeof(EventHandlerProcessor<>).MakeGenericType(eventType);
					var processor = Activator.CreateInstance(processorType) as EventHandlerProcessor;

					if (processor == null)
						result
							.WithError(
								invocationContext,
								Exceptions.Internal.ErrorCodes.Bus.CreateHandlerProcessorException(processorType.ToFriendlyFullName()));

					return processor!;
				});

			if (result.HasError())
				return result.Build();

			var handlerResult = handlerProcessor.Handle<TConnectionStringProvider>(invocationContext, @event, connectionProvider, callOptions);
			result.MergeAllWithData(handlerResult);

			if (result.HasTransactionRollbackError())
			{
				var rollbackResult = transactionsController!.RollbackAll(invocationContext, null, TransactionsControllerStatus.CommitInProgress);
				result.MergeHasError(rollbackResult);
			}
			else
			{
				if (isLocalConnectionProvider)
				{
					var commitResult = transactionsController!.CommitAll(invocationContext, TransactionsControllerStatus.NotIdle);
					if (result.MergeHasError(commitResult))
					{
						var rollbackResult = transactionsController.RollbackAll(invocationContext, null, TransactionsControllerStatus.CommitInProgress);
						result.MergeHasError(rollbackResult);
					}
				}
			}

			return result.Build();
		}
		finally
		{
			if (isLocalConnectionProvider)
			{
				connectionProvider?.Dispose();
				transactionsController?.Dispose();
			}
		}
	}
}
