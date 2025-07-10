using Legion.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Legion.ADF.Cache.SqlServer.Model.Repositories;

public partial class CacheDataRepository : Legion.ADF.Cache.SqlServer.CacheRepositoryBase, Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.CacheData>, Legion.ADF.Cache.Model.Repositories.ICacheDataRepository
{
	public async Task<Cache.Model.CacheData?> TryGetCacheDataAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);

		var keyHash = HashHelper.ComputeMD5Hash(key);
		var nowUtc = GlobalContext.Instance.UtcNow;

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.SqlServer.ICacheDbContext>(scopeContext);
		var cacheData = await context.CacheData.Where(x => x.KeyHash == keyHash).FirstOrDefaultAsync();
		if (cacheData == null)
			return null;

		if (cacheData.ExpiresUtc != null && cacheData.ExpiresUtc <= nowUtc)
		{
			context.CacheData.Remove(cacheData);
			await context.SaveAsync(scopeContext);
			return null;
		}

		if (cacheData.SlidingTime != null)
		{
			var updateSlidingResult = cacheData.UpdateSlidingAccess(scopeContext, nowUtc);
			updateSlidingResult.ThrowIfError(scopeContext, null, true);
		}

		//this method does not update row version!!!!

		var updateLastResult = cacheData.UpdateLastAccess(scopeContext, nowUtc);
		updateLastResult.ThrowIfError(scopeContext, null, true);

		await context.SaveAsync(scopeContext);

		return cacheData;
	}

	public async Task<string?> TryGetValueAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var cacheData = await TryGetCacheDataAsync(scopeContext, key, cancellationToken);
		return cacheData?.Value;
	}

	public async Task<bool> SetAsync(
		IScopeContext scopeContext,
		string key,
		string value,
		long? currentRowVersion,
		MemoryCacheEntryOptions? options,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);
		Throw.IfNullOrWhiteSpace(value);

		var nowUtc = GlobalContext.Instance.UtcNow;

		var createResult = Legion.ADF.Cache.Model.CacheData.Create(
			scopeContext,
			key,
			value,
			nowUtc,
			expiresUtc:
				options?.AbsoluteExpiration?.UtcDateTime ??
					(options?.AbsoluteExpirationRelativeToNow.HasValue == true
						? (nowUtc + options.AbsoluteExpirationRelativeToNow.Value)
						: null),
			slidingTime: options?.SlidingExpiration);

		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheData = createResult.Data!;

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.SqlServer.ICacheDbContext>(scopeContext);

		if (currentRowVersion.HasValue)
		{
			var sql = """
			MERGE INTO cache.[CacheData] AS target
			USING (SELECT 
				@keyHash AS KeyHash,
				@valueHash AS ValueHash,
				@key AS [Key],
				@value AS Value,
				@keyPrefix AS KeyPrefix450,
				@expiresUtc AS ExpiresUtc,
				@sliding AS SlidingTime,
				@now AS LastAccessedUtc,
				@currentRowVersion AS CurrentRowVersion
			) AS source
			ON target.[KeyHash] = source.[KeyHash] AND target.[RowVersion] = source.[CurrentRowVersion]
			WHEN MATCHED THEN
				UPDATE SET
					target.[ValueHash] = source.[ValueHash],
					target.[Key] = source.[Key],
					target.[Value] = source.[Value],
					target.[KeyPrefix450] = source.[KeyPrefix450],
					target.[ExpiresUtc] = source.[ExpiresUtc],
					target.[SlidingTime] = source.[SlidingTime],
					target.[LastAccessedUtc] = source.[LastAccessedUtc],
					target.[RowVersion] = target.[RowVersion] + 1
			WHEN NOT MATCHED THEN
				INSERT (
					[KeyHash], [ValueHash], [Key], [Value],
					[KeyPrefix450], [ExpiresUtc], [SlidingTime], [LastAccessedUtc], [RowVersion])
				VALUES (
					source.[KeyHash], source.[ValueHash], source.[Key], source.[Value],
					source.[KeyPrefix450], source.[ExpiresUtc], source.[SlidingTime], source.[LastAccessedUtc], 0);
		""";

			var rows = await context.Database.ExecuteSqlRawAsync(sql, [
				new Microsoft.Data.SqlClient.SqlParameter("keyHash", cacheData.KeyHash),
				new Microsoft.Data.SqlClient.SqlParameter("valueHash", cacheData.ValueHash),
				new Microsoft.Data.SqlClient.SqlParameter("key", cacheData.Key),
				new Microsoft.Data.SqlClient.SqlParameter("value", cacheData.Value),
				new Microsoft.Data.SqlClient.SqlParameter("keyPrefix", cacheData.KeyPrefix450),
				new Microsoft.Data.SqlClient.SqlParameter("expiresUtc", cacheData.ExpiresUtc ?? (object)DBNull.Value),
				new Microsoft.Data.SqlClient.SqlParameter("sliding", cacheData.SlidingTime ?? (object)DBNull.Value),
				new Microsoft.Data.SqlClient.SqlParameter("now", nowUtc),
				new Microsoft.Data.SqlClient.SqlParameter("currentRowVersion", currentRowVersion ?? (object)DBNull.Value)
				]);

			return rows == 1;
		}
		else
		{
			var sql = """
			MERGE INTO cache.[CacheData] AS target
			USING (SELECT 
				@keyHash AS KeyHash,
				@valueHash AS ValueHash,
				@key AS [Key],
				@value AS Value,
				@keyPrefix AS KeyPrefix450,
				@expiresUtc AS ExpiresUtc,
				@sliding AS SlidingTime,
				@now AS LastAccessedUtc
			) AS source
			ON target.[KeyHash] = source.[KeyHash]
			WHEN MATCHED THEN
				UPDATE SET
					target.[ValueHash] = source.[ValueHash],
					target.[Key] = source.[Key],
					target.[Value] = source.[Value],
					target.[KeyPrefix450] = source.[KeyPrefix450],
					target.[ExpiresUtc] = source.[ExpiresUtc],
					target.[SlidingTime] = source.[SlidingTime],
					target.[LastAccessedUtc] = source.[LastAccessedUtc],
					target.[RowVersion] = target.[RowVersion] + 1
			WHEN NOT MATCHED THEN
				INSERT (
					[KeyHash], [ValueHash], [Key], [Value],
					[KeyPrefix450], [ExpiresUtc], [SlidingTime], [LastAccessedUtc], [RowVersion])
				VALUES (
					source.[KeyHash], source.[ValueHash], source.[Key], source.[Value],
					source.[KeyPrefix450], source.[ExpiresUtc], source.[SlidingTime], source.[LastAccessedUtc], 0);
		""";

			var rows = await context.Database.ExecuteSqlRawAsync(sql, [
				new Microsoft.Data.SqlClient.SqlParameter("keyHash", cacheData.KeyHash),
				new Microsoft.Data.SqlClient.SqlParameter("valueHash", cacheData.ValueHash),
				new Microsoft.Data.SqlClient.SqlParameter("key", cacheData.Key),
				new Microsoft.Data.SqlClient.SqlParameter("value", cacheData.Value),
				new Microsoft.Data.SqlClient.SqlParameter("keyPrefix", cacheData.KeyPrefix450),
				new Microsoft.Data.SqlClient.SqlParameter("expiresUtc", cacheData.ExpiresUtc ?? (object)DBNull.Value),
				new Microsoft.Data.SqlClient.SqlParameter("sliding", cacheData.SlidingTime ?? (object)DBNull.Value),
				new Microsoft.Data.SqlClient.SqlParameter("now", nowUtc)
				]);

			return rows == 1;
		}
	}

	public async Task<bool> TryUpdateAsync(
		IScopeContext scopeContext,
		string key,
		string oldValue,
		string newValue,
		long currentRowVersion,
		MemoryCacheEntryOptions? options = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);
		Throw.IfNullOrWhiteSpace(oldValue);
		Throw.IfNullOrWhiteSpace(newValue);

		var nowUtc = GlobalContext.Instance.UtcNow;

		var createResult = Legion.ADF.Cache.Model.CacheData.Create(
			scopeContext,
			key,
			newValue,
			nowUtc,
			expiresUtc: options?.AbsoluteExpiration?.UtcDateTime ??
						  (options?.AbsoluteExpirationRelativeToNow.HasValue == true
							? (nowUtc + options.AbsoluteExpirationRelativeToNow.Value)
							: null),
			slidingTime: options?.SlidingExpiration);

		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheData = createResult.Data!;

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.SqlServer.ICacheDbContext>(scopeContext);
		var sql = """
			UPDATE cache.[CacheData]
			SET
				[Value] = @newValue,
				[ValueHash] = @newValueHash,
				[Key] = @key,
				[KeyPrefix450] = @keyPrefix,
				[ExpiresUtc] = @expiresUtc,
				[SlidingTime] = @sliding,
				[LastAccessedUtc] = @now,
				[RowVersion] = @currentRowVersion + 1
			WHERE [KeyHash] = @keyHash AND [ValueHash] = @oldValueHash AND [RowVersion] = @currentRowVersion;
		""";

		var oldValueHash = HashHelper.ComputeMD5Hash(oldValue);

		var rows = await context.Database.ExecuteSqlRawAsync(sql,
		[
			new Microsoft.Data.SqlClient.SqlParameter("newValue", cacheData.Value),
			new Microsoft.Data.SqlClient.SqlParameter("newValueHash", cacheData.ValueHash),
			new Microsoft.Data.SqlClient.SqlParameter("key", cacheData.Key),
			new Microsoft.Data.SqlClient.SqlParameter("keyPrefix", cacheData.KeyPrefix450),
			new Microsoft.Data.SqlClient.SqlParameter("expiresUtc", cacheData.ExpiresUtc ?? (object)DBNull.Value),
			new Microsoft.Data.SqlClient.SqlParameter("sliding", cacheData.SlidingTime ?? (object)DBNull.Value),
			new Microsoft.Data.SqlClient.SqlParameter("now", nowUtc),
			new Microsoft.Data.SqlClient.SqlParameter("keyHash", cacheData.KeyHash),
			new Microsoft.Data.SqlClient.SqlParameter("oldValueHash", oldValueHash),
			new Microsoft.Data.SqlClient.SqlParameter("currentRowVersion", currentRowVersion)
		]);

		return rows == 1;
	}

	public async Task<bool> RemoveAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);

		var keyHash = HashHelper.ComputeMD5Hash(key);

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.SqlServer.ICacheDbContext>(scopeContext);
		var rows = await context.CacheData.Where(x => x.KeyHash == keyHash).ExecuteDeleteAsync(cancellationToken);

		return rows == 1;
	}

	public async Task DeleteExpiredAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var utcNow = GlobalContext.Instance.UtcNow;

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.SqlServer.ICacheDbContext>(scopeContext);
		await context.CacheData.Where(x => x.ExpiresUtc <= utcNow).ExecuteDeleteAsync(cancellationToken);
	}
}
