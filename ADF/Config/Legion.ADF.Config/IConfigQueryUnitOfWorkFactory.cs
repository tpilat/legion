namespace Legion.ADF.Config;

public partial interface IConfigQueryUnitOfWorkFactory
{
	IConfigQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IConfigQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IConfigQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IConfigQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConfigQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
