namespace Legion.ADF.Messaging.DomainEvents.Model.Repositories;

public partial interface IBlockedDomainEventTypeRepository : Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>? AccessControlManager { get; }

	Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType.IGetAllBlockedDomainEventTypes GetAllBlockedDomainEventTypes(
		Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType.GetAllBlockedDomainEventTypesQuery getAllBlockedDomainEventTypes);
}
