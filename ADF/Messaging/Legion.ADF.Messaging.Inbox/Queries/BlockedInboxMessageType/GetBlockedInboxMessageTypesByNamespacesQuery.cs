using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType;

public record GetBlockedInboxMessageTypesByNamespacesQuery(
	List<string> Namespaces,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<Inbox.Model.BlockedInboxMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.BlockedInboxMessageType>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<Inbox.Model.BlockedInboxMessageType, List<Inbox.Model.BlockedInboxMessageType>>;

