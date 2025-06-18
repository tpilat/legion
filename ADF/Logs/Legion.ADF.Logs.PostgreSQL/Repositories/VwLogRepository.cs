using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs.PostgreSQL.Model.Repositories;

public partial class VwLogRepository : Legion.ADF.Logs.PostgreSQL.LogsQueryRepositoryBase, Legion.ADF.Logs.ILogsQueryRepository<Legion.ADF.Logs.Model.VwLog>, Legion.ADF.Logs.Model.Repositories.IVwLogRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.VwLog>?> _accessControlManager;

	private Legion.ADF.Logs.PostgreSQL.ILogsQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.VwLog>? AccessControlManager => _accessControlManager.Value;

	public VwLogRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.VwLog>>());
	}

	public IQueryable<Legion.ADF.Logs.Model.VwLog> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Logs.Model.VwLog> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Logs.PostgreSQL.ILogsQueryDbContext>(scopeContext)).VwLog;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Logs.Model.VwLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Logs.Model.VwLog> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
