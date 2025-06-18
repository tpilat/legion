using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Npgsql;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public abstract partial class OrchestrationsRepositoryBase : Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public OrchestrationsRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsDbContext>(scopeContext);

	protected NpgsqlConnection GetDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.GetDbConnection()!;

	protected NpgsqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (NpgsqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected NpgsqlConnection CreateNewDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
