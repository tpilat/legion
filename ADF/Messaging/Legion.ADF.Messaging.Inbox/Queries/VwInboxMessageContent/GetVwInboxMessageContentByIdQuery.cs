using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent;

public record GetVwInboxMessageContentByIdQuery(
	Guid IdInboxMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwInboxMessageContent>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwInboxMessageContent>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwInboxMessageContent, Model.VwInboxMessageContent?>;
