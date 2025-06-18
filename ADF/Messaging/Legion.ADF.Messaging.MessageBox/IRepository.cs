namespace Legion.ADF.Messaging.MessageBox;

public partial interface IRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
