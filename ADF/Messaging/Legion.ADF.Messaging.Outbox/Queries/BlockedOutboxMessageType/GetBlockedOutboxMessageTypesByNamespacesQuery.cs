using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType;

public record GetBlockedOutboxMessageTypesByNamespacesQuery(
	List<string> Namespaces,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<Outbox.Model.BlockedOutboxMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.BlockedOutboxMessageType>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<Outbox.Model.BlockedOutboxMessageType, List<Outbox.Model.BlockedOutboxMessageType>>;

