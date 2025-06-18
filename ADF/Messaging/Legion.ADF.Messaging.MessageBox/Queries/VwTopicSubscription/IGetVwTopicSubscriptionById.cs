namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscription;

public partial interface IGetVwTopicSubscriptionById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription? ToResult(
		Legion.IScopeContext scopeContext);
}
