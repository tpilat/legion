using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType;

public record GetOutboxMessageTypeByNamespaceQuery(
	string Namespace,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Outbox.Model.OutboxMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.OutboxMessageType>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Outbox.Model.OutboxMessageType, Outbox.Model.OutboxMessageType?>;
