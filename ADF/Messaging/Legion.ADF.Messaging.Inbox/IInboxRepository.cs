namespace Legion.ADF.Messaging.Inbox;

public partial interface IInboxRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IInboxRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IInboxRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
