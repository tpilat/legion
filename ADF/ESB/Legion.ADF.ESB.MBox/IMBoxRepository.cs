namespace Legion.ADF.ESB.MBox;

public partial interface IMBoxRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IMBoxRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IMBoxRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
