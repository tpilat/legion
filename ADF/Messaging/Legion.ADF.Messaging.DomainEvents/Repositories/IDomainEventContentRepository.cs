namespace Legion.ADF.Messaging.DomainEvents.Model.Repositories;

public partial interface IDomainEventContentRepository : Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent>? AccessControlManager { get; }

	Legion.ADF.Messaging.DomainEvents.Queries.DomainEventContent.IGetDomainEventContentById GetDomainEventContentById(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEventContent.GetDomainEventContentByIdQuery getDomainEventContentById);
}
