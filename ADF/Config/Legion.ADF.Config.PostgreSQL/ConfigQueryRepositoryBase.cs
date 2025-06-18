using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Npgsql;

namespace Legion.ADF.Config.PostgreSQL;

public abstract partial class ConfigQueryRepositoryBase : Legion.ADF.Config.IConfigQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public ConfigQueryRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Config.PostgreSQL.IConfigQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Config.PostgreSQL.IConfigQueryDbContext>(scopeContext);

	protected NpgsqlConnection GetDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.GetDbConnection()!;

	protected NpgsqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (NpgsqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected NpgsqlConnection CreateNewDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
