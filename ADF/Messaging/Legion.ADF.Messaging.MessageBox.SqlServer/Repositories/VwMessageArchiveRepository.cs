using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.SqlServer.Model.Repositories;

public partial class VwMessageArchiveRepository : Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxQueryRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageArchiveRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive>? AccessControlManager => _accessControlManager.Value;

	public VwMessageArchiveRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext>(scopeContext)).VwMessageArchive;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.IGetAllVwMessageArchivesByIdQueue GetAllVwMessageArchivesByIdQueue(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery getAllVwMessageArchivesByIdQueue)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueue(
			ConnectionProvider,
			getAllVwMessageArchivesByIdQueue);

	public Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.IGetVwMessageArchiveByIdMessage GetVwMessageArchiveByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.GetVwMessageArchiveByIdMessageQuery getVwMessageArchiveByIdMessage)
		=> new Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.GetVwMessageArchiveByIdMessage(
			ConnectionProvider,
			getVwMessageArchiveByIdMessage);
}
