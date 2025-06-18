namespace Legion.ADF.Logs.PostgreSQL;

public partial class LogsUnitOfWorkFactory : ILogsUnitOfWorkFactory
{
	public ILogsUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider)
	{
		if (connectionProvider is not Legion.EntityFrameworkCore.IEFConnectionProvider efConnectionProvider)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Database.IConnectionProvider)} is not an instance of {nameof(Legion.EntityFrameworkCore.IEFConnectionProvider)}");
			return null!;
		}

		return new LogsUnitOfWork(efConnectionProvider);
	}

	public ILogsUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork)
	{
		if (unitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Model.Repositories.IUnitOfWork)} is not an instance of {nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork)}");
			return null!;
		}

		return new LogsUnitOfWork(dbUnitOfWork);
	}

	public ILogsUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork)
	{
		if (queryUnitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Model.Repositories.IQueryUnitOfWork)} is not an instance of {nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork)}");
			return null!;
		}

		return new LogsUnitOfWork(dbQueryUnitOfWork);
	}

	public ILogsUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new LogsUnitOfWork(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);

	public ILogsUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new LogsUnitOfWork(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
}
