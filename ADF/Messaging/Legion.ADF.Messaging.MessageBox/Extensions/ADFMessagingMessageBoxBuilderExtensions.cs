using Legion.ADF.Messaging.MessageBox;
using Legion.ADF.Messaging.MessageBox.Events;
using Legion.Extensions;

namespace Legion.ADF.Messaging;

public static class ADFMessagingMessageBoxBuilderExtensions
{
	public static ADFMessagingMessageBoxBuilder AddMessageType<T>(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		builder.MessageTypeRegistry.RegisterMessageType<T>(scopeContext);

		return builder;
	}

	public static ADFMessagingMessageBoxBuilder AddMessageType(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		builder.MessageTypeRegistry.RegisterMessageType(
			scopeContext,
			code,
			name,
			@namespace);

		return builder;
	}

	public static ADFMessagingMessageBoxBuilder AddQueue<E>(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<MessageBox.Model.Message, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO = false,
		int messagesBatchCount = 10,
		int? maxDegreeOfParallelism = 0,
		int maxMessageProcessingRetryCount = 5,
		string? messageTypeNamespace = null)
		where E : MessageReceivedEvent
		=> AddQueue(
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
			idProcessingMode: MessageBox.Model.QueueProcessingMode.Archivate,
			idSuspendingMode: MessageBox.Model.QueueProcessingMode.NoAction,
			messageTypeNamespace: messageTypeNamespace,
			idJob: null,
			idOrchestration: null);

	public static ADFMessagingMessageBoxBuilder AddQueue<E>(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<MessageBox.Model.Message, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		string? messageTypeNamespace,
		Guid? idJob,
		Guid? idOrchestration)
		where E : MessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		Guid? idMessageType = string.IsNullOrWhiteSpace(messageTypeNamespace)
			? null
			: builder.MessageTypeRegistry.GetIdMessageType(messageTypeNamespace, false);

		if (!string.IsNullOrWhiteSpace(messageTypeNamespace) && !idMessageType.HasValue)
			Throw.InvalidOperationException($"{nameof(MessageBox.Model.MessageType)} with {nameof(messageTypeNamespace)} = {messageTypeNamespace} was not found", scopeContext);

		builder.QueueRegistry.RegisterQueue(
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
			idMessageType,
			idJob,
			idOrchestration);

		return builder;
	}

	public static ADFMessagingMessageBoxBuilder AddQueue<E, M>(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<MessageBox.Model.Message, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO = false,
		int messagesBatchCount = 10,
		int? maxDegreeOfParallelism = 0,
		int maxMessageProcessingRetryCount = 5)
		where E : MessageReceivedEvent
		=> AddQueue<E, M>(
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
			idProcessingMode: MessageBox.Model.QueueProcessingMode.Archivate,
			idSuspendingMode: MessageBox.Model.QueueProcessingMode.NoAction,
			idJob : null,
			idOrchestration: null);

	public static ADFMessagingMessageBoxBuilder AddQueue<E, M>(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext,
		string queueName,
		Func<MessageBox.Model.Message, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		Guid? idJob,
		Guid? idOrchestration)
		where E : MessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);

		var messageTypeNamespace = typeof(M).GetSimplifiedAssemblyQualifiedName();
		Guid? idMessageType = builder.MessageTypeRegistry.GetIdMessageType(messageTypeNamespace, false);

		if (!idMessageType.HasValue)
			Throw.InvalidOperationException($"{nameof(MessageBox.Model.MessageType)} with {nameof(messageTypeNamespace)} = {messageTypeNamespace} was not found", scopeContext);

		builder.QueueRegistry.RegisterQueue(
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
			idMessageType,
			idJob,
			idOrchestration);

		return builder;
	}

	public static ADFMessagingMessageBoxBuilder AddTopic(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext,
		string topicName,
		TimeSpan timeoutForMessageProcessing,
		Action<ADFMessagingMessageBoxTopicSubscriptionBuilder> configureSubscriptions,
		bool isSequentialFIFO = false,
		int messagesBatchCount = 10,
		int? maxDegreeOfParallelism = 0,
		int maxMessageProcessingRetryCount = 5)
		=> AddTopic(
			builder,
			scopeContext,
			topicName,
			timeoutForMessageProcessing,
			configureSubscriptions,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			maxMessageProcessingRetryCount,
			properties: null,
			idProcessingMode: MessageBox.Model.QueueProcessingMode.Archivate,
			idSuspendingMode: MessageBox.Model.QueueProcessingMode.NoAction);

	public static ADFMessagingMessageBoxBuilder AddTopic(
		this ADFMessagingMessageBoxBuilder builder,
		IScopeContext scopeContext,
		string topicName,
		TimeSpan timeoutForMessageProcessing,
		Action<ADFMessagingMessageBoxTopicSubscriptionBuilder> configureSubscriptions,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(builder);
		Throw.IfArgumentNull(configureSubscriptions);

		var topic = builder.TopicRegistry.RegisterTopic(
			scopeContext,
			topicName,
			timeoutForMessageProcessing,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			maxMessageProcessingRetryCount,
			properties,
			idProcessingMode,
			idSuspendingMode);

		scopeContext.AddContextProperty(nameof(topic), topic.Name);

		var subscriptionBuilder = new ADFMessagingMessageBoxTopicSubscriptionBuilder(scopeContext, builder.TopicRegistry, topic);
		configureSubscriptions.Invoke(subscriptionBuilder);

		var topicSubscriptions = builder.TopicRegistry.GetAllTopicSubscriptions(topic.Name);

		Throw.IfNullOrEmpty(topicSubscriptions, scopeContext);

		return builder;
	}
}
