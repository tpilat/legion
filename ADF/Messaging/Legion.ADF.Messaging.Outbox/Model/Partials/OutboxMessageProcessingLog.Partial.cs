namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageProcessingLog : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OutboxMessageProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Guid idOutboxQueue,
		Guid idOutboxMessageStatus,
		Guid? idLogMessage,
		string code,
		string? detail)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageProcessingLog = new OutboxMessageProcessingLog
		{
			__IsNewObject = true,
			IdOutboxMessageProcessingLog = id,
			IdOutboxMessage = idOutboxMessage,
			IdOutboxQueue = idOutboxQueue,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdOutboxMessageStatus = idOutboxMessageStatus,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = idLogMessage,
			Code = code,
			Detail = detail,
			IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageProcessingLog).Build();
	}

	internal static IResult<OutboxMessageProcessingLog?> Create(
		IScopeContext scopeContext,
		OutboxMessage outboxMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog?>();

		if (result.IsArgumentNull(scopeContext, outboxMessage))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageProcessingLog = new OutboxMessageProcessingLog
		{
			__IsNewObject = true,
			IdOutboxMessageProcessingLog = id,
			IdOutboxMessage = outboxMessage.IdOutboxMessage,
			IdOutboxQueue = outboxMessage.IdOutboxQueue,
			CreatedUtc = outboxMessage.CreatedUtc,
			IdOutboxMessageStatus = OutboxMessageStatus.Created,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = null,
			Code = nameof(OutboxMessageStatus.Created),
			Detail = null,
			IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageProcessingLog).Build();
	}
}
