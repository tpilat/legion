namespace Legion.ADF.Auth;

public partial interface IAuthUnitOfWorkFactory
{
	IAuthUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IAuthUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IAuthUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IAuthUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IAuthUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
