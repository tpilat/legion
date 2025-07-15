namespace Legion.Model.Repositories;

public partial interface IQueryUnitOfWorkFactory<TQUoW>
	where TQUoW : Legion.Model.Repositories.IQueryUnitOfWork
{
	TQUoW Create(Legion.Database.IConnectionProvider connectionProvider);

	TQUoW Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);

	TQUoW Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);

	TQUoW Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	TQUoW CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
