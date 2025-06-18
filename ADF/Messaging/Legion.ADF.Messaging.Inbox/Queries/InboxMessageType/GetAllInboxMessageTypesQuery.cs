using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessageType;

public record GetAllInboxMessageTypesQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Inbox.Model.InboxMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.InboxMessageType>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Inbox.Model.InboxMessageType, List<Inbox.Model.InboxMessageType>>;
