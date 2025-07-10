using Legion.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.PostgreSQL.Model.Repositories;

public partial class DistributedLockRepository : Legion.ADF.Cache.PostgreSQL.CacheRepositoryBase, Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.DistributedLock>, Legion.ADF.Cache.Model.Repositories.IDistributedLockRepository
{
	public async Task<Cache.Model.DistributedLock?> TryGetDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);

		var keyHash = HashHelper.ComputeMD5Hash(key);
		var nowUtc = GlobalContext.Instance.UtcNow;

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
		var distributedLock = await context.DistributedLock.Where(x => x.KeyHash == keyHash).FirstOrDefaultAsync();
		if (distributedLock == null)
			return null;

		if (distributedLock.ExpiresUtc <= nowUtc)
		{
			context.DistributedLock.Remove(distributedLock);
			await context.SaveAsync(scopeContext);
			return null;
		}

		return distributedLock;
	}

	public async Task<Cache.Model.DistributedLock?> TryAcquireDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		TimeSpan timeout,
		string? metadata,
		TimeSpan? retryDelay = null,
		int? maxRetries = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);
		Throw.IfLessThanOrEqual(timeout, TimeSpan.Zero);

		if (retryDelay.HasValue)
			Throw.IfLessThanOrEqual(retryDelay.Value, TimeSpan.Zero);

		if (maxRetries.HasValue)
			Throw.IfLessThanOrEqual(maxRetries.Value, 0);

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);

		var attempt = 0;

		do
		{
			var createResult = Legion.ADF.Cache.Model.DistributedLock.Create(
				scopeContext,
				key,
				timeout,
				metadata);

			createResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			var distributedLock = createResult.Data!;

			var sql = @"
				INSERT INTO cache.""DistributedLock"" (""KeyHash"", ""LockKey"", ""LockId"", ""Metadata"", ""ExpiresUtc"")
				VALUES(@keyHash, @lockKey, @lockId, @metadata, @expires)
				ON CONFLICT(""KeyHash"") DO NOTHING;";

			var affected = await context.Database.ExecuteSqlRawAsync(sql, [
				new Npgsql.NpgsqlParameter("keyHash", distributedLock.KeyHash),
				new Npgsql.NpgsqlParameter("lockKey", distributedLock.LockKey),
				new Npgsql.NpgsqlParameter("lockId", distributedLock.LockId),
				new Npgsql.NpgsqlParameter("metadata", distributedLock.Metadata),
				new Npgsql.NpgsqlParameter("expires", distributedLock.ExpiresUtc)
			], cancellationToken);

			if (affected == 1)
				return distributedLock;

			if (!maxRetries.HasValue)
				return null;

			attempt++;
			if (maxRetries < attempt)
				return null;

			retryDelay ??= TimeSpan.FromMilliseconds(200);

			await Task.Delay(retryDelay.Value, cancellationToken);

		} while (true);
	}

	public async Task<string?> TryAcquireDistributedLockIdAsync(
		IScopeContext scopeContext,
		string key,
		TimeSpan timeout,
		string? metadata,
		TimeSpan? retryDelay = null,
		int? maxRetries = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var distributedLock = await TryAcquireDistributedLockAsync(
			scopeContext,
			key,
			timeout,
			metadata,
			retryDelay,
			maxRetries,
			cancellationToken);

		return distributedLock?.LockId;
	}

	public async Task<bool> ReleaseDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		string lockId,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);

		var keyHash = HashHelper.ComputeMD5Hash(key);

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
		var affected = await context.DistributedLock.Where(x => x.KeyHash == keyHash && x.LockId == lockId).ExecuteDeleteAsync(cancellationToken);

		return affected == 1;
	}

	public async Task<bool> RenewDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		string lockId,
		TimeSpan timeout,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfNullOrWhiteSpace(key);
		Throw.IfLessThanOrEqual(timeout, TimeSpan.Zero);

		var expiresUtc = GlobalContext.Instance.UtcNow.Add(timeout);

		var keyHash = HashHelper.ComputeMD5Hash(key);

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
		var affected = await context.DistributedLock
			.Where(x => x.KeyHash == keyHash && x.LockId == lockId)
			.ExecuteUpdateAsync(
				x => x.SetProperty(p => p.ExpiresUtc, p => expiresUtc),
				cancellationToken);

		return affected == 1;
	}

	public async Task DeleteExpiredAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var utcNow = GlobalContext.Instance.UtcNow;

		await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Cache.PostgreSQL.ICacheDbContext>(scopeContext);
		await context.DistributedLock.Where(x => x.ExpiresUtc <= utcNow).ExecuteDeleteAsync(cancellationToken);
	}
}
