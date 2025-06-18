namespace Legion.ADF.Config;

public partial interface IConfigQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Config.Model.Repositories.IVwConfigurationClassRepository VwConfigurationClassRepository { get; }
}
