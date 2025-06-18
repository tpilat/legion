using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Microsoft.Data.SqlClient;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public abstract partial class InboxRepositoryBase : Legion.ADF.Messaging.Inbox.IInboxRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public InboxRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext>(scopeContext);

	protected SqlConnection GetDbConnection()
		=> (SqlConnection)ConnectionProvider.GetDbConnection()!;

	protected SqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (SqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected SqlConnection CreateNewDbConnection()
		=> (SqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
