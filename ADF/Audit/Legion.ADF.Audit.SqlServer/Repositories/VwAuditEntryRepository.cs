using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.SqlServer.Model.Repositories;

public partial class VwAuditEntryRepository : Legion.ADF.Audit.SqlServer.AuditQueryRepositoryBase, Legion.ADF.Audit.IAuditQueryRepository<Legion.ADF.Audit.Model.VwAuditEntry>, Legion.ADF.Audit.Model.Repositories.IVwAuditEntryRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwAuditEntry>?> _accessControlManager;

	private Legion.ADF.Audit.SqlServer.IAuditQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwAuditEntry>? AccessControlManager => _accessControlManager.Value;

	public VwAuditEntryRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwAuditEntry>>());
	}

	public IQueryable<Legion.ADF.Audit.Model.VwAuditEntry> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Audit.Model.VwAuditEntry> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Audit.SqlServer.IAuditQueryDbContext>(scopeContext)).VwAuditEntry;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Audit.Model.VwAuditEntry> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Audit.Model.VwAuditEntry> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
