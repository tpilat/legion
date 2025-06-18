using Legion.Logging;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxProcessingLog : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid? idInboxQueue,
		ILogMessage logMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxProcessingLog?>();

		if (result.IsArgumentNull(scopeContext, logMessage))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, logMessage.Detail))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxProcessingLog = new InboxProcessingLog
		{
			__IsNewObject = true,
			IdInboxProcessingLog = id,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			IdInboxQueue = idInboxQueue,
			CreatedUtc = logMessage.CreatedUtc.UtcDateTime,
			IdLogLevel = logMessage.IdLogLevel,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = logMessage.IdLogMessage,
			Code = logMessage.Detail,
			Detail = logMessage.InternalMessage,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(inboxProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxProcessingLog).Build();
	}

	internal static IResult<InboxProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid? idInboxQueue,
		int idLogLevel,
		Guid? idLogMessage,
		string code,
		string? detail)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxProcessingLog?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxProcessingLog = new InboxProcessingLog
		{
			__IsNewObject = true,
			IdInboxProcessingLog = id,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			IdInboxQueue = idInboxQueue,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdLogLevel = idLogLevel,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = idLogMessage,
			Code = code,
			Detail = detail,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(inboxProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxProcessingLog).Build();
	}
}
