namespace Legion.ADF.Messaging.MessageBox.Queries.Message;

public partial interface IGetMessageById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.Message> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.Message?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.Message? ToResult(
		Legion.IScopeContext scopeContext);
}
