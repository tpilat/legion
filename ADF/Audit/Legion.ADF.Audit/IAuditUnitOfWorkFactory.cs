namespace Legion.ADF.Audit;

public partial interface IAuditUnitOfWorkFactory
{
	IAuditUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IAuditUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IAuditUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IAuditUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IAuditUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
