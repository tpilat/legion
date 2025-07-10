namespace Legion.ADF.ServiceBus;

public partial interface IServiceBusUnitOfWorkFactory
{
	IServiceBusUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IServiceBusUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IServiceBusUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IServiceBusUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IServiceBusUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
