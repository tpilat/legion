namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Topic : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<Topic?> Create(
		IScopeContext scopeContext,
		string name,
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

		var result = new ResultBuilder<Topic?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, messagesBatchCount, 0))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, timeoutForMessageProcessing, TimeSpan.Zero))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, maxMessageProcessingRetryCount, 0))
			return result.Build();

		if (isSequentialFIFO && (maxDegreeOfParallelism == 0 || 1 < maxDegreeOfParallelism))
			return result.WithArgumentException(scopeContext, maxDegreeOfParallelism, null, $"{nameof(maxDegreeOfParallelism)} must be NULL or 1 if {nameof(isSequentialFIFO)} is true.");

		var id = Guid.NewGuid();
		var topic = new Topic
		{
			__IsNewObject = true,
			IdTopic = id,
			Name = name,
			IsActive = true,
			IsSequentialFIFO = isSequentialFIFO,
			MessagesBatchCount = messagesBatchCount,
			MaxDegreeOfParallelism = maxDegreeOfParallelism,
			TimeoutForMessageProcessing = timeoutForMessageProcessing,
			MaxMessageProcessingRetryCount = maxMessageProcessingRetryCount,
			Properties = properties,
			IdProcessingMode = idProcessingMode,
			IdSuspendingMode = idSuspendingMode,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(topic);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(topic).Build();
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

	internal IResult AddSubscription(
		IScopeContext scopeContext,
		TopicSubscription subscription)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNull(scopeContext, subscription))
			return result.Build();

		_topicSubscriptions.Add(subscription);

		return result.Build();
	}
}
