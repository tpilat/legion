using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Npgsql;

namespace Legion.ADF.Audit.PostgreSQL;

public abstract partial class AuditRepositoryBase : Legion.ADF.Audit.IAuditRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public AuditRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Audit.PostgreSQL.IAuditDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Audit.PostgreSQL.IAuditDbContext>(scopeContext);

	protected NpgsqlConnection GetDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.GetDbConnection()!;

	protected NpgsqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (NpgsqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected NpgsqlConnection CreateNewDbConnection()
		=> (NpgsqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
