namespace Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription;

public partial interface IGetTopicSubscriptionByTopicAndName
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.TopicSubscription? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdTopicSubscriptionAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdTopicSubscription(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
