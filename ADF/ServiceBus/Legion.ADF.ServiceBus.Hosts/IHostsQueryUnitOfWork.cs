namespace Legion.ADF.ServiceBus.Hosts;

public partial interface IHostsQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.ServiceBus.Hosts.Model.Repositories.IVwHostRepository VwHostRepository { get; }
}
