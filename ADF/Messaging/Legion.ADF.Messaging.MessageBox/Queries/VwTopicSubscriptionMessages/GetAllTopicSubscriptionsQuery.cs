using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage;

public record GetAllTopicSubscriptionsQuery(
	bool IncludeInactiveTopicSubscriptions,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.VwTopicSubscriptionMessages>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwTopicSubscriptionMessages>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<MessageBox.Model.VwTopicSubscriptionMessages, List<MessageBox.Model.VwTopicSubscriptionMessages>>;
