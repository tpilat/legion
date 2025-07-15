using Legion.MessageBus.Messages;

namespace Legion.ADF.ServiceBus.Queries.JobExecution;

public record GetJobExecutionByIdQuery(
	Guid IdJob,
	DateTime FromUtc,
	DateTime ToUtc,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.JobExecution>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.JobExecution>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.JobExecution, List<Model.JobExecution>>;
