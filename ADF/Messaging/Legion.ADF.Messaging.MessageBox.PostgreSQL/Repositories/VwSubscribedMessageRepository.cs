using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories;

public partial class VwSubscribedMessageRepository : Legion.ADF.Messaging.MessageBox.PostgreSQL.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwSubscribedMessageRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage>? AccessControlManager => _accessControlManager.Value;

	public VwSubscribedMessageRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext>(scopeContext)).VwSubscribedMessage;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
