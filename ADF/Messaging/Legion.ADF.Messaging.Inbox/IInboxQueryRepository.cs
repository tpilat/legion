namespace Legion.ADF.Messaging.Inbox;

public partial interface IInboxQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IInboxQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IInboxQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
