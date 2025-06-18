using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwQueue;

public record GetVwQueueByIdQuery(
	Guid IdQueue,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwQueue>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwQueue>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwQueue, Model.VwQueue?>;
