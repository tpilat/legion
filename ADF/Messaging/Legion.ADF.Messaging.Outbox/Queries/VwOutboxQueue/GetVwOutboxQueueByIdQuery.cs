using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue;

public record GetVwOutboxQueueByIdQuery(
	Guid IdOutboxQueue,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwOutboxQueue>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwOutboxQueue>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwOutboxQueue, Model.VwOutboxQueue?>;
