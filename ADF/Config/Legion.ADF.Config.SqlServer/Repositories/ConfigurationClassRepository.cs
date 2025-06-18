using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.SqlServer.Model.Repositories;

public partial class ConfigurationClassRepository : Legion.ADF.Config.SqlServer.ConfigRepositoryBase, Legion.ADF.Config.IConfigRepository<Legion.ADF.Config.Model.ConfigurationClass>, Legion.ADF.Config.Model.Repositories.IConfigurationClassRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationClass>?> _accessControlManager;

	private Legion.ADF.Config.SqlServer.IConfigDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationClass>? AccessControlManager => _accessControlManager.Value;

	public ConfigurationClassRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationClass>>());
	}

	public IQueryable<Legion.ADF.Config.Model.ConfigurationClass> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Config.Model.ConfigurationClass> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Config.SqlServer.IConfigDbContext>(scopeContext)).ConfigurationClass;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Config.Model.ConfigurationClass> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Config.Model.ConfigurationClass> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Config.Queries.ConfigurationClass.IGetAllConfigurationClasses GetAllConfigurationClasses(
		Legion.ADF.Config.Queries.ConfigurationClass.GetAllConfigurationClassesQuery getAllConfigurationClassesQuery)
		=> new Legion.ADF.Config.Queries.ConfigurationClass.GetAllConfigurationClasses(
			ConnectionProvider,
			getAllConfigurationClassesQuery);

	public Legion.ADF.Config.Queries.ConfigurationClass.IGetConfigurationClassByRootPath GetConfigurationClassByRootPath(
		Legion.ADF.Config.Queries.ConfigurationClass.GetConfigurationClassByRootPathQuery getConfigurationClassByRootPathQuery)
		=> new Legion.ADF.Config.Queries.ConfigurationClass.GetConfigurationClassByRootPath(
			ConnectionProvider,
			getConfigurationClassByRootPathQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Config.Model.ConfigurationClass entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationClass.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Config.Model.ConfigurationClass entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ConfigurationClass.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Config.Model.ConfigurationClass> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationClass.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Config.Model.ConfigurationClass> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ConfigurationClass.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Config.Model.ConfigurationClass entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationClass.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Config.Model.ConfigurationClass> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationClass.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Config.Model.ConfigurationClass> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetConfigurationClassTableInfo();

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
