using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog;

public record GetVwMessageProcessingLogsByIdMessageQuery(
	Guid IdMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwMessageProcessingLog>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwMessageProcessingLog>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwMessageProcessingLog, List<Model.VwMessageProcessingLog>>;
