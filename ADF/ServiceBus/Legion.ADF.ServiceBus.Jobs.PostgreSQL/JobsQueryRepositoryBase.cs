using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Npgsql;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public abstract partial class JobsQueryRepositoryBase : Legion.ADF.ServiceBus.Jobs.IJobsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public JobsQueryRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsQueryDbContext>(scopeContext);

	protected NpgsqlConnection GetDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.GetDbConnection()!;

	protected NpgsqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (NpgsqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected NpgsqlConnection CreateNewDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
