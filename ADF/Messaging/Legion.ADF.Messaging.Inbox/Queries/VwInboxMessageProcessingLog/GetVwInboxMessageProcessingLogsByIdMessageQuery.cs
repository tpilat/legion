using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog;

public record GetVwInboxMessageProcessingLogsByIdMessageQuery(
	Guid IdInboxMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwInboxMessageProcessingLog>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwInboxMessageProcessingLog>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwInboxMessageProcessingLog, List<Model.VwInboxMessageProcessingLog>>;
