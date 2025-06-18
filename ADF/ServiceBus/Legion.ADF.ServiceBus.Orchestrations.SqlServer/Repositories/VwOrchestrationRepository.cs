using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Orchestrations.SqlServer.Model.Repositories;

public partial class VwOrchestrationRepository : Legion.ADF.ServiceBus.Orchestrations.SqlServer.OrchestrationsQueryRepositoryBase, Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsQueryRepository<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration>, Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IVwOrchestrationRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration>?> _accessControlManager;

	private Legion.ADF.ServiceBus.Orchestrations.SqlServer.IOrchestrationsQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration>? AccessControlManager => _accessControlManager.Value;

	public VwOrchestrationRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration>>());
	}

	public IQueryable<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Orchestrations.SqlServer.IOrchestrationsQueryDbContext>(scopeContext)).VwOrchestration;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
