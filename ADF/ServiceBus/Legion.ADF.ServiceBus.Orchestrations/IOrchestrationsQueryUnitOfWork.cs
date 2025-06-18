namespace Legion.ADF.ServiceBus.Orchestrations;

public partial interface IOrchestrationsQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IVwOrchestrationRepository VwOrchestrationRepository { get; }
}
