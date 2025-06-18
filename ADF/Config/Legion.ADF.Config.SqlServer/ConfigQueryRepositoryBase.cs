using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Microsoft.Data.SqlClient;

namespace Legion.ADF.Config.SqlServer;

public abstract partial class ConfigQueryRepositoryBase : Legion.ADF.Config.IConfigQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public ConfigQueryRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Config.SqlServer.IConfigQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Config.SqlServer.IConfigQueryDbContext>(scopeContext);

	protected SqlConnection GetDbConnection()
		=> (SqlConnection)ConnectionProvider.GetDbConnection()!;

	protected SqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (SqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected SqlConnection CreateNewDbConnection()
		=> (SqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
