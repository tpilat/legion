namespace Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage;

public partial interface IGetQueuedMessagesByIdMessage
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> ToResult(
		Legion.IScopeContext scopeContext);

	Task<Dictionary<Guid, DateTime>> ToMessageIds(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Dictionary<Guid, DateTime> ToMessageIds(
		Legion.IScopeContext scopeContext);
}
