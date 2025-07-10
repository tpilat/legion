namespace Legion.ADF.Cache;

public partial interface ICacheUnitOfWorkFactory
{
	ICacheUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	ICacheUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	ICacheUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	ICacheUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	ICacheUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
