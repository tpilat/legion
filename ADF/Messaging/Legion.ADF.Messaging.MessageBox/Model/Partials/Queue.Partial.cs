namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Queue : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<Queue?> Create(
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
		Guid idSuspendingMode,
		Guid? idJob,
		Guid? idOrchestration)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Queue?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, receivedEventNamespace))

			if (result.IsArgumentLessThanOrEqual(scopeContext, messagesBatchCount, 0))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, timeoutForMessageProcessing, TimeSpan.Zero))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, maxMessageProcessingRetryCount, 0))
			return result.Build();

		if (isSequentialFIFO && (maxDegreeOfParallelism == 0 || 1 < maxDegreeOfParallelism))
			return result.WithArgumentException(scopeContext, maxDegreeOfParallelism, null, $"{nameof(maxDegreeOfParallelism)} must be NULL or 1 if {nameof(isSequentialFIFO)} is true.");

		var id = Guid.NewGuid();
		var queue = new Queue
		{
			__IsNewObject = true,
			IdQueue = id,
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
			IdJob = idJob,
			IdOrchestration = idOrchestration,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(queue);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(queue).Build();
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
