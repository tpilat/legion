namespace Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance;

public partial interface IExistsMessageBoxInstanceById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool ToResult(
		Legion.IScopeContext scopeContext);
}
