namespace Legion.ADF.ServiceBus.Hosts;

public partial interface IHostsQueryUnitOfWorkFactory
{
	IHostsQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IHostsQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IHostsQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IHostsQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IHostsQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
