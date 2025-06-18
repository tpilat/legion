namespace Legion.Model.Repositories;

public partial interface IQueryRepositoryBase
{
}

public interface IQueryRepositoryBase<T> : IQueryRepositoryBase
{
	IQueryable<T> AsQueryable(IScopeContext scopeContext);

	IQueryable<T> AsReadOnlyQueryable(IScopeContext scopeContext);
}
