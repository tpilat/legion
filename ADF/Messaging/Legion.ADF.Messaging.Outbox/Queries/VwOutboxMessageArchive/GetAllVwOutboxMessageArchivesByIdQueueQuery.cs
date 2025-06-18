using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive;

public record GetAllVwOutboxMessageArchivesByIdQueueQuery(
	Guid IdOutboxQueue,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwOutboxMessageArchive>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwOutboxMessageArchive>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwOutboxMessageArchive, List<Model.VwOutboxMessageArchive>>;
