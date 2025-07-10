namespace Legion.ADF.ServiceBus;

public partial interface IServiceBusQueryUnitOfWorkFactory
{
	IServiceBusQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IServiceBusQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IServiceBusQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IServiceBusQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IServiceBusQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
