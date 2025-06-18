namespace Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage;

public partial interface IGetSubscribedMessageById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage? ToResult(
		Legion.IScopeContext scopeContext);
}
