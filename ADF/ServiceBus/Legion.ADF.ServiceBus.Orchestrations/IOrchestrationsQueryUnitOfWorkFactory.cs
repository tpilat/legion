namespace Legion.ADF.ServiceBus.Orchestrations;

public partial interface IOrchestrationsQueryUnitOfWorkFactory
{
	IOrchestrationsQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IOrchestrationsQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IOrchestrationsQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IOrchestrationsQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IOrchestrationsQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
