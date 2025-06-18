using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxQueue;

public record GetVwInboxQueueByIdQuery(
	Guid IdInboxQueue,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwInboxQueue>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwInboxQueue>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwInboxQueue, Model.VwInboxQueue?>;
