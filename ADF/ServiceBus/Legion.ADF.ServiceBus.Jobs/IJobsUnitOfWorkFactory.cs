namespace Legion.ADF.ServiceBus.Jobs;

public partial interface IJobsUnitOfWorkFactory
{
	IJobsUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IJobsUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IJobsUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IJobsUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IJobsUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
