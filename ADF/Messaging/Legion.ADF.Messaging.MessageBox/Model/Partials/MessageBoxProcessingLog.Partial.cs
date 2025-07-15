using Legion.Logging;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxProcessingLog : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<MessageBoxProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid? idQueue,
		Guid? idTopic,
		Guid? idTopicSubscription,
		ILogMessage logMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageBoxProcessingLog?>();

		if (result.IsArgumentNull(scopeContext, logMessage))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, logMessage.Detail))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageBoxProcessingLog = new MessageBoxProcessingLog
		{
			__IsNewObject = true,
			IdMessageBoxProcessingLog = id,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			IdQueue = idQueue,
			IdTopic = idTopic,
			IdTopicSubscription = idTopicSubscription,
			CreatedUtc = logMessage.CreatedUtc.UtcDateTime,
			IdLogLevel = logMessage.IdLogLevel,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = logMessage.IdLogMessage,
			Code = logMessage.Detail,
			Detail = logMessage.InternalMessage,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(messageBoxProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageBoxProcessingLog).Build();
	}

	internal static IResult<MessageBoxProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid? idQueue,
		Guid? idTopic,
		Guid? idTopicSubscription,
		int idLogLevel,
		Guid? idLogMessage,
		string code,
		string? detail)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageBoxProcessingLog?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageBoxProcessingLog = new MessageBoxProcessingLog
		{
			__IsNewObject = true,
			IdMessageBoxProcessingLog = id,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			IdQueue = idQueue,
			IdTopic = idTopic,
			IdTopicSubscription = idTopicSubscription,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdLogLevel = idLogLevel,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = idLogMessage,
			Code = code,
			Detail = detail,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(messageBoxProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageBoxProcessingLog).Build();
	}
}
