using Legion.MessageBus.Messages;

namespace Legion.ADF.ServiceBus.Queries.JobStatistics;

public record GetJobStatisticsByJobIdAndStartHourQuery(
	Guid IdJob,
	DateTime StartHourUtc,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.JobStatistics>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.JobStatistics>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.JobStatistics, Model.JobStatistics>;
