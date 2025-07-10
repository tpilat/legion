using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Hosts.PostgreSQL.Model.Repositories;

public partial class VwHostRepository : Legion.ADF.ServiceBus.Hosts.PostgreSQL.HostsQueryRepositoryBase, Legion.ADF.ServiceBus.Hosts.IHostsQueryRepository<Legion.ADF.ServiceBus.Hosts.Model.VwHost>, Legion.ADF.ServiceBus.Hosts.Model.Repositories.IVwHostRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Hosts.Model.VwHost>?> _accessControlManager;

	private Legion.ADF.ServiceBus.Hosts.PostgreSQL.IHostsQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Hosts.Model.VwHost>? AccessControlManager => _accessControlManager.Value;

	public VwHostRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Hosts.Model.VwHost>>());
	}

	public IQueryable<Legion.ADF.ServiceBus.Hosts.Model.VwHost> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.ServiceBus.Hosts.Model.VwHost> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Hosts.PostgreSQL.IHostsQueryDbContext>(scopeContext)).VwHost;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.ServiceBus.Hosts.Model.VwHost> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.ServiceBus.Hosts.Model.VwHost> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
