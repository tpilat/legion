namespace Legion.ADF.Messaging.Outbox;

public partial interface IOutboxRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IOutboxRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IOutboxRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
