namespace Legion.ADF.Auth;

public partial interface IAuthQueryUnitOfWorkFactory
{
	IAuthQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IAuthQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IAuthQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IAuthQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IAuthQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
