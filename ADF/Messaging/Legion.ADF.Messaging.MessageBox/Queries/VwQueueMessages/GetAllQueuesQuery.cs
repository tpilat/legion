using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage;

public record GetAllQueuesQuery(
	bool IncludeInactiveQueues,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.VwQueueMessages>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwQueueMessages>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<MessageBox.Model.VwQueueMessages, List<MessageBox.Model.VwQueueMessages>>;
