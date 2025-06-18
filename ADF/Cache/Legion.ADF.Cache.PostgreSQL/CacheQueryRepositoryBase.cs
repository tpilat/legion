using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Npgsql;

namespace Legion.ADF.Cache.PostgreSQL;

public abstract partial class CacheQueryRepositoryBase : Legion.ADF.Cache.ICacheQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public CacheQueryRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Cache.PostgreSQL.ICacheQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheQueryDbContext>(scopeContext);

	protected NpgsqlConnection GetDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.GetDbConnection()!;

	protected NpgsqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (NpgsqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected NpgsqlConnection CreateNewDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
