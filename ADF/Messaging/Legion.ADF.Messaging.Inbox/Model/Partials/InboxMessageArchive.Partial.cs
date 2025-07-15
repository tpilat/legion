namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageArchive : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxMessageArchive?> Create(
		IScopeContext scopeContext,
		InboxMessage inboxMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageArchive?>();

		if (result.IsArgumentNull(scopeContext, inboxMessage))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var inboxMessageArchive = new InboxMessageArchive
		{
			__IsNewObject = true,
			IdInboxMessage = inboxMessage.IdInboxMessage,
			IdMessageType = inboxMessage.IdMessageType,
			IdInboxMessageStatus = inboxMessage.IdInboxMessageStatus,
			IdMessageContent = inboxMessage.IdMessageContent,
			IdInboxQueue = inboxMessage.IdInboxQueue,
			MessageId = inboxMessage.MessageId,
			BusinessId = inboxMessage.BusinessId,
			CorrelationId = inboxMessage.CorrelationId,
			SessionId = inboxMessage.SessionId,
			SessionMessagePartId = inboxMessage.SessionMessagePartId,
			TraceCorrelationId = inboxMessage.TraceCorrelationId,
			Properties = inboxMessage.Properties,
			Publisher = inboxMessage.Publisher,
			PublisherId = inboxMessage.PublisherId,
			CreatedUtc = inboxMessage.CreatedUtc,
			ProcessedUtc = inboxMessage.ProcessedUtc,
			SuspendedUtc = inboxMessage.SuspendedUtc,
			LastProcessingUtc = inboxMessage.LastProcessingUtc,
			LastProcessingTimeoutUtc = inboxMessage.LastProcessingTimeoutUtc,
			NextProcessingUtc = inboxMessage.NextProcessingUtc,
			RetryCount = inboxMessage.RetryCount,
			TargetTopic = inboxMessage.TargetTopic,
			TargetQueueName = inboxMessage.TargetQueueName,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(inboxMessageArchive);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageArchive).Build();
	}
}
