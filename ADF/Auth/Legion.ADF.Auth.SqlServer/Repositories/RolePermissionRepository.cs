using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.SqlServer.Model.Repositories;

public partial class RolePermissionRepository : Legion.ADF.Auth.SqlServer.AuthRepositoryBase, Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.RolePermission>, Legion.ADF.Auth.Model.Repositories.IRolePermissionRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.RolePermission>?> _accessControlManager;

	private Legion.ADF.Auth.SqlServer.IAuthDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.RolePermission>? AccessControlManager => _accessControlManager.Value;

	public RolePermissionRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.RolePermission>>());
	}

	public IQueryable<Legion.ADF.Auth.Model.RolePermission> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Auth.Model.RolePermission> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.SqlServer.IAuthDbContext>(scopeContext)).RolePermission;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Auth.Model.RolePermission> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Auth.Model.RolePermission> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Auth.Queries.RolePermission.IGetRolePermissionsByRoleIdAndClaimValue GetRolePermissionsByRoleIdAndClaimValue(
		Legion.ADF.Auth.Queries.RolePermission.GetRolePermissionsByRoleIdAndClaimValueQuery getRolePermissionsByRoleIdAndClaimValueQuery)
		=> new Queries.RolePermission.GetRolePermissionsByRoleIdAndClaimValue(
			ConnectionProvider,
			getRolePermissionsByRoleIdAndClaimValueQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Auth.Model.RolePermission entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RolePermission.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Auth.Model.RolePermission entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.RolePermission.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Auth.Model.RolePermission> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RolePermission.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.RolePermission> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.RolePermission.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auth.Model.RolePermission entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RolePermission.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.RolePermission> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RolePermission.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Auth.Model.RolePermission> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetRolePermissionTableInfo();

		var isNewConnection = false;
		var connection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (connection == null)
			Throw.InvalidOperationException($"{nameof(connection)} == null");

		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			var rows = entities.Select(e => e.ToDictionary()).ToList();
			var dataTable = tableInfo.ToDataTable(rows);

			using var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(connection);

			bulkCopy.DestinationTableName = tableInfo.FullTableName;

			foreach (var column in tableInfo.Columns)
				bulkCopy.ColumnMappings.Add(column.PropertyName, column.ColumnName);

			bulkCopy.WriteToServer(dataTable);
		}
		finally
		{
			if (isNewConnection)
				connection.Dispose();
		}

		return result;
	}
}
