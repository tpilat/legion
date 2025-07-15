namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobStatistics : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	internal static IResult<JobStatistics> Create(
		IScopeContext scopeContext,
		Job job,
		DateTime startHourUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<JobStatistics>();

		if (result.IsArgumentNull(scopeContext, job))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var host = new JobStatistics
		{
			__IsNewObject = true,
			IdJobStatistics = id,
			Job = job,
			StartHourUtc = startHourUtc,
			ExecutionCount = 1,
			ErrorCount = 0,
			DurationSumInSeconds = 0
		};

		var validationResult =
			DefaultDBValidator
				.Validate(host);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(host).Build();
	}

	internal IResult Start(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		ExecutionCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult Finish(
		IScopeContext scopeContext,
		bool isError,
		long durationInSeconds)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (isError)
			ErrorCount++;

		DurationSumInSeconds += durationInSeconds;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal DTOs.Jobs.JobStatisticsDto ToDto()
	{
		var dto = new DTOs.Jobs.JobStatisticsDto
		{
			IdJob = IdJob,
			StartHourUtc = StartHourUtc,
			ExecutionCount = ExecutionCount,
			ErrorCount = ErrorCount,
			AverageDurationInSeconds = DurationSumInSeconds / ExecutionCount,
		};

		return dto;
	}
}
