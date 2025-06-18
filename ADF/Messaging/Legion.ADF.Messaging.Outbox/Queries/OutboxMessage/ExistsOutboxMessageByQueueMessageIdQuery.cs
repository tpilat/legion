using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessage;

public record ExistsOutboxMessageByQueueMessageIdQuery(
	Guid IdOutboxQueue,
	string MessageId,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<Outbox.Model.OutboxMessage>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.OutboxMessage>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<Outbox.Model.OutboxMessage, bool>;
