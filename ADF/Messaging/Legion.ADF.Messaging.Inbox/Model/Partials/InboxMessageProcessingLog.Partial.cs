namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageProcessingLog : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxMessageProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Guid idInboxQueue,
		Guid idInboxMessageStatus,
		Guid? idLogMessage,
		string code,
		string? detail)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageProcessingLog = new InboxMessageProcessingLog
		{
			__IsNewObject = true,
			IdInboxMessageProcessingLog = id,
			IdInboxMessage = idInboxMessage,
			IdInboxQueue = idInboxQueue,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdInboxMessageStatus = idInboxMessageStatus,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = idLogMessage,
			Code = code,
			Detail = detail,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(inboxMessageProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageProcessingLog).Build();
	}

	internal static IResult<InboxMessageProcessingLog?> Create(
		IScopeContext scopeContext,
		InboxMessage inboxMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog?>();

		if (result.IsArgumentNull(scopeContext, inboxMessage))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageProcessingLog = new InboxMessageProcessingLog
		{
			__IsNewObject = true,
			IdInboxMessageProcessingLog = id,
			IdInboxMessage = inboxMessage.IdInboxMessage,
			IdInboxQueue = inboxMessage.IdInboxQueue,
			CreatedUtc = inboxMessage.CreatedUtc,
			IdInboxMessageStatus = InboxMessageStatus.Created,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = null,
			Code = nameof(InboxMessageStatus.Created),
			Detail = null,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(inboxMessageProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageProcessingLog).Build();
	}
}
