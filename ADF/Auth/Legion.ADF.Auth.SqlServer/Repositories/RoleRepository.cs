using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.SqlServer.Model.Repositories;

public partial class RoleRepository : Legion.ADF.Auth.SqlServer.AuthRepositoryBase, Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.Role>, Legion.ADF.Auth.Model.Repositories.IRoleRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Role>?> _accessControlManager;

	private Legion.ADF.Auth.SqlServer.IAuthDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Role>? AccessControlManager => _accessControlManager.Value;

	public RoleRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Role>>());
	}

	public IQueryable<Legion.ADF.Auth.Model.Role> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Auth.Model.Role> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.SqlServer.IAuthDbContext>(scopeContext)).Role;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Auth.Model.Role> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Auth.Model.Role> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Auth.Queries.Role.IGetAllRolesWithPermissions GetAllRolesWithPermissions(
		Legion.ADF.Auth.Queries.Role.GetAllRolesWithPermissionsQuery getAllRolesWithPermissionsQuery)
		=> new Queries.Role.GetAllRolesWithPermissions(
			ConnectionProvider,
			getAllRolesWithPermissionsQuery);

	public Legion.ADF.Auth.Queries.Role.IGetRoleById GetRoleById(
		Legion.ADF.Auth.Queries.Role.GetRoleByIdQuery getRoleByIdQuery)
		=> new Queries.Role.GetRoleById(
			ConnectionProvider,
			getRoleByIdQuery);

	public Legion.ADF.Auth.Queries.Role.IGetRoleByNormalizedName GetRoleByNormalizedName(
		Legion.ADF.Auth.Queries.Role.GetRoleByNormalizedNameQuery getRoleByNormalizedNameQuery)
		=> new Queries.Role.GetRoleByNormalizedName(
			ConnectionProvider,
			getRoleByNormalizedNameQuery);

	public Legion.ADF.Auth.Queries.Role.IGetRolesByIdUser GetRolesByIdUser(
		Legion.ADF.Auth.Queries.Role.GetRolesByIdUserQuery getRolesByIdUserQuery)
		=> new Queries.Role.GetRolesByIdUser(
			ConnectionProvider,
			getRolesByIdUserQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Auth.Model.Role entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Role.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Auth.Model.Role entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.Role.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Auth.Model.Role> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Role.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.Role> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.Role.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auth.Model.Role entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Role.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.Role> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Role.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Auth.Model.Role> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetRoleTableInfo();

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
