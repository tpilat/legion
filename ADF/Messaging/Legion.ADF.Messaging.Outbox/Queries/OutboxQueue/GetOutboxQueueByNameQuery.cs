using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxQueue;

public record GetOutboxQueueByNameQuery(
	string Name,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Outbox.Model.OutboxQueue>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.OutboxQueue>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Outbox.Model.OutboxQueue, Outbox.Model.OutboxQueue?>;
