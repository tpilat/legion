namespace Legion.ADF.ServiceBus.Jobs;

public partial interface IJobsQueryUnitOfWorkFactory
{
	IJobsQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IJobsQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IJobsQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IJobsQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IJobsQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
