using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Microsoft.Data.SqlClient;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public abstract partial class MessageBoxQueryRepositoryBase : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public MessageBoxQueryRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext>(scopeContext);

	protected SqlConnection GetDbConnection()
		=> (SqlConnection)ConnectionProvider.GetDbConnection()!;

	protected SqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (SqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected SqlConnection CreateNewDbConnection()
		=> (SqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
