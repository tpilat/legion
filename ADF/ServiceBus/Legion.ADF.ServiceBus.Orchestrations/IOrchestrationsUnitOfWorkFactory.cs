namespace Legion.ADF.ServiceBus.Orchestrations;

public partial interface IOrchestrationsUnitOfWorkFactory
{
	IOrchestrationsUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IOrchestrationsUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IOrchestrationsUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IOrchestrationsUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IOrchestrationsUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
