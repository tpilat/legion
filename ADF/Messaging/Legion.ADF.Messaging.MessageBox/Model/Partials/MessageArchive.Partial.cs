namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageArchive : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<MessageArchive?> Create(
		IScopeContext scopeContext,
		Message message)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageArchive?>();

		if (result.IsArgumentNull(scopeContext, message))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageArchive = new MessageArchive
		{
			__IsNewObject = true,
			IdMessage = message.IdMessage,
			IdMessageType = message.IdMessageType,
			IdMessageStatus = message.IdMessageStatus,
			IdMessageContent = message.IdMessageContent,
			IdQueue = message.IdQueue,
			IdTopic = message.IdTopic,
			MessageId = message.MessageId,
			BusinessId = message.BusinessId,
			CorrelationId = message.CorrelationId,
			SessionId = message.SessionId,
			SessionMessagePartId = message.SessionMessagePartId,
			TraceCorrelationId = message.TraceCorrelationId,
			Properties = message.Properties,
			Publisher = message.Publisher,
			PublisherId = message.PublisherId,
			CreatedUtc = message.CreatedUtc,
			ValidToUtc = message.ValidToUtc,
			Priority = message.Priority,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(messageArchive);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageArchive).Build();
	}
}
