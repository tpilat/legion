namespace Legion.ADF.Config.Queries.ConfigurationClass;

public partial interface IGetConfigurationClassByRootPath
{
	IQueryable<Legion.ADF.Config.Model.ConfigurationClass> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Config.Model.ConfigurationClass?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Config.Model.ConfigurationClass? ToResult(
		Legion.IScopeContext scopeContext);
}
