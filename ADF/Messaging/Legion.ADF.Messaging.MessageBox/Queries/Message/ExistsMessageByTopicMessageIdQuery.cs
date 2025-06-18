using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.Message;

public record ExistsMessageByTopicMessageIdQuery(
	Guid IdTopic,
	string MessageId,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.Message>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Message>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<MessageBox.Model.Message, bool>;
