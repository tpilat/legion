namespace Legion.Model.Repositories;

public interface IUnitOfWorkProvider
{
	IResult<TUnitOfWork> Create<TUnitOfWork>(IScopeContext scopeContext)
		where TUnitOfWork : IUnitOfWork;

	IResult<TQueryUnitOfWork> CreateQuery<TQueryUnitOfWork>(IScopeContext scopeContext)
		where TQueryUnitOfWork : IQueryUnitOfWork;
}
