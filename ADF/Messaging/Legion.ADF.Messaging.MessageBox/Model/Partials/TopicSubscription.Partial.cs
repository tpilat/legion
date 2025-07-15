namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class TopicSubscription : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<TopicSubscription?> Create(
		IScopeContext scopeContext,
		Topic topic,
		DTOs.TopicSubscriptionDto subscription)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<TopicSubscription?>();

		if (result.IsArgumentNull(scopeContext, topic))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, subscription))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, subscription.SubscriptionName))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, subscription.ReceivedEventNamespace))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, subscription.MessagesBatchCount, 0))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, subscription.TimeoutForMessageProcessing, TimeSpan.Zero))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, subscription.MaxMessageProcessingRetryCount, 0))
			return result.Build();

		if (subscription.IsSequentialFIFO && (subscription.MaxDegreeOfParallelism == 0 || 1 < subscription.MaxDegreeOfParallelism))
			return result.WithArgumentException(scopeContext, subscription.MaxDegreeOfParallelism, null, $"{nameof(subscription.MaxDegreeOfParallelism)} must be NULL or 1 if {nameof(subscription.IsSequentialFIFO)} is true.");

		var id = GlobalContext.Instance.NewGuid();
		var topicSubscription = new TopicSubscription
		{
			__IsNewObject = true,
			IdTopicSubscription = id,
			IdTopic = topic.IdTopic,
			Topic = topic,
			SubscriptionName = subscription.SubscriptionName,
			ReceivedEventNamespace = subscription.ReceivedEventNamespace,
			IsActive = true,
			IsSequentialFIFO = subscription.IsSequentialFIFO,
			MessagesBatchCount = subscription.MessagesBatchCount,
			MaxDegreeOfParallelism = subscription.MaxDegreeOfParallelism,
			TimeoutForMessageProcessing = subscription.TimeoutForMessageProcessing,
			MaxMessageProcessingRetryCount = subscription.MaxMessageProcessingRetryCount,
			Properties = subscription.Properties,
			IdProcessingMode = subscription.IdProcessingMode,
			IdSuspendingMode = subscription.IdSuspendingMode,
			IdJob = subscription.IdJob,
			IdOrchestration = subscription.IdOrchestration,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(topicSubscription);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(topicSubscription).Build();
	}

	internal IResult Update(
		IScopeContext scopeContext,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		TimeSpan timeoutForMessageProcessing,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentLessThanOrEqual(scopeContext, messagesBatchCount, 0))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, timeoutForMessageProcessing, TimeSpan.Zero))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, maxMessageProcessingRetryCount, 0))
			return result.Build();

		if (isSequentialFIFO && (maxDegreeOfParallelism == 0 || 1 < maxDegreeOfParallelism))
			return result.WithArgumentException(scopeContext, maxDegreeOfParallelism, null, $"{nameof(maxDegreeOfParallelism)} must be NULL or 1 if {nameof(isSequentialFIFO)} is true.");


		IsSequentialFIFO = isSequentialFIFO;
		MessagesBatchCount = messagesBatchCount;
		MaxDegreeOfParallelism = maxDegreeOfParallelism;
		TimeoutForMessageProcessing = timeoutForMessageProcessing;
		MaxMessageProcessingRetryCount = maxMessageProcessingRetryCount;
		Properties = properties;
		IdProcessingMode = idProcessingMode;
		IdSuspendingMode = idSuspendingMode;
		IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		result.MergeHasError(scopeContext, validationResult, true);
		return result.Build();
	}
}
