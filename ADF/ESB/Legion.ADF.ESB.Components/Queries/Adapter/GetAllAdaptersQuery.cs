using Legion.MessageBus.Messages;

namespace Legion.ADF.ESB.Components.Queries.Adapter;

public record GetAllAdaptersQuery(
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Components.Model.Adapter>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Adapter>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Components.Model.Adapter, List<Components.Model.Adapter>>;
