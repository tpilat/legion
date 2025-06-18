namespace Legion.ADF.ESB.Orchestrations;

public partial interface IOrchestrationsQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork
{
	Legion.ADF.ESB.Orchestrations.Model.Repositories.IVwOrchestrationRepository VwOrchestrationRepository { get; }
}
