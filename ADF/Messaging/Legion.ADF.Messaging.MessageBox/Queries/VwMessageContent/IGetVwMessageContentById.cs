namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageContent;

public partial interface IGetVwMessageContentById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.VwMessageContent? ToResult(
		Legion.IScopeContext scopeContext);
}
