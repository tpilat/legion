namespace Legion.ADF.Messaging.Inbox;

public partial interface IInboxQueryUnitOfWorkFactory
{
	IInboxQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IInboxQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IInboxQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IInboxQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IInboxQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
