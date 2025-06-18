using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.PostgreSQL.Model.Repositories;

public partial class VwApplicationEntryRepository : Legion.ADF.Audit.PostgreSQL.AuditQueryRepositoryBase, Legion.ADF.Audit.IAuditQueryRepository<Legion.ADF.Audit.Model.VwApplicationEntry>, Legion.ADF.Audit.Model.Repositories.IVwApplicationEntryRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwApplicationEntry>?> _accessControlManager;

	private Legion.ADF.Audit.PostgreSQL.IAuditQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwApplicationEntry>? AccessControlManager => _accessControlManager.Value;

	public VwApplicationEntryRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwApplicationEntry>>());
	}

	public IQueryable<Legion.ADF.Audit.Model.VwApplicationEntry> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Audit.Model.VwApplicationEntry> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Audit.PostgreSQL.IAuditQueryDbContext>(scopeContext)).VwApplicationEntry;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Audit.Model.VwApplicationEntry> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Audit.Model.VwApplicationEntry> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
