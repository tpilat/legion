namespace Legion.ADF.Messaging.Inbox;

public partial interface IInboxUnitOfWorkFactory
{
	IInboxUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IInboxUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IInboxUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IInboxUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IInboxUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
