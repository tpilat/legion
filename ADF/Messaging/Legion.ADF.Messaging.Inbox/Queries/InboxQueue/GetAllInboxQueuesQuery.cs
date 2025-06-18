using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxQueue;

public record GetAllInboxQueuesQuery(
	bool IncludeInactiveQueues,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Inbox.Model.InboxQueue>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.InboxQueue>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Inbox.Model.InboxQueue, List<Inbox.Model.InboxQueue>>;
