namespace Legion.ADF.Messaging.Outbox;

public partial interface IOutboxUnitOfWorkFactory
{
	IOutboxUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IOutboxUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IOutboxUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IOutboxUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IOutboxUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
