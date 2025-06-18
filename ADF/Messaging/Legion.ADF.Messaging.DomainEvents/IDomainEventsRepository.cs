namespace Legion.ADF.Messaging.DomainEvents;

public partial interface IDomainEventsRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IDomainEventsRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IDomainEventsRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
