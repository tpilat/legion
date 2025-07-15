using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobExecution : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	internal static IResult<JobExecution> Create(
		IScopeContext scopeContext,
		Job job,
		DateTime startUtc,
		DateTime statisticsStartHourUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<JobExecution>();

		if (result.IsArgumentNull(scopeContext, job))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var host = new JobExecution
		{
			__IsNewObject = true,
			IdJobExecution = id,
			Job = job,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			StartUtc = startUtc,
			EndUtc = null,
			IdJobStatus = JobStatus.Running,
			StatisticsStartHourUtc = statisticsStartHourUtc
		};

		var validationResult =
			DefaultDBValidator
				.Validate(host);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(host).Build();
	}

	internal DTOs.Jobs.JobExecutionDto ToDto()
	{
		var dto = new DTOs.Jobs.JobExecutionDto
		{
			IdJob = IdJob,
			TraceCorrelationId = TraceCorrelationId,
			StartUtc = StartUtc,
			EndUtc = EndUtc,
			IdJobStatus = IdJobStatus,
			JobStatus = JobStatus.FromId(IdJobStatus)!.Code!.ToCammelCase(removeUnderscores: false, throwIfEmpty: false)!,
			StatisticsStartHourAt = StatisticsStartHourUtc.ToLocalTime()
		};

		return dto;
	}
}
