using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.PostgreSQL.Model.Repositories;

public partial class VwReloadableCacheKeyRepository : Legion.ADF.Cache.PostgreSQL.CacheQueryRepositoryBase, Legion.ADF.Cache.ICacheQueryRepository<Legion.ADF.Cache.Model.VwReloadableCacheKey>, Legion.ADF.Cache.Model.Repositories.IVwReloadableCacheKeyRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.VwReloadableCacheKey>?> _accessControlManager;

	private Legion.ADF.Cache.PostgreSQL.ICacheQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.VwReloadableCacheKey>? AccessControlManager => _accessControlManager.Value;

	public VwReloadableCacheKeyRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.VwReloadableCacheKey>>());
	}

	public IQueryable<Legion.ADF.Cache.Model.VwReloadableCacheKey> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Cache.Model.VwReloadableCacheKey> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheQueryDbContext>(scopeContext)).VwReloadableCacheKey;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Cache.Model.VwReloadableCacheKey> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Cache.Model.VwReloadableCacheKey> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
