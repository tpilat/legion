namespace Legion.Model.Repositories;

public partial interface IEntityRepositoryBase
{
}

public interface IEntityRepositoryBase<T> : IEntityRepositoryBase
{
	IQueryable<T> AsQueryable(IScopeContext scopeContext);

	IQueryable<T> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions);

	IQueryable<T> AsReadOnlyQueryable(IScopeContext scopeContext);

	IQueryable<T> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions);

	void Add(IScopeContext scopeContext, T entity);

	Task AddAsync(
		IScopeContext scopeContext,
		T entity,
		CancellationToken cancellationToken = default);

	void AddRange(IScopeContext scopeContext, IEnumerable<T> entities);

	Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<T> entities,
		CancellationToken cancellationToken = default);

	void Remove(IScopeContext scopeContext, T entity);

	void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<T> entities);

	ulong BulkInsert(
		IEnumerable<T> entities,
		bool allowCreateNewDbConnection = false);
}
