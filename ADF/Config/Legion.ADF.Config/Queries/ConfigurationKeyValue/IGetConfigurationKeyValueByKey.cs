namespace Legion.ADF.Config.Queries.ConfigurationKeyValue;

public partial interface IGetConfigurationKeyValueByKey
{
	IQueryable<Legion.ADF.Config.Model.ConfigurationKeyValue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Config.Model.ConfigurationKeyValue?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Config.Model.ConfigurationKeyValue? ToResult(
		Legion.IScopeContext scopeContext);
}
