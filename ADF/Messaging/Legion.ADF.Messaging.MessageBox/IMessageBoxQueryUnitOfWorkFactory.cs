namespace Legion.ADF.Messaging.MessageBox;

public partial interface IMessageBoxQueryUnitOfWorkFactory
{
	IMessageBoxQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IMessageBoxQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IMessageBoxQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IMessageBoxQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IMessageBoxQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
