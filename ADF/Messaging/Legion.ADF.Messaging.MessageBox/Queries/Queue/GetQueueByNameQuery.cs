using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.Queue;

public record GetQueueByNameQuery(
	string Name,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.Queue>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Queue>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<MessageBox.Model.Queue, MessageBox.Model.Queue?>;
