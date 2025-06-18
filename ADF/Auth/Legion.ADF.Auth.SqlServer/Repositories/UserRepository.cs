using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.SqlServer.Model.Repositories;

public partial class UserRepository : Legion.ADF.Auth.SqlServer.AuthRepositoryBase, Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.User>, Legion.ADF.Auth.Model.Repositories.IUserRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.User>?> _accessControlManager;

	private Legion.ADF.Auth.SqlServer.IAuthDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.User>? AccessControlManager => _accessControlManager.Value;

	public UserRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.User>>());
	}

	public IQueryable<Legion.ADF.Auth.Model.User> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Auth.Model.User> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.SqlServer.IAuthDbContext>(scopeContext)).User;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Auth.Model.User> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Auth.Model.User> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Auth.Queries.User.IGetUserByExternalLoginProviderIdentifier GetUserByExternalLoginProviderIdentifier(
		Legion.ADF.Auth.Queries.User.GetUserByExternalLoginProviderIdentifierQuery getUserByExternalLoginProviderIdentifierQuery)
		=> new Queries.User.GetUserByExternalLoginProviderIdentifier(
			ConnectionProvider,
			getUserByExternalLoginProviderIdentifierQuery);

	public Legion.ADF.Auth.Queries.User.IGetUserById GetUserById(
		Legion.ADF.Auth.Queries.User.GetUserByIdQuery getUserByIdQuery)
		=> new Queries.User.GetUserById(
			ConnectionProvider,
			getUserByIdQuery);

	public Legion.ADF.Auth.Queries.User.IGetUserByNormalizedEmail GetUserByNormalizedEmail(
		Legion.ADF.Auth.Queries.User.GetUserByNormalizedEmailQuery getUserByNormalizedEmailQuery)
		=> new Queries.User.GetUserByNormalizedEmail(
			ConnectionProvider,
			getUserByNormalizedEmailQuery);

	public Legion.ADF.Auth.Queries.User.IGetUserByNormalizedLogin GetUserByNormalizedLogin(
		Legion.ADF.Auth.Queries.User.GetUserByNormalizedLoginQuery getUserByNormalizedLoginQuery)
		=> new Queries.User.GetUserByNormalizedLogin(
			ConnectionProvider,
			getUserByNormalizedLoginQuery);

	public Legion.ADF.Auth.Queries.User.IGetUserByNormalizedRoleName GetUserByNormalizedRoleName(
		Legion.ADF.Auth.Queries.User.GetUserByNormalizedRoleNameQuery getUserByNormalizedRoleNameQuery)
		=> new Queries.User.GetUserByNormalizedRoleName(
			ConnectionProvider,
			getUserByNormalizedRoleNameQuery);

	public Legion.ADF.Auth.Queries.User.IGetUserPermissionsAndRolesById GetUserPermissionsAndRolesById(
		Legion.ADF.Auth.Queries.User.GetUserPermissionsAndRolesByIdQuery getUserPermissionsAndRolesByIdQuery)
		=> new Queries.User.GetUserPermissionsAndRolesById(
			ConnectionProvider,
			getUserPermissionsAndRolesByIdQuery);

	public Legion.ADF.Auth.Queries.User.IGetUsersByClaimValue GetUsersByClaimValue(
		Legion.ADF.Auth.Queries.User.GetUsersByClaimValueQuery getUsersByClaimValueQuery)
		=> new Queries.User.GetUsersByClaimValue(
			ConnectionProvider,
			getUsersByClaimValueQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Auth.Model.User entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.User.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Auth.Model.User entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.User.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Auth.Model.User> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.User.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.User> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.User.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auth.Model.User entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.User.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.User> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.User.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Auth.Model.User> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetUserTableInfo();

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
