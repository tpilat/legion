using Legion.MessageBus.Messages;

namespace Legion.ADF.Logs.Queries.UnstructuredLog;

public record GetUnstructuredLogByIdQuery(
	Guid IdUnstructuredLog,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UnstructuredLog>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UnstructuredLog>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UnstructuredLog, Model.UnstructuredLog?>;
