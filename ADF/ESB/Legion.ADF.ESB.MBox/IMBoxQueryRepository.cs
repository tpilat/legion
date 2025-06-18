namespace Legion.ADF.ESB.MBox;

public partial interface IMBoxQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IMBoxQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IMBoxQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
