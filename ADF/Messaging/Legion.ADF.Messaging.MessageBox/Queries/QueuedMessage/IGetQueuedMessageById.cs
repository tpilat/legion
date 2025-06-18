namespace Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage;

public partial interface IGetQueuedMessageById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.QueuedMessage? ToResult(
		Legion.IScopeContext scopeContext);
}
