using Legion.MessageBus.Messages;

namespace Legion.ADF.ServiceBus.Queries.Job;

public record GetJobByIdQuery(
	Guid IdJob,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Job>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Job>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Job, Model.Job?>;
