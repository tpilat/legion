using Legion;
using Legion.EntityFrameworkCore;
using Legion.Extensions;
using Legion.Model.Audit;
using Legion.Model.Repositories;
using Microsoft.Data.SqlClient;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public abstract partial class DomainEventsQueryRepositoryBase : Legion.ADF.Messaging.DomainEvents.IDomainEventsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
	public IEFConnectionProvider ConnectionProvider { get; }

	public DomainEventsQueryRepositoryBase(
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	protected Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsQueryDbContext>(scopeContext);

	protected SqlConnection GetDbConnection()
		=> (SqlConnection)ConnectionProvider.GetDbConnection()!;

	protected SqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
		=> (SqlConnection)ConnectionProvider.GetOrCreateNewDbConnection(out isNewConnection)!;

	protected SqlConnection CreateNewDbConnection()
		=> (SqlConnection)ConnectionProvider.CreateNewDbConnection()!;
}
