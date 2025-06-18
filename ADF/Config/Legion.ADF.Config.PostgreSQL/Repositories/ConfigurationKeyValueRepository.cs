using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.PostgreSQL.Model.Repositories;

public partial class ConfigurationKeyValueRepository : Legion.ADF.Config.PostgreSQL.ConfigRepositoryBase, Legion.ADF.Config.IConfigRepository<Legion.ADF.Config.Model.ConfigurationKeyValue>, Legion.ADF.Config.Model.Repositories.IConfigurationKeyValueRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationKeyValue>?> _accessControlManager;

	private Legion.ADF.Config.PostgreSQL.IConfigDbContext? _context;

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
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Config.PostgreSQL.IConfigDbContext>(scopeContext)).ConfigurationKeyValue;

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
		IEnumerable<Legion.ADF.Config.Model.ConfigurationKeyValue> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetConfigurationKeyValueTableInfo();
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
