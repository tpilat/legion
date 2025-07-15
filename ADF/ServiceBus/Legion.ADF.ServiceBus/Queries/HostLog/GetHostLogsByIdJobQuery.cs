using Legion.MessageBus.Messages;

namespace Legion.ADF.ServiceBus.Queries.HostLog;

public record GetHostLogsByIdHostQuery(
	Guid IdHost,
	DateTime From,
	DateTime To,
	int PageIndex,
	int PageSize,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.HostLog>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.HostLog>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.HostLog, List<Model.HostLog>>;
