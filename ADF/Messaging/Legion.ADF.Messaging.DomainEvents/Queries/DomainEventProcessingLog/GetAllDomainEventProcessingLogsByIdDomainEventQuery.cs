using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog;

public record GetAllDomainEventProcessingLogsByIdDomainEventQuery(
	Guid IdDomainEvent,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<DomainEvents.Model.DomainEventProcessingLog>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.DomainEventProcessingLog>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<DomainEvents.Model.DomainEventProcessingLog, List<DomainEvents.Model.DomainEventProcessingLog>>;
