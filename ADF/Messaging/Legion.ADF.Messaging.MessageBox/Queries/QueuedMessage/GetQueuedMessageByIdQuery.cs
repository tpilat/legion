using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage;

public record GetQueuedMessageByIdQuery(
	Guid IdQueuedMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.QueuedMessage>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.QueuedMessage>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<MessageBox.Model.QueuedMessage, MessageBox.Model.QueuedMessage?>;
