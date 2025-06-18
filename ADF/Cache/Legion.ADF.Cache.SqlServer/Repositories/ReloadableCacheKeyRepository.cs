using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.SqlServer.Model.Repositories;

public partial class ReloadableCacheKeyRepository : Legion.ADF.Cache.SqlServer.CacheRepositoryBase, Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.ReloadableCacheKey>, Legion.ADF.Cache.Model.Repositories.IReloadableCacheKeyRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.ReloadableCacheKey>?> _accessControlManager;

	private Legion.ADF.Cache.SqlServer.ICacheDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.ReloadableCacheKey>? AccessControlManager => _accessControlManager.Value;

	public ReloadableCacheKeyRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.ReloadableCacheKey>>());
	}

	public IQueryable<Legion.ADF.Cache.Model.ReloadableCacheKey> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Cache.Model.ReloadableCacheKey> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.SqlServer.ICacheDbContext>(scopeContext)).ReloadableCacheKey;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Cache.Model.ReloadableCacheKey> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Cache.Model.ReloadableCacheKey> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetAllReloadableCacheKeys GetAllReloadableCacheKeys(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetAllReloadableCacheKeysQuery getAllReloadableCacheKeys)
		=> new Legion.ADF.Cache.Queries.ReloadableCacheKey.GetAllReloadableCacheKeys(
			ConnectionProvider,
			getAllReloadableCacheKeys);

	public Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetAllReloadableCacheKeysByReloadAt GetAllReloadableCacheKeysByReloadAt(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetAllReloadableCacheKeysByReloadAtQuery getAllReloadableCacheKeysByReloadAt)
		=> new Legion.ADF.Cache.Queries.ReloadableCacheKey.GetAllReloadableCacheKeysByReloadAt(
			ConnectionProvider,
			getAllReloadableCacheKeysByReloadAt);

	public Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetReloadableCacheKeyByKey GetReloadableCacheKeyByKey(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByKeyQuery getReloadableCacheKeyByKey)
		=> new Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByKey(
			ConnectionProvider,
			getReloadableCacheKeyByKey);

	public Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetReloadableCacheKeyByTags GetReloadableCacheKeyByTags(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByTagsQuery getReloadableCacheKeyByTags)
		=> new Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByTags(
			ConnectionProvider,
			getReloadableCacheKeyByTags);

	public void Add(IScopeContext scopeContext, Legion.ADF.Cache.Model.ReloadableCacheKey entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ReloadableCacheKey.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Cache.Model.ReloadableCacheKey entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ReloadableCacheKey.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Cache.Model.ReloadableCacheKey> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ReloadableCacheKey.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Cache.Model.ReloadableCacheKey> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ReloadableCacheKey.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Cache.Model.ReloadableCacheKey entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ReloadableCacheKey.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Cache.Model.ReloadableCacheKey> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ReloadableCacheKey.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Cache.Model.ReloadableCacheKey> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetReloadableCacheKeyTableInfo();

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
