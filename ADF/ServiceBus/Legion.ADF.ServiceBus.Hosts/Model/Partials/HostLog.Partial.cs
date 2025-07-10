using Legion.Logging;

namespace Legion.ADF.ServiceBus.Hosts.Model;

public sealed partial class HostLog : Hosts.HostsBaseEntity, Legion.Model.IEntity
{
	public static IResult<HostLog> Create(
		IScopeContext scopeContext,
		Guid idHost,
		string code,
		ILogMessage logMessage,
		bool isRunning)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<HostLog>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, logMessage))
			return result.Build();

		var id = Guid.NewGuid();
		var hostLog = new HostLog
		{
			__IsNewObject = true,
			IdHostLog = id,
			IdHost = idHost,
			IdLogLevel = logMessage.IdLogLevel,
			CreatedUtc = logMessage.CreatedUtc.UtcDateTime,
			IsRunning = isRunning,
			TraceCorrelationId = logMessage.ScopeContext.TraceCorrelationId,
			IdLogMessage = logMessage.IdLogMessage,
			Code = code,
			Detail = logMessage.ToMessageText(),
		};

		var validationResult =
			DefaultDBValidator
				.Validate(hostLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(hostLog).Build();
	}
}
