namespace Legion.ADF.ServiceBus.Jobs;

public partial interface IJobsQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IVwJobRepository VwJobRepository { get; }
}
