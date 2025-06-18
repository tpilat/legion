namespace Legion.ADF.Messaging.DomainEvents.Model.Repositories;

public partial interface IDomainEventRepository : Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>? AccessControlManager { get; }

	Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.IExistsDomainEventByIdDomainEvent ExistsDomainEventByIdDomainEvent(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.ExistsDomainEventByIdDomainEventQuery existsDomainEventByIdDomainEvent);

	Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.IGetDomainEventById GetDomainEventById(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetDomainEventByIdQuery getDomainEventById);

	Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.IGetNextDomainEvents GetNextDomainEvents(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetNextDomainEventsQuery getNextDomainEvents);
}
