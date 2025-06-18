namespace Legion.ADF.ESB.Components;

public partial interface IComponentsQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IComponentsQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IComponentsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
