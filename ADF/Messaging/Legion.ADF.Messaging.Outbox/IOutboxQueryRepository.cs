namespace Legion.ADF.Messaging.Outbox;

public partial interface IOutboxQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IOutboxQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IOutboxQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
