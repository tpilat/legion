namespace Legion.ADF.Messaging.DomainEvents;

public partial interface IDomainEventsQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IDomainEventsQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IDomainEventsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
