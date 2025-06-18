namespace Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription;

public partial interface IGetAllTopicSubscriptionsByTopic
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<Guid>> GetIdTopicSubscriptionsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Guid> GetIdTopicSubscriptions(
		Legion.IScopeContext scopeContext);
}
