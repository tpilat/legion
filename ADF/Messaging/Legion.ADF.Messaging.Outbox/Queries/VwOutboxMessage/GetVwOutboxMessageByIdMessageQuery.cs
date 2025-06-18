using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage;

public record GetVwOutboxMessageByIdMessageQuery(
	Guid IdOutboxMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwOutboxMessage>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwOutboxMessage>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwOutboxMessage, Model.VwOutboxMessage?>;
