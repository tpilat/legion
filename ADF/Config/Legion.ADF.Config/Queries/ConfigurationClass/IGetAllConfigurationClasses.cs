namespace Legion.ADF.Config.Queries.ConfigurationClass;

public partial interface IGetAllConfigurationClasses
{
	IQueryable<Legion.ADF.Config.Model.ConfigurationClass> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Config.Model.ConfigurationClass>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Config.Model.ConfigurationClass> ToResult(
		Legion.IScopeContext scopeContext);
}
