using Legion.MessageBus.Messages;

namespace Legion.ADF.ServiceBus.Queries.Host;

public record GetHostByIdQuery(
	Guid IdHost,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Host>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Host>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Host, Model.Host?>;
