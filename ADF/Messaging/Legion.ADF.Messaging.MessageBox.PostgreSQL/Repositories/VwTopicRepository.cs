using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories;

public partial class VwTopicRepository : Legion.ADF.Messaging.MessageBox.PostgreSQL.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwTopic>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopic>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopic>? AccessControlManager => _accessControlManager.Value;

	public VwTopicRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopic>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopic> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopic> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext>(scopeContext)).VwTopic;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopic> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwTopic> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.VwTopic.IGetVwTopicById GetVwTopicById(
		Legion.ADF.Messaging.MessageBox.Queries.VwTopic.GetVwTopicByIdQuery getVwTopicById)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwTopic.GetVwTopicById(
			ConnectionProvider,
			getVwTopicById);
}
