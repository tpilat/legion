using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage;

public record GetAllVwInboxMessagesByIdQueueQuery(
	Guid IdInboxQueue,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwInboxMessage>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwInboxMessage>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwInboxMessage, List<Model.VwInboxMessage>>;
