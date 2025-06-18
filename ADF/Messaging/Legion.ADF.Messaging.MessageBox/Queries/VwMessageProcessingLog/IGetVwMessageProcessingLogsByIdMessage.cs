namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog;

public partial interface IGetVwMessageProcessingLogsByIdMessage
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> ToResult(
		Legion.IScopeContext scopeContext);
}
