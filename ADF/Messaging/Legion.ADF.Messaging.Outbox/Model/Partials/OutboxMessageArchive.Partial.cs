namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageArchive : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OutboxMessageArchive?> Create(
		IScopeContext scopeContext,
		OutboxMessage outboxMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageArchive?>();

		if (result.IsArgumentNull(scopeContext, outboxMessage))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageArchive = new OutboxMessageArchive
		{
			__IsNewObject = true,
			IdOutboxMessage = outboxMessage.IdOutboxMessage,
			IdMessageType = outboxMessage.IdMessageType,
			IdOutboxMessageStatus = outboxMessage.IdOutboxMessageStatus,
			IdMessageContent = outboxMessage.IdMessageContent,
			IdOutboxQueue = outboxMessage.IdOutboxQueue,
			MessageId = outboxMessage.MessageId,
			BusinessId = outboxMessage.BusinessId,
			CorrelationId = outboxMessage.CorrelationId,
			SessionId = outboxMessage.SessionId,
			SessionMessagePartId = outboxMessage.SessionMessagePartId,
			TraceCorrelationId = outboxMessage.TraceCorrelationId,
			Properties = outboxMessage.Properties,
			Publisher = outboxMessage.Publisher,
			PublisherId = outboxMessage.PublisherId,
			CreatedUtc = outboxMessage.CreatedUtc,
			ProcessedUtc = outboxMessage.ProcessedUtc,
			SuspendedUtc = outboxMessage.SuspendedUtc,
			LastProcessingUtc = outboxMessage.LastProcessingUtc,
			LastProcessingTimeoutUtc = outboxMessage.LastProcessingTimeoutUtc,
			NextProcessingUtc = outboxMessage.NextProcessingUtc,
			RetryCount = outboxMessage.RetryCount,
			TargetTopic = outboxMessage.TargetTopic,
			TargetQueueName = outboxMessage.TargetQueueName,
			IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageArchive);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageArchive).Build();
	}
}
