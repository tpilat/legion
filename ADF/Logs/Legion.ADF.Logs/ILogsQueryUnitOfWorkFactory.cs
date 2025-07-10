namespace Legion.ADF.Logs;

public partial interface ILogsQueryUnitOfWorkFactory
{
	ILogsQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	ILogsQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	ILogsQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	ILogsQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	ILogsQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
