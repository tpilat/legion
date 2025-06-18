using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.SqlServer.Model.Repositories;

public partial class ConfigurationKeyValueRepository : Legion.ADF.Config.SqlServer.ConfigRepositoryBase, Legion.ADF.Config.IConfigRepository<Legion.ADF.Config.Model.ConfigurationKeyValue>, Legion.ADF.Config.Model.Repositories.IConfigurationKeyValueRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationKeyValue>?> _accessControlManager;

	private Legion.ADF.Config.SqlServer.IConfigDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationKeyValue>? AccessControlManager => _accessControlManager.Value;

	public ConfigurationKeyValueRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationKeyValue>>());
	}

	public IQueryable<Legion.ADF.Config.Model.ConfigurationKeyValue> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Config.Model.ConfigurationKeyValue> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Config.SqlServer.IConfigDbContext>(scopeContext)).ConfigurationKeyValue;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Config.Model.ConfigurationKeyValue> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Config.Model.ConfigurationKeyValue> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Config.Queries.ConfigurationKeyValue.IGetAllConfigurationKeyValues GetAllConfigurationKeyValues(
		Legion.ADF.Config.Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesQuery getAllConfigurationKeyValuesQuery)
		=> new Legion.ADF.Config.Queries.ConfigurationKeyValue.GetAllConfigurationKeyValues(
			ConnectionProvider,
			getAllConfigurationKeyValuesQuery);

	public Legion.ADF.Config.Queries.ConfigurationKeyValue.IGetAllConfigurationKeyValuesByPath GetAllConfigurationKeyValuesByPath(
		Legion.ADF.Config.Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesByPathQuery getAllConfigurationKeyValuesByPath)
		=> new Legion.ADF.Config.Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesByPath(
			ConnectionProvider,
			getAllConfigurationKeyValuesByPath);

	public Legion.ADF.Config.Queries.ConfigurationKeyValue.IGetConfigurationKeyValueByKey GetConfigurationKeyValueByKey(
		Legion.ADF.Config.Queries.ConfigurationKeyValue.GetConfigurationKeyValueByKeyQuery getConfigurationKeyValueByKey)
		=> new Legion.ADF.Config.Queries.ConfigurationKeyValue.GetConfigurationKeyValueByKey(
			ConnectionProvider,
			getConfigurationKeyValueByKey);

	public void Add(IScopeContext scopeContext, Legion.ADF.Config.Model.ConfigurationKeyValue entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationKeyValue.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Config.Model.ConfigurationKeyValue entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ConfigurationKeyValue.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Config.Model.ConfigurationKeyValue> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationKeyValue.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Config.Model.ConfigurationKeyValue> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ConfigurationKeyValue.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Config.Model.ConfigurationKeyValue entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationKeyValue.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Config.Model.ConfigurationKeyValue> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ConfigurationKeyValue.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Config.Model.ConfigurationKeyValue> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetConfigurationKeyValueTableInfo();

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
