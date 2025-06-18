namespace Legion.ADF.ESB.Components;

public partial interface IComponentsRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IComponentsRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IComponentsRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
