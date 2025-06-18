using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.PostgreSQL.Model.Repositories;

public partial class VwUserRepository : Legion.ADF.Auth.PostgreSQL.AuthQueryRepositoryBase, Legion.ADF.Auth.IAuthQueryRepository<Legion.ADF.Auth.Model.VwUser>, Legion.ADF.Auth.Model.Repositories.IVwUserRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.VwUser>?> _accessControlManager;

	private Legion.ADF.Auth.PostgreSQL.IAuthQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.VwUser>? AccessControlManager => _accessControlManager.Value;

	public VwUserRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.VwUser>>());
	}

	public IQueryable<Legion.ADF.Auth.Model.VwUser> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Auth.Model.VwUser> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.PostgreSQL.IAuthQueryDbContext>(scopeContext)).VwUser;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Auth.Model.VwUser> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Auth.Model.VwUser> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
