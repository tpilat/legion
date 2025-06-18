using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessage;

public record GetInboxMessageByIdQuery(
	Guid IdInboxMessage,
	bool IncludeContent,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<Inbox.Model.InboxMessage>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.InboxMessage>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<Inbox.Model.InboxMessage, Inbox.Model.InboxMessage?>;
