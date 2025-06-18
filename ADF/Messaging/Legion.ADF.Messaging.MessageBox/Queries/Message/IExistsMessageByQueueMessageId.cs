namespace Legion.ADF.Messaging.MessageBox.Queries.Message;

public partial interface IExistsMessageByQueueMessageId
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.Message> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdMessageAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdMessage(
		Legion.IScopeContext scopeContext);
}
