using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.SqlServer.Model.Repositories;

public partial class VwBlockedMessageTypeRepository : Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwBlockedMessageTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType>? AccessControlManager => _accessControlManager.Value;

	public VwBlockedMessageTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext>(scopeContext)).VwBlockedMessageType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType.IGetAllVwBlockedMessageTypes GetAllVwBlockedMessageTypes(
		Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType.GetAllVwBlockedMessageTypesQuery getAllVwBlockedMessageTypes)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType.GetAllVwBlockedMessageTypes(
			ConnectionProvider,
			getAllVwBlockedMessageTypes);
}
