using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories;

public partial class VwMessageRepository : Legion.ADF.Messaging.MessageBox.PostgreSQL.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwMessage>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessage>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessage>? AccessControlManager => _accessControlManager.Value;

	public VwMessageRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessage>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessage> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessage> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext>(scopeContext)).VwMessage;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessage> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.VwMessage.IGetAllVwMessagesByIdQueue GetAllVwMessagesByIdQueue(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetAllVwMessagesByIdQueueQuery getAllVwMessagesByIdQueue)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetAllVwMessagesByIdQueue(
			ConnectionProvider,
			getAllVwMessagesByIdQueue);

	public Legion.ADF.Messaging.MessageBox.Queries.VwMessage.IGetVwMessageByIdMessage GetVwMessageByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetVwMessageByIdMessageQuery getVwMessageByIdMessage)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetVwMessageByIdMessage(
			ConnectionProvider,
			getVwMessageByIdMessage);
}
