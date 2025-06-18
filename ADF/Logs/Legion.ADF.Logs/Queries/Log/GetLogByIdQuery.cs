using Legion.MessageBus.Messages;

namespace Legion.ADF.Logs.Queries.Log;

public record GetLogByIdQuery(
	Guid IdLog,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Log>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Log>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Log, Model.Log?>;
