using Legion.Logging;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class HostLog : ServiceBusBaseEntity, Legion.Model.IEntity
{
	internal static IResult<HostLog> Create(
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

		var id = GlobalContext.Instance.NewGuid();
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

	internal DTOs.Hosts.HostLogDto ToDto()
	{
		var dto = new DTOs.Hosts.HostLogDto
		{
			IdHost = IdHost,
			IdLogLevel = IdLogLevel,
			CreatedUtc = CreatedUtc,
			IsRunning = IsRunning,
			TraceCorrelationId = TraceCorrelationId,
			IdLogMessage = IdLogMessage,
			Code = Code,
			Detail = Detail
		};

		return dto;
	}
}
