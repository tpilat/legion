using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType;

public record GetAllVwBlockedOutboxMessageTypesQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<Outbox.Model.VwBlockedOutboxMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwBlockedOutboxMessageType>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<Outbox.Model.VwBlockedOutboxMessageType, List<Outbox.Model.VwBlockedOutboxMessageType>>;

