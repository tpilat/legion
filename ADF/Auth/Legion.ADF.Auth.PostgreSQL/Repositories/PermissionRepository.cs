using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.PostgreSQL.Model.Repositories;

public partial class PermissionRepository : Legion.ADF.Auth.PostgreSQL.AuthRepositoryBase, Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.Permission>, Legion.ADF.Auth.Model.Repositories.IPermissionRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Permission>?> _accessControlManager;

	private Legion.ADF.Auth.PostgreSQL.IAuthDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Permission>? AccessControlManager => _accessControlManager.Value;

	public PermissionRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Permission>>());
	}

	public IQueryable<Legion.ADF.Auth.Model.Permission> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Auth.Model.Permission> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.PostgreSQL.IAuthDbContext>(scopeContext)).Permission;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Auth.Model.Permission> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Auth.Model.Permission> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Auth.Queries.Permission.IGetAllPermissionsWithRoles GetAllPermissionsWithRoles(
		Legion.ADF.Auth.Queries.Permission.GetAllPermissionsWithRolesQuery getAllPermissionsWithRolesQuery)
		=> new Queries.Permission.GetAllPermissionsWithRoles(
			ConnectionProvider,
			getAllPermissionsWithRolesQuery);

	public Legion.ADF.Auth.Queries.Permission.IGetClaimsByRoleId GetClaimsByRoleId(
		Legion.ADF.Auth.Queries.Permission.GetClaimsByRoleIdQuery getClaimsByRoleIdQuery)
		=> new Queries.Permission.GetClaimsByRoleId(
			ConnectionProvider,
			getClaimsByRoleIdQuery);

	public Legion.ADF.Auth.Queries.Permission.IGetClaimsByUserId GetClaimsByUserId(
		Legion.ADF.Auth.Queries.Permission.GetClaimsByUserIdQuery getClaimsByUserIdQuery)
		=> new Queries.Permission.GetClaimsByUserId(
			ConnectionProvider,
			getClaimsByUserIdQuery);

	public Legion.ADF.Auth.Queries.Permission.IGetPermissionByClaimValue GetPermissionByClaimValue(
		Legion.ADF.Auth.Queries.Permission.GetPermissionByClaimValueQuery getPermissionByClaimValueQuery)
		=> new Queries.Permission.GetPermissionByClaimValue(
			ConnectionProvider,
			getPermissionByClaimValueQuery);

	public Legion.ADF.Auth.Queries.Permission.IGetPermissionsByClaimValues GetPermissionsByClaimValues(
		Legion.ADF.Auth.Queries.Permission.GetPermissionsByClaimValuesQuery getPermissionsByClaimValuesQuery)
		=> new Queries.Permission.GetPermissionsByClaimValues(
			ConnectionProvider,
			getPermissionsByClaimValuesQuery);

	public Legion.ADF.Auth.Queries.Permission.IGetPermissionsByRoleId GetPermissionsByRoleId(
		Legion.ADF.Auth.Queries.Permission.GetPermissionsByRoleIdQuery getPermissionsByRoleIdQuery)
		=> new Queries.Permission.GetPermissionsByRoleId(
			ConnectionProvider,
			getPermissionsByRoleIdQuery);

	public Legion.ADF.Auth.Queries.Permission.IGetPermissionsByRoleIdAndClaimValue GetPermissionsByRoleIdAndClaimValue(
		Legion.ADF.Auth.Queries.Permission.GetPermissionsByRoleIdAndClaimValueQuery getPermissionsByRoleIdAndClaimValueQuery)
		=> new Queries.Permission.GetPermissionsByRoleIdAndClaimValue(
			ConnectionProvider,
			getPermissionsByRoleIdAndClaimValueQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Auth.Model.Permission entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Permission.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Auth.Model.Permission entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.Permission.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Auth.Model.Permission> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Permission.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.Permission> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.Permission.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auth.Model.Permission entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Permission.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.Permission> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Permission.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Auth.Model.Permission> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetPermissionTableInfo();
		string sql = $"COPY {tableInfo.FullTableName} ({tableInfo.CommaSeparatedColumns}) FROM STDIN (FORMAT BINARY)";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($"{nameof(npgsqlConnection)} == null");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);
		
			foreach (var entity in entities)
			{
				writer.StartRow();
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					writer.Write(kvp.Value, columnTypes[kvp.Key]);
			}

			result = writer.Complete();
		}
		finally
		{
			if (isNewConnection)
			{
				writer?.Dispose();
				npgsqlConnection.Dispose();
			}
		}

		return result;
	}

	public async Task<ulong> BulkInsertAsync(
		IEnumerable<Legion.ADF.Auth.Model.Permission> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetPermissionTableInfo();
		string sql = $"COPY {tableInfo.FullTableName} ({tableInfo.CommaSeparatedColumns}) FROM STDIN (FORMAT BINARY)";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($"{nameof(npgsqlConnection)} == null");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);

			foreach (var entity in entities)
			{
				await writer.StartRowAsync(cancellationToken).ConfigureAwait(false);
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					await writer.WriteAsync(kvp.Value, columnTypes[kvp.Key], cancellationToken).ConfigureAwait(false);
			}

			result = await writer.CompleteAsync(cancellationToken).ConfigureAwait(false);
		}
		finally
		{
			if (isNewConnection)
			{
				if (writer != null)
					await writer.DisposeAsync();

				await npgsqlConnection.DisposeAsync();
			}
		}

		return result;
	}
}
