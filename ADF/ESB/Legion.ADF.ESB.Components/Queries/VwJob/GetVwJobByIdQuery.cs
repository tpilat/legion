using Legion.MessageBus.Messages;

namespace Legion.ADF.ESB.Components.Queries.VwJob;

public record GetVwJobByIdQuery(
	Guid IdJob,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Components.Model.VwJob>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwJob>(true, DisableCahce, QueryableBuilder),
		IQueryRequest<Components.Model.VwJob, Components.Model.VwJob?>;
