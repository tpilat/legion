namespace Legion.ADF.Config;

public partial interface IConfigRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IConfigRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IConfigRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
