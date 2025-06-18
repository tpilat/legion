using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType;

public record GetAllVwBlockedInboxMessageTypesQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<Inbox.Model.VwBlockedInboxMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwBlockedInboxMessageType>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<Inbox.Model.VwBlockedInboxMessageType, List<Inbox.Model.VwBlockedInboxMessageType>>;

