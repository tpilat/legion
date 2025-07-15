using Legion.Logging;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxProcessingLog : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OutboxProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid? idOutboxQueue,
		ILogMessage logMessage)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxProcessingLog?>();

		if (result.IsArgumentNull(scopeContext, logMessage))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, logMessage.Detail))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var outboxProcessingLog = new OutboxProcessingLog
		{
			__IsNewObject = true,
			IdOutboxProcessingLog = id,
			IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			IdOutboxQueue = idOutboxQueue,
			CreatedUtc = logMessage.CreatedUtc.UtcDateTime,
			IdLogLevel = logMessage.IdLogLevel,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = logMessage.IdLogMessage,
			Code = logMessage.Detail,
			Detail = logMessage.InternalMessage,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxProcessingLog).Build();
	}

	internal static IResult<OutboxProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid? idOutboxQueue,
		int idLogLevel,
		Guid? idLogMessage,
		string code,
		string? detail)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxProcessingLog?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var outboxProcessingLog = new OutboxProcessingLog
		{
			__IsNewObject = true,
			IdOutboxProcessingLog = id,
			IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			IdOutboxQueue = idOutboxQueue,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdLogLevel = idLogLevel,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = idLogMessage,
			Code = code,
			Detail = detail,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxProcessingLog).Build();
	}
}
