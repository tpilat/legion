namespace Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance;

public partial interface IGetMessageBoxInstanceById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance? ToResult(
		Legion.IScopeContext scopeContext);
}
