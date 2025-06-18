namespace Legion.ADF.Auth;

public partial interface IAuthQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IAuthQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IAuthQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
