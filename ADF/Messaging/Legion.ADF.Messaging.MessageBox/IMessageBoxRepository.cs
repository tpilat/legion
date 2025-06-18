namespace Legion.ADF.Messaging.MessageBox;

public partial interface IMessageBoxRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IMessageBoxRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IMessageBoxRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
