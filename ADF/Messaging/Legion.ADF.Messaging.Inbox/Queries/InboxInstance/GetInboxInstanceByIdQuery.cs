using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxInstance;

public record GetInboxInstanceByIdQuery(
	Guid IdInboxInstance,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Inbox.Model.InboxInstance>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.InboxInstance>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Inbox.Model.InboxInstance, Inbox.Model.InboxInstance?>;
