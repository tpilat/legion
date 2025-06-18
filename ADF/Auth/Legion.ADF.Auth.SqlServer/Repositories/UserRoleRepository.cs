using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.SqlServer.Model.Repositories;

public partial class UserRoleRepository : Legion.ADF.Auth.SqlServer.AuthRepositoryBase, Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.UserRole>, Legion.ADF.Auth.Model.Repositories.IUserRoleRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.UserRole>?> _accessControlManager;

	private Legion.ADF.Auth.SqlServer.IAuthDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.UserRole>? AccessControlManager => _accessControlManager.Value;

	public UserRoleRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.UserRole>>());
	}

	public IQueryable<Legion.ADF.Auth.Model.UserRole> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Auth.Model.UserRole> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.SqlServer.IAuthDbContext>(scopeContext)).UserRole;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Auth.Model.UserRole> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Auth.Model.UserRole> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Auth.Queries.UserRole.IGetUserRoleByIdUserAndIdRole GetUserRoleByIdUserAndIdRole(
		Legion.ADF.Auth.Queries.UserRole.GetUserRoleByIdUserAndIdRoleQuery getUserRoleByIdUserAndIdRole)
		=> new Queries.UserRole.GetUserRoleByIdUserAndIdRole(
			ConnectionProvider,
			getUserRoleByIdUserAndIdRole);

	public Legion.ADF.Auth.Queries.UserRole.IGetUserRoleByIdUserAndNormalizedRoleName GetUserRoleByIdUserAndNormalizedRoleName(
		Legion.ADF.Auth.Queries.UserRole.GetUserRoleByIdUserAndNormalizedRoleNameQuery getUserRoleByIdUserAndNormalizedRoleName)
		=> new Queries.UserRole.GetUserRoleByIdUserAndNormalizedRoleName(
			ConnectionProvider,
			getUserRoleByIdUserAndNormalizedRoleName);

	public Legion.ADF.Auth.Queries.UserRole.IIsInRole IsInRole(
		Legion.ADF.Auth.Queries.UserRole.IsInRoleQuery IsInRole)
		=> new Queries.UserRole.IsInRole(
			ConnectionProvider,
			IsInRole);

	public void Add(IScopeContext scopeContext, Legion.ADF.Auth.Model.UserRole entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.UserRole.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Auth.Model.UserRole entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.UserRole.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Auth.Model.UserRole> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.UserRole.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.UserRole> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.UserRole.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auth.Model.UserRole entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.UserRole.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.UserRole> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.UserRole.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Auth.Model.UserRole> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetUserRoleTableInfo();

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
