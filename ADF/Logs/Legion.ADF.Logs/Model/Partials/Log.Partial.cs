using Legion.Logging;
using Legion.Serializer;

namespace Legion.ADF.Logs.Model;

public sealed partial class Log : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<Log> CreateLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		string? sourceContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Log>();

		if (result.IsArgumentNull(scopeContext, logMessage))
			return result.Build();

		var log = new Log
		{
			__IsNewObject = true,
			IdLog = logMessage.IdLogMessage,
			CreatedUtc = logMessage.CreatedUtc.UtcDateTime,
			InternalMessage = logMessage.InternalMessage,
			ClientMessage = logMessage.ClientMessage,
			Detail = logMessage.Detail,
			StackTrace = logMessage.StackTrace,
			Component = logMessage.ScopeContext.Component,
			OperationName = logMessage.OperationName,
			AggregateName = logMessage.AggregateName,
			AggregateIdentifier = logMessage.AggregateIdentifier,
			CustomCorrelationId = logMessage.ScopeContext.CustomCorrelationId,
			IdApplicationEntry = logMessage.ScopeContext.IdApplicationEntry,
			CorrelationId = logMessage.ScopeContext.CorrelationId,
			ExternalCorrelationId = logMessage.ScopeContext.ExternalCorrelationId,
			ContextProperties = JsonSerializerHelper.Serialize(logMessage.ScopeContext.ContextProperties),
			IdUser = logMessage.ScopeContext.IdUser,
			TenantIdentifier = logMessage.ScopeContext.TenantIdentifier,
			IdLogLevel = logMessage.IdLogLevel,
			LogCode = logMessage.ErrorCode?.Code,
			SourceSystemName = logMessage.ScopeContext.SourceSystemName,
			TraceCorrelationId = logMessage.ScopeContext.TraceCorrelationId,
			TraceFrame = logMessage.ScopeContext.TraceFrameStack.ToString(),
			SourceContext = string.IsNullOrWhiteSpace(logMessage.SourceContext) ? sourceContext : logMessage.SourceContext,
			RuntimeUniqueKey = logMessage.ScopeContext.RuntimeUniqueKey,
			IsValidationError = logMessage.IsValidationError,
			PropertyName = logMessage.PropertyName,
			DisplayPropertyName = logMessage.DisplayPropertyName,
			ValidationFailure = logMessage.ValidationFailure?.ToString()
		};

		var validationResult =
			DefaultDBValidator
				.Validate(log);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(log).Build();
	}
}
