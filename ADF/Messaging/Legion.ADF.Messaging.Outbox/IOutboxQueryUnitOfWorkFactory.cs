namespace Legion.ADF.Messaging.Outbox;

public partial interface IOutboxQueryUnitOfWorkFactory
{
	IOutboxQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IOutboxQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IOutboxQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IOutboxQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IOutboxQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
