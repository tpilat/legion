namespace Legion.ADF.Messaging.DomainEvents;

public partial interface IDomainEventsQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Messaging.DomainEvents.Model.Repositories.IVwDomainEventRepository VwDomainEventRepository { get; }
}
