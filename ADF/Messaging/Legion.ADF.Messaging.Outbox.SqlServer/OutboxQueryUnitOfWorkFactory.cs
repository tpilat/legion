namespace Legion.ADF.Messaging.Outbox.SqlServer;

public partial class OutboxQueryUnitOfWorkFactory : IOutboxQueryUnitOfWorkFactory
{
	public IOutboxQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider)
	{
		if (connectionProvider is not Legion.EntityFrameworkCore.IEFConnectionProvider efConnectionProvider)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Database.IConnectionProvider)} is not an instance of {nameof(Legion.EntityFrameworkCore.IEFConnectionProvider)}");
			return null!;
		}

		return new OutboxQueryUnitOfWork(efConnectionProvider);
	}

	public IOutboxQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork)
	{
		if (unitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Model.Repositories.IUnitOfWork)} is not an instance of {nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork)}");
			return null!;
		}

		return new OutboxQueryUnitOfWork(dbUnitOfWork);
	}

	public IOutboxQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork)
	{
		if (queryUnitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
		{
			Legion.Throw.InvalidOperationException($"The provided {nameof(Legion.Model.Repositories.IQueryUnitOfWork)} is not an instance of {nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork)}");
			return null!;
		}

		return new OutboxQueryUnitOfWork(dbQueryUnitOfWork);
	}

	public IOutboxQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new OutboxQueryUnitOfWork(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);

	public IOutboxQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new OutboxQueryUnitOfWork(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
}
