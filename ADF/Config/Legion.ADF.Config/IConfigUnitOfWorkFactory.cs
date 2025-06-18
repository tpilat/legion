namespace Legion.ADF.Config;

public partial interface IConfigUnitOfWorkFactory
{
	IConfigUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IConfigUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IConfigUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IConfigUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConfigUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
