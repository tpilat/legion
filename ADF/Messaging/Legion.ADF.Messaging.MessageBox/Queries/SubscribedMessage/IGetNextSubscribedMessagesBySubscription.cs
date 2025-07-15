namespace Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage;

public partial interface IGetNextSubscribedMessagesBySubscription
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> ToResult(
		Legion.IScopeContext scopeContext);

	Task<Dictionary<Guid, DateTime>> ToSubscribedMessageIds(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Dictionary<Guid, DateTime> ToSubscribedMessageIds(
		Legion.IScopeContext scopeContext);
}
