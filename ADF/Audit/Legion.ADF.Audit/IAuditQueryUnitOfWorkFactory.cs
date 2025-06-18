namespace Legion.ADF.Audit;

public partial interface IAuditQueryUnitOfWorkFactory
{
	IAuditQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IAuditQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IAuditQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IAuditQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IAuditQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
