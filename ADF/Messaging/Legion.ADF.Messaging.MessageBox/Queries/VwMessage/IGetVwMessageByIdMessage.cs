namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessage;

public partial interface IGetVwMessageByIdMessage
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.VwMessage?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.VwMessage? ToResult(
		Legion.IScopeContext scopeContext);
}
