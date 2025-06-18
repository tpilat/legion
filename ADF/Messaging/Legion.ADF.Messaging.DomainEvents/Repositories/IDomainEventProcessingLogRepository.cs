namespace Legion.ADF.Messaging.DomainEvents.Model.Repositories;

public partial interface IDomainEventProcessingLogRepository : Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>? AccessControlManager { get; }

	Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog.IGetAllDomainEventProcessingLogsByIdDomainEvent GetAllDomainEventProcessingLogsByIdDomainEvent(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEventQuery getAllDomainEventProcessingLogsByIdDomainEventQuery);
}
