using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscription;

public record GetVwTopicSubscriptionByIdQuery(
	Guid IdTopicSubscription,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwTopicSubscription>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwTopicSubscription>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwTopicSubscription, Model.VwTopicSubscription?>;
