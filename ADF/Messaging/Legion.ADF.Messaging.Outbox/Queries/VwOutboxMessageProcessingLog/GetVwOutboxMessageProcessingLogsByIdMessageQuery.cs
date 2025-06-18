using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog;

public record GetVwOutboxMessageProcessingLogsByIdMessageQuery(
	Guid IdOutboxMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwOutboxMessageProcessingLog>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwOutboxMessageProcessingLog>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwOutboxMessageProcessingLog, List<Model.VwOutboxMessageProcessingLog>>;
