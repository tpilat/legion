namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxQueue : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxQueue?> Create(
		IScopeContext scopeContext,
		string name,
		string receivedEventNamespace,
		Guid? idMessageType,
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

		var result = new ResultBuilder<InboxQueue?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, receivedEventNamespace))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, messagesBatchCount, 0))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, timeoutForMessageProcessing, TimeSpan.Zero))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, maxMessageProcessingRetryCount, 0))
			return result.Build();

		if (isSequentialFIFO && (maxDegreeOfParallelism == 0 || 1 < maxDegreeOfParallelism))
			return result.WithArgumentException(scopeContext, maxDegreeOfParallelism, null, $"{nameof(maxDegreeOfParallelism)} must be NULL or 1 if {nameof(isSequentialFIFO)} is true.");

		var id = GlobalContext.Instance.NewGuid();
		var inboxQueue = new InboxQueue
		{
			__IsNewObject = true,
			IdInboxQueue = id,
			Name = name,
			ReceivedEventNamespace = receivedEventNamespace,
			IdMessageType = idMessageType,
			IsActive = true,
			IsSequentialFIFO = isSequentialFIFO,
			MessagesBatchCount = messagesBatchCount,
			MaxDegreeOfParallelism = maxDegreeOfParallelism,
			TimeoutForMessageProcessing = timeoutForMessageProcessing,
			MaxMessageProcessingRetryCount = maxMessageProcessingRetryCount,
			Properties = properties,
			IdProcessingMode = idProcessingMode,
			IdSuspendingMode = idSuspendingMode,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(inboxQueue);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxQueue).Build();
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
		IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		result.MergeHasError(scopeContext, validationResult, true);
		return result.Build();
	}
}
