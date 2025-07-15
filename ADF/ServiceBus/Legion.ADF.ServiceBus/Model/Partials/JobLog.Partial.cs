using Legion.Extensions;
using Legion.Logging;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobLog : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	internal static IResult<JobLog> Create(
		IScopeContext scopeContext,
		Job job,
		string code,
		ILogMessage logMessage,
		Guid idJobStatus,
		Guid? idMessageProcessingLog = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<JobLog>();

		if (result.IsArgumentNull(scopeContext, job))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, logMessage))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var jobLog = new JobLog
		{
			__IsNewObject = true,
			IdJobLog = id,
			Job = job,
			IdLogLevel = logMessage.IdLogLevel,
			CreatedUtc = logMessage.CreatedUtc.UtcDateTime,
			IdJobStatus = idJobStatus,
			TraceCorrelationId = logMessage.ScopeContext.TraceCorrelationId,
			IdLogMessage = logMessage.IdLogMessage,
			Code = code,
			Detail = logMessage.ToMessageText(),
			IdMessageProcessingLog = idMessageProcessingLog
		};

		var validationResult =
			DefaultDBValidator
				.Validate(jobLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(jobLog).Build();
	}

	internal DTOs.Jobs.JobLogDto ToDto()
	{
		var dto = new DTOs.Jobs.JobLogDto
		{
			IdJob = IdJob,
			IdLogLevel = IdLogLevel,
			CreatedUtc = CreatedUtc,
			IdJobStatus = IdJobStatus,
			TraceCorrelationId = TraceCorrelationId,
			IdLogMessage = IdLogMessage,
			Code = Code,
			Detail = Detail,
			IdMessageProcessingLog = IdMessageProcessingLog,
			JobStatus = JobStatus.FromId(IdJobStatus)!.Code!.ToCammelCase(removeUnderscores: false, throwIfEmpty: false)!
		};

		return dto;
	}
}
