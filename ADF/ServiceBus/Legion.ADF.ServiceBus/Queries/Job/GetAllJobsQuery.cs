using Legion.MessageBus.Messages;

namespace Legion.ADF.ServiceBus.Queries.Job;

public record GetAllJobsQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Job>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Job>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Job, List<Model.Job>>;
