namespace Legion.ADF.Config.Queries.ConfigurationKeyValue;

public partial interface IGetAllConfigurationKeyValues
{
	IQueryable<Legion.ADF.Config.Model.ConfigurationKeyValue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Config.Model.ConfigurationKeyValue>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Config.Model.ConfigurationKeyValue> ToResult(
		Legion.IScopeContext scopeContext);
}
