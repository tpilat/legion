using Legion.ADF.Cache.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.Queries.DistributedLock;

public class GetDistributedLockByKeyHash :
	QueryDefinition<
		ICacheDbContext,
		Cache.Model.DistributedLock,
		Cache.Model.DistributedLock?,
		GetDistributedLockByKeyHashQuery>,
		IGetDistributedLockByKeyHash
{
	public GetDistributedLockByKeyHash(
		IEFConnectionProvider connectionProvider,
		GetDistributedLockByKeyHashQuery getDistributedLockByKeyHash)
		: base(connectionProvider, getDistributedLockByKeyHash)
	{
		Throw.IfArgumentNullOrWhiteSpace(getDistributedLockByKeyHash?.KeyHash);
	}

	protected override IQueryable<Cache.Model.DistributedLock> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.DistributedLock;
	}

	public override IQueryable<Cache.Model.DistributedLock> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<ICacheAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.KeyHash == QueryRequest.KeyHash);
	}

	public override async Task<Cache.Model.DistributedLock?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Cache.Model.DistributedLock? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<bool> ExistsAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool Exists(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Any();
	}

	public async Task<string?> GetMetadataAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(x => x.Metadata)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public string? GetMetadata(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Select(x => x.Metadata).FirstOrDefault();
	}
}
