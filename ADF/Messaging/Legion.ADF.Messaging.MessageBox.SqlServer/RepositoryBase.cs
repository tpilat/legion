using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Microsoft.Data.SqlClient;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public abstract partial class RepositoryBase : Legion.ADF.Messaging.MessageBox.IRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public RepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext>(scopeContext);

	protected SqlConnection GetDbConnection()
		=> (SqlConnection)ConnectionProvider.GetDbConnection()!;

	protected SqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (SqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected SqlConnection CreateNewDbConnection()
		=> (SqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
