using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.PostgreSQL.Model.Repositories;

public partial class VwConfigurationClassRepository : Legion.ADF.Config.PostgreSQL.ConfigQueryRepositoryBase, Legion.ADF.Config.IConfigQueryRepository<Legion.ADF.Config.Model.VwConfigurationClass>, Legion.ADF.Config.Model.Repositories.IVwConfigurationClassRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.VwConfigurationClass>?> _accessControlManager;

	private Legion.ADF.Config.PostgreSQL.IConfigQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.VwConfigurationClass>? AccessControlManager => _accessControlManager.Value;

	public VwConfigurationClassRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.VwConfigurationClass>>());
	}

	public IQueryable<Legion.ADF.Config.Model.VwConfigurationClass> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Config.Model.VwConfigurationClass> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Config.PostgreSQL.IConfigQueryDbContext>(scopeContext)).VwConfigurationClass;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Config.Model.VwConfigurationClass> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Config.Model.VwConfigurationClass> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
