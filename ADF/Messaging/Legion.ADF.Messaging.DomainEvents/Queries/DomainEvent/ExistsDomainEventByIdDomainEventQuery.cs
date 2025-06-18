using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent;

public record ExistsDomainEventByIdDomainEventQuery(
	Guid IdDomainEvent,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<DomainEvents.Model.DomainEvent>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.DomainEvent>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<DomainEvents.Model.DomainEvent, bool>;
