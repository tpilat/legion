using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage;

public record GetAllOutboxQueuesQuery(
	bool IncludeInactiveQueues,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Outbox.Model.VwOutboxQueueMessages>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwOutboxQueueMessages>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Outbox.Model.VwOutboxQueueMessages, List<Outbox.Model.VwOutboxQueueMessages>>;
