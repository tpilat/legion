using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus;
using Legion.MessageBus.Messages;
using Legion.MessageBus.Processors;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.Bus;

internal partial class InMemoryMessageBus<TConnectionStringProvider> : IMessageBus<TConnectionStringProvider>
	where TConnectionStringProvider : class, IConnectionStringProvider
{
	public async Task<IResult> SendAsync(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return await SendAsync(invocationContext, message, connectionProvider, null, cancellationToken);
	}

	public Task<IResult> SendAsync(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default)
		=> SendAsync(invocationContext, message, connectionProvider, null, cancellationToken);

	public async Task<IResult> SendAsync(
		IScopeContext scopeContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return await SendAsync(invocationContext, message, connectionProvider, callOptions, cancellationToken);
	}

	public async Task<IResult> SendAsync(
		IInvocationContext invocationContext,
		IRequestMessage message,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
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

		invocationContext = invocationContext.InvocationCreateNewWith(logger: _logger, cancellationToken: cancellationToken);

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

			var handlerProcessor = (AsyncVoidMessageHandlerProcessor)_messageAsyncProcessors.GetOrAdd(
				requestMessageType,
				requestMessageType =>
				{
					var processorType = typeof(AsyncVoidMessageHandlerProcessor<>).MakeGenericType(requestMessageType);
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

			var handlerResult = await handlerProcessor.HandleAsync<TConnectionStringProvider>(invocationContext, message, connectionProvider, callOptions, cancellationToken).ConfigureAwait(false);
			result.MergeAll(handlerResult);

			if (result.HasTransactionRollbackError())
			{
				var rollbackResult = await transactionsController!.RollbackAllAsync(invocationContext, null, TransactionsControllerStatus.CommitInProgress, cancellationToken: default);
				result.MergeHasError(rollbackResult);
			}
			else
			{
				if (isLocalConnectionProvider)
				{
					var commitResult = await transactionsController!.CommitAllAsync(invocationContext, TransactionsControllerStatus.NotIdle, cancellationToken: default);
					if (result.MergeHasError(commitResult))
					{
						var rollbackResult = await transactionsController.RollbackAllAsync(invocationContext, null, TransactionsControllerStatus.CommitInProgress, cancellationToken: default);
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
				if (connectionProvider != null)
					await connectionProvider.DisposeAsync();

				if (transactionsController != null)
					await transactionsController.DisposeAsync();
			}
		}
	}

	public async Task<IResult<TResponse>> SendAsync<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return await SendAsync(invocationContext, message, connectionProvider, null, cancellationToken);
	}

	public Task<IResult<TResponse>> SendAsync<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default)
		=> SendAsync(invocationContext, message, connectionProvider, null, cancellationToken);

	public async Task<IResult<TResponse>> SendAsync<TResponse>(
		IScopeContext scopeContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null,
		CallOptions? callOptions = null,
		CancellationToken cancellationToken = default)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return await SendAsync(invocationContext, message, connectionProvider, callOptions, cancellationToken);
	}

	public async Task<IResult<TResponse>> SendAsync<TResponse>(
		IInvocationContext invocationContext,
		IRequestMessage<TResponse> message,
		IConnectionProvider? connectionProvider = null,
		CallOptions? callOptions = null,
		CancellationToken cancellationToken = default)
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

		invocationContext = invocationContext.InvocationCreateNewWith(logger: _logger, cancellationToken: cancellationToken);

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

			var handlerProcessor = (AsyncMessageHandlerProcessor<TResponse>)_messageAsyncProcessors.GetOrAdd(
				requestMessageType,
				requestMessageType =>
				{
					var processorType = typeof(AsyncMessageHandlerProcessor<,>).MakeGenericType(requestMessageType, typeof(TResponse));
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

			var handlerResult = await handlerProcessor.HandleAsync<TConnectionStringProvider>(invocationContext, message, connectionProvider, callOptions, cancellationToken).ConfigureAwait(false);
			result.MergeAllWithData(handlerResult);

			if (result.HasTransactionRollbackError())
			{
				var rollbackResult = await transactionsController!.RollbackAllAsync(invocationContext, null, TransactionsControllerStatus.CommitInProgress, cancellationToken: default);
				result.MergeHasError(rollbackResult);
			}
			else
			{
				if (isLocalConnectionProvider)
				{
					var commitResult = await transactionsController!.CommitAllAsync(invocationContext, TransactionsControllerStatus.NotIdle, cancellationToken: default);
					if (result.MergeHasError(commitResult))
					{
						var rollbackResult = await transactionsController.RollbackAllAsync(invocationContext, null, TransactionsControllerStatus.CommitInProgress, cancellationToken: default);
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
				if (connectionProvider != null)
					await connectionProvider.DisposeAsync();

				if (transactionsController != null)
					await transactionsController.DisposeAsync();
			}
		}
	}

	public async Task<IResult<bool>> PublishAsync(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return await PublishAsync(invocationContext, @event, connectionProvider, null, cancellationToken);
	}

	public Task<IResult<bool>> PublishAsync(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider = null,
		CancellationToken cancellationToken = default)
		=> PublishAsync(invocationContext, @event, connectionProvider, null, cancellationToken);

	public async Task<IResult<bool>> PublishAsync(
		IScopeContext scopeContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
	{
		var invocationContext = new InvocationContextBuilder(scopeContext).Initialize(_serviceProvider).Build();
		return await PublishAsync(invocationContext, @event, connectionProvider, callOptions, cancellationToken);
	}

	public async Task<IResult<bool>> PublishAsync(
		IInvocationContext invocationContext,
		IEvent @event,
		IConnectionProvider? connectionProvider,
		CallOptions? callOptions,
		CancellationToken cancellationToken = default)
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

		invocationContext = invocationContext.InvocationCreateNewWith(logger: _logger, cancellationToken: cancellationToken);

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

			var handlerProcessor = (AsyncEventHandlerProcessor)_eventAsyncProcessors.GetOrAdd(
				eventType,
				eventType =>
				{
					var processorType = typeof(AsyncEventHandlerProcessor<>).MakeGenericType(eventType);
					var processor = Activator.CreateInstance(processorType) as AsyncEventHandlerProcessor;

					if (processor == null)
						result
							.WithError(
								invocationContext,
								Exceptions.Internal.ErrorCodes.Bus.CreateHandlerProcessorException(processorType.ToFriendlyFullName()));

					return processor!;
				});

			if (result.HasError())
				return result.Build();

			var handlerResult = await handlerProcessor.HandleAsync<TConnectionStringProvider>(invocationContext, @event, connectionProvider, callOptions, cancellationToken).ConfigureAwait(false);
			result.MergeAllWithData(handlerResult);

			if (result.HasTransactionRollbackError())
			{
				var rollbackResult = await transactionsController!.RollbackAllAsync(invocationContext, null, TransactionsControllerStatus.CommitInProgress, cancellationToken: default);
				result.MergeHasError(rollbackResult);
			}
			else
			{
				if (isLocalConnectionProvider)
				{
					var commitResult = await transactionsController!.CommitAllAsync(invocationContext, TransactionsControllerStatus.NotIdle, cancellationToken: default);
					if (result.MergeHasError(commitResult))
					{
						var rollbackResult = await transactionsController.RollbackAllAsync(invocationContext, null, TransactionsControllerStatus.CommitInProgress, cancellationToken: default);
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
				if (connectionProvider != null)
					await connectionProvider.DisposeAsync();

				if (transactionsController != null)
					await transactionsController.DisposeAsync();
			}
		}
	}
}
