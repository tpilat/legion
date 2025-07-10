namespace Legion.ADF.Cache;

public partial interface ICacheQueryUnitOfWorkFactory
{
	ICacheQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	ICacheQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	ICacheQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	ICacheQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	ICacheQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
