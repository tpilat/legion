namespace Legion.ADF.Logs;

public partial interface ILogsUnitOfWorkFactory
{
	ILogsUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	ILogsUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	ILogsUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	ILogsUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	ILogsUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
