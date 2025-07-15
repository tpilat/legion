using Legion.MessageBus.Messages;

namespace Legion.ADF.ServiceBus.Queries.JobLog;

public record GetJobLogsByIdJobQuery(
	Guid IdJob,
	Guid? IdJobExecution,
	DateTime From,
	DateTime To,
	int PageIndex,
	int PageSize,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.JobLog>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.JobLog>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.JobLog, List<Model.JobLog>>;
