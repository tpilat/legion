namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageProcessingLog : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<MessageProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid idMessage,
		Guid? idQueuedMessage,
		Guid? idSubscribedMessage,
		Guid idMessageProcessingStatus,
		Guid? idLogMessage,
		string code,
		string? detail)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(idQueuedMessage), idQueuedMessage?.ToString())
			.AddContextProperty(nameof(idSubscribedMessage), idSubscribedMessage?.ToString())
			.AddContextProperty(nameof(idMessageProcessingStatus), idMessageProcessingStatus.ToString())
			.AddContextProperty(nameof(code), code);

		var result = new ResultBuilder<MessageProcessingLog?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		if (!idQueuedMessage.HasValue && !idSubscribedMessage.HasValue && result.IsArgumentNull(scopeContext, idQueuedMessage, errorCode: null, detail: null, paramName: $"{nameof(idQueuedMessage)} && {nameof(idSubscribedMessage)}"))
			return result.Build();

		if (idQueuedMessage.HasValue && idSubscribedMessage.HasValue)
			return result.WithArgumentException(scopeContext, idQueuedMessage, errorCode: null, detail: "Both are set", paramName: $"{nameof(idQueuedMessage)} != null && {nameof(idSubscribedMessage)} != null");

		var id = GlobalContext.Instance.NewGuid();
		var messageProcessingLog = new MessageProcessingLog
		{
			__IsNewObject = true,
			IdMessageProcessingLog = id,
			IdMessage = idMessage,
			IdQueuedMessage = idQueuedMessage,
			IdSubscribedMessage = idSubscribedMessage,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdMessageProcessingStatus = idMessageProcessingStatus,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = idLogMessage,
			Code = code,
			Detail = detail,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(messageProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageProcessingLog).Build();
	}

	internal static IResult<MessageProcessingLog?> Create(
		IScopeContext scopeContext,
		QueuedMessage queuedMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog?>();

		if (result.IsArgumentNull(scopeContext, queuedMessage))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageProcessingLog = new MessageProcessingLog
		{
			__IsNewObject = true,
			IdMessageProcessingLog = id,
			IdMessage = queuedMessage.IdMessage,
			IdQueuedMessage = queuedMessage.IdQueuedMessage,
			IdSubscribedMessage = null,
			CreatedUtc = queuedMessage.AssignedUtc,
			IdMessageProcessingStatus = MessageStatus.Created,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = null,
			Code = nameof(MessageStatus.Created),
			Detail = null,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(messageProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageProcessingLog).Build();
	}

	internal static IResult<MessageProcessingLog?> Create(
		IScopeContext scopeContext,
		SubscribedMessage subscribedMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog?>();

		if (result.IsArgumentNull(scopeContext, subscribedMessage))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageProcessingLog = new MessageProcessingLog
		{
			__IsNewObject = true,
			IdMessageProcessingLog = id,
			IdMessage = subscribedMessage.IdMessage,
			IdQueuedMessage = null,
			IdSubscribedMessage = subscribedMessage.IdSubscribedMessage,
			CreatedUtc = subscribedMessage.AssignedUtc,
			IdMessageProcessingStatus = MessageStatus.Created,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = null,
			Code = nameof(MessageStatus.Created),
			Detail = null,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(messageProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageProcessingLog).Build();
	}
}
