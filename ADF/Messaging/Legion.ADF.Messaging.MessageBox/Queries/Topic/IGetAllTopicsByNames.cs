namespace Legion.ADF.Messaging.MessageBox.Queries.Topic;

public partial interface IGetAllTopicsByNames
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.Topic> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.Topic>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.Topic> ToResult(
		Legion.IScopeContext scopeContext);
}
