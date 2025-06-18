namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

public partial class JobsQueryUnitOfWorkFactory : IJobsQueryUnitOfWorkFactory
{
	public IJobsQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider)
	{
		if (connectionProvider is not Legion.EntityFrameworkCore.IEFConnectionProvider efConnectionProvider)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Database.IConnectionProvider)} is not an instance of {nameof(Legion.EntityFrameworkCore.IEFConnectionProvider)}");
			return null!;
		}

		return new JobsQueryUnitOfWork(efConnectionProvider);
	}

	public IJobsQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork)
	{
		if (unitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Model.Repositories.IUnitOfWork)} is not an instance of {nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork)}");
			return null!;
		}

		return new JobsQueryUnitOfWork(dbUnitOfWork);
	}

	public IJobsQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork)
	{
		if (queryUnitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Model.Repositories.IQueryUnitOfWork)} is not an instance of {nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork)}");
			return null!;
		}

		return new JobsQueryUnitOfWork(dbQueryUnitOfWork);
	}

	public IJobsQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new JobsQueryUnitOfWork(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);

	public IJobsQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new JobsQueryUnitOfWork(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
}
