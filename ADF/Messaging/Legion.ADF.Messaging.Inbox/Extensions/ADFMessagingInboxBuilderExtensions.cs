using Legion.ADF.Messaging.Inbox;
using Legion.ADF.Messaging.Inbox.Events;
using Legion.Extensions;

namespace Legion.ADF.Messaging;

public static class ADFMessagingInboxBuilderExtensions
{
	public static ADFMessagingInboxBuilder AddInboxMessageType<T>(
		this ADFMessagingInboxBuilder builder,
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		builder.InboxMessageTypeRegistry.RegisterInboxMessageType<T>(scopeContext);

		return builder;
	}

	public static ADFMessagingInboxBuilder AddInboxMessageType(
		this ADFMessagingInboxBuilder builder,
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		builder.InboxMessageTypeRegistry.RegisterInboxMessageType(
			scopeContext,
			code,
			name,
			@namespace);

		return builder;
	}

	public static ADFMessagingInboxBuilder AddInboxQueue<E>(
		this ADFMessagingInboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Inbox.Model.InboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO = false,
		int messagesBatchCount = 10,
		int? maxDegreeOfParallelism = 0,
		int maxMessageProcessingRetryCount = 5,
		string? messageTypeNamespace = null)
		where E : InboxMessageReceivedEvent
		=> AddInboxQueue(
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
			idProcessingMode: Inbox.Model.InboxQueueProcessingMode.Archivate,
			idSuspendingMode: Inbox.Model.InboxQueueProcessingMode.NoAction,
			messageTypeNamespace: messageTypeNamespace);

	public static ADFMessagingInboxBuilder AddInboxQueue<E>(
		this ADFMessagingInboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Inbox.Model.InboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		string? messageTypeNamespace)
		where E : InboxMessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		Guid? idMessageType = string.IsNullOrWhiteSpace(messageTypeNamespace)
			? null
			: builder.InboxMessageTypeRegistry.GetIdInboxMessageType(messageTypeNamespace, false);

		if (!string.IsNullOrWhiteSpace(messageTypeNamespace) && !idMessageType.HasValue)
			Throw.InvalidOperationException($"{nameof(Inbox.Model.InboxMessageType)} with {nameof(messageTypeNamespace)} = {messageTypeNamespace} was not found", scopeContext);

		builder.InboxQueueRegistry.RegisterInboxQueue(
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

	public static ADFMessagingInboxBuilder AddInboxQueue<E, M>(
		this ADFMessagingInboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Inbox.Model.InboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO = false,
		int messagesBatchCount = 10,
		int? maxDegreeOfParallelism = 0,
		int maxMessageProcessingRetryCount = 5)
		where E : InboxMessageReceivedEvent
		=> AddInboxQueue<E, M>(
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
			idProcessingMode: Inbox.Model.InboxQueueProcessingMode.Archivate,
			idSuspendingMode: Inbox.Model.InboxQueueProcessingMode.NoAction);

	public static ADFMessagingInboxBuilder AddInboxQueue<E, M>(
		this ADFMessagingInboxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<Inbox.Model.InboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode)
		where E : InboxMessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		var messageTypeNamespace = typeof(M).GetSimplifiedAssemblyQualifiedName();
		Guid? idMessageType = builder.InboxMessageTypeRegistry.GetIdInboxMessageType(messageTypeNamespace, false);

		if (!idMessageType.HasValue)
			Throw.InvalidOperationException($"{nameof(Inbox.Model.InboxMessageType)} with {nameof(messageTypeNamespace)} = {messageTypeNamespace} was not found", scopeContext);

		builder.InboxQueueRegistry.RegisterInboxQueue(
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
