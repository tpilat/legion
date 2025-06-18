namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive;

public partial interface IGetVwMessageArchiveByIdMessage
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive? ToResult(
		Legion.IScopeContext scopeContext);
}
