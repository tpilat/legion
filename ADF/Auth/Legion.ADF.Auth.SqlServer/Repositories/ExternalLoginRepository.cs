using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.SqlServer.Model.Repositories;

public partial class ExternalLoginRepository : Legion.ADF.Auth.SqlServer.AuthRepositoryBase, Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.ExternalLogin>, Legion.ADF.Auth.Model.Repositories.IExternalLoginRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.ExternalLogin>?> _accessControlManager;

	private Legion.ADF.Auth.SqlServer.IAuthDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.ExternalLogin>? AccessControlManager => _accessControlManager.Value;

	public ExternalLoginRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.ExternalLogin>>());
	}

	public IQueryable<Legion.ADF.Auth.Model.ExternalLogin> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Auth.Model.ExternalLogin> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.SqlServer.IAuthDbContext>(scopeContext)).ExternalLogin;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Auth.Model.ExternalLogin> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Auth.Model.ExternalLogin> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Auth.Queries.ExternalLogin.IGetExternalLoginByExternalIdentifier GetExternalLoginByExternalIdentifier(
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginByExternalIdentifierQuery getExternalLoginByExternalIdentifierQuery)
		=> new Queries.ExternalLogin.GetExternalLoginByExternalIdentifier(
			ConnectionProvider,
			getExternalLoginByExternalIdentifierQuery);

	public Legion.ADF.Auth.Queries.ExternalLogin.IGetExternalLoginByUserAndExternalIdentifier GetExternalLoginByUserAndExternalIdentifier(
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginByUserAndExternalIdentifierQuery getExternalLoginByUserAndExternalIdentifierQuery)
		=> new Queries.ExternalLogin.GetExternalLoginByUserAndExternalIdentifier(
			ConnectionProvider,
			getExternalLoginByUserAndExternalIdentifierQuery);

	public Legion.ADF.Auth.Queries.ExternalLogin.IGetExternalLoginsByUserId GetExternalLoginsByUserId(
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginsByUserIdQuery getExternalLoginsByUserIdQuery)
		=> new Queries.ExternalLogin.GetExternalLoginsByUserId(
			ConnectionProvider,
			getExternalLoginsByUserIdQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Auth.Model.ExternalLogin entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ExternalLogin.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Auth.Model.ExternalLogin entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ExternalLogin.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Auth.Model.ExternalLogin> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ExternalLogin.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.ExternalLogin> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ExternalLogin.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auth.Model.ExternalLogin entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ExternalLogin.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Auth.Model.ExternalLogin> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ExternalLogin.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Auth.Model.ExternalLogin> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetExternalLoginTableInfo();

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
