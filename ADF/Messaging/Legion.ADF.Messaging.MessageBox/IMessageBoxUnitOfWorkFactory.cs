namespace Legion.ADF.Messaging.MessageBox;

public partial interface IMessageBoxUnitOfWorkFactory
{
	IMessageBoxUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IMessageBoxUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IMessageBoxUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IMessageBoxUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IMessageBoxUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
