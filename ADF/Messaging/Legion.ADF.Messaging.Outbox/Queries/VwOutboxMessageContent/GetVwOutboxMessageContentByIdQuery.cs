using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent;

public record GetVwOutboxMessageContentByIdQuery(
	Guid IdOutboxMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwOutboxMessageContent>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwOutboxMessageContent>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwOutboxMessageContent, Model.VwOutboxMessageContent?>;
