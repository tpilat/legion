using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage;

public record GetAllInboxQueuesQuery(
	bool IncludeInactiveQueues,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Inbox.Model.VwInboxQueueMessages>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwInboxQueueMessages>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Inbox.Model.VwInboxQueueMessages, List<Inbox.Model.VwInboxQueueMessages>>;
