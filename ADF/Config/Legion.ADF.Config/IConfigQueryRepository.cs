namespace Legion.ADF.Config;

public partial interface IConfigQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IConfigQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IConfigQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
