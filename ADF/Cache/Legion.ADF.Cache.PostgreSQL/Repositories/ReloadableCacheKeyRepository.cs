using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.PostgreSQL.Model.Repositories;

public partial class ReloadableCacheKeyRepository : Legion.ADF.Cache.PostgreSQL.CacheRepositoryBase, Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.ReloadableCacheKey>, Legion.ADF.Cache.Model.Repositories.IReloadableCacheKeyRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.ReloadableCacheKey>?> _accessControlManager;

	private Legion.ADF.Cache.PostgreSQL.ICacheDbContext? _context;

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
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext)).ReloadableCacheKey;

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
		IEnumerable<Legion.ADF.Cache.Model.ReloadableCacheKey> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetReloadableCacheKeyTableInfo();
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
