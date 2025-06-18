using Legion.ADF.Messaging.Outbox;
using Legion.ADF.Messaging.Outbox.Events;
using Legion.Extensions;

namespace Legion.ADF.Messaging;

public static class ADFMessagingOutboxBuilderExtensions
{
	public static ADFMessagingOutboxBuilder AddOutboxMessageType<T>(
		this ADFMessagingOutboxBuilder builder,
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		builder.OutboxMessageTypeRegistry.RegisterOutboxMessageType<T>(scopeContext);

		return builder;
	}

	public static ADFMessagingOutboxBuilder AddOutboxMessageType(
		this ADFMessagingOutboxBuilder builder,
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		builder.OutboxMessageTypeRegistry.RegisterOutboxMessageType(
			scopeContext,
			code,
			name,
			@namespace);

		return builder;
	}

	public static ADFMessagingOutboxBuilder AddOutboxQueue<E>(
		this ADFMessagingOutboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Outbox.Model.OutboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO = false,
		int messagesBatchCount = 10,
		int? maxDegreeOfParallelism = 0,
		int maxMessageProcessingRetryCount = 5,
		string? messageTypeNamespace = null)
		where E : OutboxMessageReceivedEvent
		=> AddOutboxQueue(
			builder,
			scopeContext,
			queueName,
			factory,
			timeoutForMessageProcessing,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			maxMessageProcessingRetryCount,
			properties: null,
			idProcessingMode: Outbox.Model.OutboxQueueProcessingMode.Archivate,
			idSuspendingMode: Outbox.Model.OutboxQueueProcessingMode.NoAction,
			messageTypeNamespace: messageTypeNamespace);

	public static ADFMessagingOutboxBuilder AddOutboxQueue<E>(
		this ADFMessagingOutboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Outbox.Model.OutboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		string? messageTypeNamespace)
		where E : OutboxMessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		Guid? idMessageType = string.IsNullOrWhiteSpace(messageTypeNamespace)
			? null
			: builder.OutboxMessageTypeRegistry.GetIdOutboxMessageType(messageTypeNamespace, false);

		if (!string.IsNullOrWhiteSpace(messageTypeNamespace) && !idMessageType.HasValue)
			Throw.InvalidOperationException($"{nameof(Outbox.Model.OutboxMessageType)} with {nameof(messageTypeNamespace)} = {messageTypeNamespace} was not found", scopeContext);

		builder.OutboxQueueRegistry.RegisterOutboxQueue(
			scopeContext,
			queueName,
			factory,
			timeoutForMessageProcessing,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			maxMessageProcessingRetryCount,
			properties,
			idProcessingMode,
			idSuspendingMode,
			idMessageType);

		return builder;
	}

	public static ADFMessagingOutboxBuilder AddOutboxQueue<E, M>(
		this ADFMessagingOutboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Outbox.Model.OutboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO = false,
		int messagesBatchCount = 10,
		int? maxDegreeOfParallelism = 0,
		int maxMessageProcessingRetryCount = 5)
		where E : OutboxMessageReceivedEvent
		=> AddOutboxQueue<E, M>(
			builder,
			scopeContext,
			queueName,
			factory,
			timeoutForMessageProcessing,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			maxMessageProcessingRetryCount,
			properties: null,
			idProcessingMode: Outbox.Model.OutboxQueueProcessingMode.Archivate,
			idSuspendingMode: Outbox.Model.OutboxQueueProcessingMode.NoAction);

	public static ADFMessagingOutboxBuilder AddOutboxQueue<E, M>(
		this ADFMessagingOutboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Outbox.Model.OutboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode)
		where E : OutboxMessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		var messageTypeNamespace = typeof(M).GetSimplifiedAssemblyQualifiedName();
		Guid? idMessageType = builder.OutboxMessageTypeRegistry.GetIdOutboxMessageType(messageTypeNamespace, false);

		if (!idMessageType.HasValue)
			Throw.InvalidOperationException($"{nameof(Outbox.Model.OutboxMessageType)} with {nameof(messageTypeNamespace)} = {messageTypeNamespace} was not found", scopeContext);

		builder.OutboxQueueRegistry.RegisterOutboxQueue(
			scopeContext,
			queueName,
			factory,
			timeoutForMessageProcessing,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			maxMessageProcessingRetryCount,
			properties,
			idProcessingMode,
			idSuspendingMode,
			idMessageType);

		return builder;
	}
}
