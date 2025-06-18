using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories;

public partial class VwJobRepository : Legion.ADF.ServiceBus.Jobs.SqlServer.JobsQueryRepositoryBase, Legion.ADF.ServiceBus.Jobs.IJobsQueryRepository<Legion.ADF.ServiceBus.Jobs.Model.VwJob>, Legion.ADF.ServiceBus.Jobs.Model.Repositories.IVwJobRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.VwJob>?> _accessControlManager;

	private Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.VwJob>? AccessControlManager => _accessControlManager.Value;

	public VwJobRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.VwJob>>());
	}

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.VwJob> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.VwJob> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsQueryDbContext>(scopeContext)).VwJob;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.VwJob> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.VwJob> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
