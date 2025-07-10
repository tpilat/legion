using Legion.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Legion.ADF.Cache.PostgreSQL.Model.Repositories;

public partial class CacheDataRepository : Legion.ADF.Cache.PostgreSQL.CacheRepositoryBase, Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.CacheData>, Legion.ADF.Cache.Model.Repositories.ICacheDataRepository
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

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
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

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);

		if (currentRowVersion.HasValue)
		{
			var sql = """
				INSERT INTO cache."CacheData" (
					"KeyHash", "ValueHash", "Key", "Value", "KeyPrefix450", "ExpiresUtc", "SlidingTime", "LastAccessedUtc", "RowVersion")
				VALUES (
					@p0, @p1, @p2, @p3,
					@p4, @p5, @p6, @p7, 0)
				ON CONFLICT ("KeyHash") DO UPDATE SET
					"ValueHash" = EXCLUDED."ValueHash",
					"Key" = EXCLUDED."Key",
					"Value" = EXCLUDED."Value",
					"KeyPrefix450" = EXCLUDED."KeyPrefix450",
					"ExpiresUtc" = EXCLUDED."ExpiresUtc",
					"SlidingTime" = EXCLUDED."SlidingTime",
					"LastAccessedUtc" = EXCLUDED."LastAccessedUtc",
					"RowVersion" = cache."CacheData"."RowVersion" + 1
				WHERE cache."CacheData"."RowVersion" = @p8;
			""";

			var rows = await context.Database.ExecuteSqlRawAsync(sql, [
				cacheData.KeyHash,
				cacheData.ValueHash,
				cacheData.Key,
				cacheData.Value,
				cacheData.KeyPrefix450,
				cacheData.ExpiresUtc,
				cacheData.SlidingTime,
				cacheData.LastAccessedUtc,
				currentRowVersion
				]);

			return rows == 1;
		}
		else
		{
			var sql = """
				INSERT INTO cache."CacheData" (
					"KeyHash", "ValueHash", "Key", "Value", "KeyPrefix450", "ExpiresUtc", "SlidingTime", "LastAccessedUtc", "RowVersion")
				VALUES (
					@p0, @p1, @p2, @p3,
					@p4, @p5, @p6, @p7, 0)
				ON CONFLICT ("KeyHash") DO UPDATE SET
					"ValueHash" = EXCLUDED."ValueHash",
					"Key" = EXCLUDED."Key",
					"Value" = EXCLUDED."Value",
					"KeyPrefix450" = EXCLUDED."KeyPrefix450",
					"ExpiresUtc" = EXCLUDED."ExpiresUtc",
					"SlidingTime" = EXCLUDED."SlidingTime",
					"LastAccessedUtc" = EXCLUDED."LastAccessedUtc",
					"RowVersion" = cache."CacheData"."RowVersion" + 1;
			""";

			var rows = await context.Database.ExecuteSqlRawAsync(sql, [
				cacheData.KeyHash,
				cacheData.ValueHash,
				cacheData.Key,
				cacheData.Value,
				cacheData.KeyPrefix450,
				cacheData.ExpiresUtc,
				cacheData.SlidingTime,
				cacheData.LastAccessedUtc
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

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
		var sql = """
			UPDATE cache."CacheData"
			SET
				"Value" = @newValue,
				"ValueHash" = @newValueHash,
				"Key" = @key,
				"KeyPrefix450" = @keyPrefix,
				"ExpiresUtc" = @expiresUtc,
				"SlidingTime" = @sliding,
				"LastAccessedUtc" = @now,
				"RowVersion" = @currentRowVersion + 1
			WHERE "KeyHash" = @keyHash AND "ValueHash" = @oldValueHash AND "RowVersion" = @currentRowVersion;
		""";

		var oldValueHash = HashHelper.ComputeMD5Hash(oldValue);

		var rows = await context.Database.ExecuteSqlRawAsync(sql,
		[
			new Npgsql.NpgsqlParameter("newValue", cacheData.Value),
			new Npgsql.NpgsqlParameter("newValueHash", cacheData.ValueHash),
			new Npgsql.NpgsqlParameter("key", cacheData.Key),
			new Npgsql.NpgsqlParameter("keyPrefix", cacheData.KeyPrefix450),
			new Npgsql.NpgsqlParameter("expiresUtc", cacheData.ExpiresUtc ?? (object)DBNull.Value),
			new Npgsql.NpgsqlParameter("sliding", cacheData.SlidingTime ?? (object)DBNull.Value),
			new Npgsql.NpgsqlParameter("now", nowUtc),
			new Npgsql.NpgsqlParameter("keyHash", cacheData.KeyHash),
			new Npgsql.NpgsqlParameter("oldValueHash", oldValueHash),
			new Npgsql.NpgsqlParameter("currentRowVersion", currentRowVersion)
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

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
		var rows = await context.CacheData.Where(x => x.KeyHash == keyHash).ExecuteDeleteAsync(cancellationToken);

		return rows == 1;
	}

	public async Task DeleteExpiredAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var utcNow = GlobalContext.Instance.UtcNow;

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
		await context.CacheData.Where(x => x.ExpiresUtc <= utcNow).ExecuteDeleteAsync(cancellationToken);
	}
}
