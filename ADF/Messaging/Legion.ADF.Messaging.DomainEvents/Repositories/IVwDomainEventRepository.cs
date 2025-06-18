namespace Legion.ADF.Messaging.DomainEvents.Model.Repositories;

public partial interface IVwDomainEventRepository : Legion.ADF.Messaging.DomainEvents.IDomainEventsQueryRepository<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent>? AccessControlManager { get; }

}
