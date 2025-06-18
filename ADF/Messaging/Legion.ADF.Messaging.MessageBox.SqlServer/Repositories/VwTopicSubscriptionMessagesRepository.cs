using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.SqlServer.Model.Repositories;

public partial class VwTopicSubscriptionMessagesRepository : Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicSubscriptionMessagesRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages>? AccessControlManager => _accessControlManager.Value;

	public VwTopicSubscriptionMessagesRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext>(scopeContext)).VwTopicSubscriptionMessages;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage.IGetAllTopicSubscriptions GetAllTopicSubscriptions(
		Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage.GetAllTopicSubscriptionsQuery getAllTopicSubscriptions)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage.GetAllTopicSubscriptions(
			ConnectionProvider,
			getAllTopicSubscriptions);
}
