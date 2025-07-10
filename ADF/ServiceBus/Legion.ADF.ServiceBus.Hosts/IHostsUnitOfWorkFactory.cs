namespace Legion.ADF.ServiceBus.Hosts;

public partial interface IHostsUnitOfWorkFactory
{
	IHostsUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IHostsUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IHostsUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IHostsUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IHostsUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
