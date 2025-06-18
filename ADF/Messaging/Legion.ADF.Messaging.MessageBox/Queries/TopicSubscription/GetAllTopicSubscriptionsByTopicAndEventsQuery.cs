using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription;

public record GetAllTopicSubscriptionsByTopicAndEventsQuery(
	Guid IdTopic,
	List<string> ReceivedEventNamespaces,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.TopicSubscription>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.TopicSubscription>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<MessageBox.Model.TopicSubscription, List<MessageBox.Model.TopicSubscription>>;
