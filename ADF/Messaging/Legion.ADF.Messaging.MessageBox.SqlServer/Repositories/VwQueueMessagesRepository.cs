using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.SqlServer.Model.Repositories;

public partial class VwQueueMessagesRepository : Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueueMessagesRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages>? AccessControlManager => _accessControlManager.Value;

	public VwQueueMessagesRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext>(scopeContext)).VwQueueMessages;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage.IGetAllQueues GetAllQueues(
		Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage.GetAllQueuesQuery getAllQueues)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage.GetAllQueues(
			ConnectionProvider,
			getAllQueues);
}
