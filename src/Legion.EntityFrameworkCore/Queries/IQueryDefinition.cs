namespace Legion.EntityFrameworkCore.Queries;

public interface IQueryDefinition<TContext, T>
	where TContext : IDbContext
{
	//Task<TContext> GetContextAsync(
	//	QueryOptions<TContext> queryOptions,
	//	IScopeContext scopeContext,
	//	CancellationToken cancellationToken = default);

	//Task<IQueryable<T>> GetQueryAsync(
	//	IServiceProvider serviceProvider,
	//	IScopeContext scopeContext,
	//	CancellationToken cancellationToken = default);

	//Task<IQueryable<T>> GetQueryAsync(
	//	ContextFactory<TContext> factory,
	//	IScopeContext scopeContext,
	//	CancellationToken cancellationToken = default);

	//Task<IQueryable<T>> GetQueryAsync(
	//	QueryOptions<TContext> queryOptions,
	//	IScopeContext scopeContext,
	//	CancellationToken cancellationToken = default);
}
