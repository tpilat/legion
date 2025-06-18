namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopic;

public partial interface IGetVwTopicById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopic> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.VwTopic?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.VwTopic? ToResult(
		Legion.IScopeContext scopeContext);
}
