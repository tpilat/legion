namespace Legion.ADF.ServiceBus;

public partial interface IServiceBusQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.ServiceBus.Model.Repositories.IVwHostRepository VwHostRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IVwJobRepository VwJobRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IVwOrchestrationRepository VwOrchestrationRepository { get; }
}
