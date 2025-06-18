using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage;

public record GetSubscribedMessagesByIdMessageQuery(
	Guid IdMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.SubscribedMessage>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.SubscribedMessage>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<MessageBox.Model.SubscribedMessage, List<MessageBox.Model.SubscribedMessage>>;
