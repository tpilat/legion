namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage;

public partial interface IGetAllTopicSubscriptions
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
