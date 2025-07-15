namespace Legion.Model.Repositories;

public partial interface IUnitOfWorkFactory<TUoW>
	where TUoW : Legion.Model.Repositories.IUnitOfWork
{
	TUoW Create(Legion.Database.IConnectionProvider connectionProvider);

	TUoW Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);

	TUoW Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);

	TUoW Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	TUoW CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
