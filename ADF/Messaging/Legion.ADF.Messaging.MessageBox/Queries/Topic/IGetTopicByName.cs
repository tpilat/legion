namespace Legion.ADF.Messaging.MessageBox.Queries.Topic;

public partial interface IGetTopicByName
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.Topic> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.Topic?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.Topic? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdTopicAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdTopic(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
