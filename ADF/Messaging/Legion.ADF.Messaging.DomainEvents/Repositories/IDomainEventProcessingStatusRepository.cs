namespace Legion.ADF.Messaging.DomainEvents.Model.Repositories;

public partial interface IDomainEventProcessingStatusRepository : Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus>? AccessControlManager { get; }

}
