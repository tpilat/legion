namespace Legion.ADF.Messaging.MessageBox.Queries.MessageType;

public partial interface IGetAllMessageTypes
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.MessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.MessageType> ToResult(
		Legion.IScopeContext scopeContext);
}
