using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.SqlServer.Model.Repositories;

public partial class VwMessageProcessingLogRepository : Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageProcessingLogRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog>? AccessControlManager => _accessControlManager.Value;

	public VwMessageProcessingLogRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext>(scopeContext)).VwMessageProcessingLog;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog.IGetVwMessageProcessingLogsByIdMessage GetVwMessageProcessingLogsByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery getVwMessageProcessingLogByIdMessage)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessage(
			ConnectionProvider,
			getVwMessageProcessingLogByIdMessage);
}
